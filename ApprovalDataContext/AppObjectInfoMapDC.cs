using ApprovalInterface;
using ApprovalModel;
using CommonInterface;
using CommonModel;
using ConnectionGateway;
using LS.LS.ModelBiz;
using PMSInterface;
using SupplierInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace ApprovalDataContext
{
    public class AppObjectInfoMapDC: IGeneralWaitingForApproval
    {
        ISupplierDbContext _supplierDbContext;
        ISupplierRepository _supplierRepository;
        IEmailHelper _emailHelper;
        IUploadedFileRepository _uploadedFileRepository;
        IPurchaseRequsitionRepository _purchaseRequsitionRepository;
        IRFPProcessingRepository _rFPProcessingRepository;
        public AppObjectInfoMapDC(ISupplierDbContext supplierDbContext, ISupplierRepository supplierRepository, IEmailHelper emailHelper, IUploadedFileRepository uploadedFileRepository, IPurchaseRequsitionRepository purchaseRequsitionRepository)
        {
            _supplierDbContext = supplierDbContext;
            _supplierRepository = supplierRepository;
            _emailHelper = emailHelper;
            _uploadedFileRepository = uploadedFileRepository;
            _purchaseRequsitionRepository = purchaseRequsitionRepository;
        }
        public static int GenerateApprovalFromOtherObject(int pModuleObjID, string pPKValue, string pUserCode, int pCompanyCode, SqlConnection objSqlConnection, SqlTransaction objTransaction)
        {
            SqlParameter Success = null;
            int vSPReturnValue = -1;
            SqlParameter IsFinalLevel = null;
            string vMSG = string.Empty;

            try
            {
                SqlCommand command = new SqlCommand("USP_APP_GenerateApproval", objSqlConnection, objTransaction);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@ModuleObjectID", SqlDbType.Int).Value = pModuleObjID;
                command.Parameters.Add("@PKValue", SqlDbType.VarChar).Value = pPKValue;
                command.Parameters.Add("@CompanyCode", SqlDbType.Int).Value = pCompanyCode;
                command.Parameters.Add("@UserCode", SqlDbType.VarChar).Value = pUserCode;
                Success = new SqlParameter("@Success", SqlDbType.Int);
                IsFinalLevel= new SqlParameter("@IsFinalLevel", SqlDbType.Int);
                Success.Direction= ParameterDirection.Output;
                IsFinalLevel.Direction= ParameterDirection.Output;
                command.Parameters.Add(Success);
                command.Parameters.Add(IsFinalLevel);

                command.ExecuteNonQuery();

                vSPReturnValue = Convert.ToInt32(command.Parameters["@Success"].Value.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

            return vSPReturnValue;
        }
        public List<GeneralWaitingForApproval> GetDashBoardData(string pUserId)
        {
            List<GeneralWaitingForApproval> objIGeneralWaitingForApprovalList = new List<GeneralWaitingForApproval>();
            string vComTxt = @"SELECT count(A.ModuleObjMapCode) NoOfOjectToApprove
                                ,A.ModuleObjMapCode
                                ,B.DashBoardCaption--,dbo.fxn_WorkingUnitByCode(A.CompanyCode) AS Company    
                                FROM General_WaitingForApproval A 
                                inner join LS_AppObjectInfoMap B on A.ModuleObjMapCode=B.ModuleObjMapCode And A.CompanyCode = B.CompanyCode  
                                where B.Active=1 AND AppUserCode='" + pUserId + @"'
                                and ResponseStatus=0                                
                                and A.ActionType <> 'Delete' 
                                group by A.ModuleObjMapCode,B.DashBoardCaption,A.CompanyCode ";
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            dr = objDbCommand.ExecuteReader();
            try
            {
                while (dr.Read())
                {
                    GeneralWaitingForApproval objIGeneralWaitingForApproval = new GeneralWaitingForApproval();
                    objIGeneralWaitingForApproval.ModuleObjMapCode = dr["ModuleObjMapCode"].ToString();
                    objIGeneralWaitingForApproval.NoOfObjectToApprove_VW = Convert.ToInt32(dr["NoOfOjectToApprove"]);
                    objIGeneralWaitingForApproval.DashBoardCaption_VW = dr["DashBoardCaption"].ToString();
                    //objIGeneralWaitingForApproval.CompanyName_VW = dr["Company"].ToString();
                    objIGeneralWaitingForApprovalList.Add(objIGeneralWaitingForApproval);
                }
            }
            catch (DbException ex)
            {
                
                    throw ex;
            }
            finally
            {

            }

            return objIGeneralWaitingForApprovalList;
        }

        public AppObjectInfoMap GetAppObjectInfoMapByMapCode(string pModuleObjMapCode)
        {
            AppObjectInfoMap objAppObjectInfoMap = new AppObjectInfoMap();
            string vComTxt = @"SELECT A.ModuleObjMapCode
                                ,A.ModuleCode
                                ,C.ModuleName
                                ,A.ModuleObjName
                                ,A.ModuleObjId
                                ,B.DBObjName
                                ,B.PKColumnName
                                ,B.DashBoardCaption  
                                ,B.PageCode
                                ,B.IsDynamicPage    
                                FROM LS_AppModuleObjectMapping A 
                                join  LS_AppObjectInfoMap B on A.ModuleObjMapCode=B.ModuleObjMapCode AND B.Active=1 AND A.CompanyCode = B.CompanyCode
                                join LS_Module C on C.ModuleCode=A.ModuleCode  
                                and A.ModuleObjMapCode='" + pModuleObjMapCode + @"'
                                and A.ActionType <> 'Delete' ";
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            dr = objDbCommand.ExecuteReader();
            try
            {
                while (dr.Read())
                {
                    objAppObjectInfoMap.ModuleObjMapCode_PK = dr["ModuleObjMapCode"].ToString();
                    objAppObjectInfoMap.ModuleObjName_VW = dr["ModuleObjName"].ToString();
                    objAppObjectInfoMap.ModuleObjId = Convert.ToInt32(dr["ModuleObjId"]);
                    objAppObjectInfoMap.DBObjName = dr["DBObjName"].ToString();
                    objAppObjectInfoMap.PKColumnName = dr["PKColumnName"].ToString();
                    objAppObjectInfoMap.DashBoardCaption = dr["DashBoardCaption"].ToString();

                    if (!string.IsNullOrEmpty(dr["PageCode"].ToString()))
                    {
                        objAppObjectInfoMap.PageCode = Convert.ToInt32(dr["PageCode"].ToString());
                    }

                    if (!string.IsNullOrEmpty(dr["IsDynamicPage"].ToString()))
                    {
                        objAppObjectInfoMap.IsDynamicPage = Convert.ToInt32(dr["IsDynamicPage"]); //changed by tinne
                    }
                    if (objAppObjectInfoMap != null)
                    {
                        objAppObjectInfoMap.AppObjInfoMap_GridList_VW = GetAppObjInfoMapGridListByMapCode(objAppObjectInfoMap.ModuleObjMapCode_PK, objAppObjectInfoMap);
                        objAppObjectInfoMap.AppObjectInfoMapping_Report_VW = GetAppObjectInfoMappingReportByMapCode(objAppObjectInfoMap.ModuleObjMapCode_PK);
                    }
                }
            }
            catch (DbException ex)
            {

                throw ex;
            }
            finally
            {

            }
            return objAppObjectInfoMap;
        }
        public List<AppObjInfoMap_Grid> GetAppObjInfoMapGridListByMapCode(string pModuleObjMapCode, AppObjectInfoMap objAppObjectInfoMap)
        {
            List<AppObjInfoMap_Grid> objAppObjInfoMap_GridList = new List<AppObjInfoMap_Grid>();

            string vComTxt = @"SELECT A.ObjAppInfoMapGridCode
                                ,A.ModuleObjMapCode
                                ,A.DBFieldName
                                ,A.DBFunctionMapCode
                                ,B.DBFunctionName
                                ,A.GridColumnHeaderText
                                ,A.GridColumnLength
                                ,A.IsOrderBy
                                ,A.ColumnSortOrder 
                                ,A.IsViewInMessage     
                                FROM LS_AppObjInfoMap_Grid A
                                left outer join dbo.LS_AppDBFunctionMapping B
                                on A.DBFunctionMapCode = B.DBFunctionMapCode
                                where A.ModuleObjMapCode='" + pModuleObjMapCode + @"'
                                and A.ActionType <> 'Delete'                             
                                order by A.ColumnSortOrder ";
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            dr = objDbCommand.ExecuteReader();
            try
            {
                while (dr.Read())
                {
                    AppObjInfoMap_Grid objIAppObjInfoMap_Grid = new AppObjInfoMap_Grid();
                    objIAppObjInfoMap_Grid.ObjAppInfoMapGridCode_PK = dr["ObjAppInfoMapGridCode"].ToString();
                    objIAppObjInfoMap_Grid.ModuleObjMapCode_FK = dr["ModuleObjMapCode"].ToString();
                    objIAppObjInfoMap_Grid.DBFieldName = dr["DBFieldName"].ToString();

                    if (!string.IsNullOrEmpty(dr["DBFunctionMapCode"].ToString()))
                    {
                        objIAppObjInfoMap_Grid.DBFunctionMapCode_FK = dr["DBFunctionMapCode"].ToString();
                    }

                    if (!string.IsNullOrEmpty(dr["DBFunctionName"].ToString()))
                    {
                        objIAppObjInfoMap_Grid.DBFunctionName_VW = dr["DBFunctionName"].ToString();
                    }

                    objIAppObjInfoMap_Grid.GridColumnHeaderText = dr["GridColumnHeaderText"].ToString();
                    objIAppObjInfoMap_Grid.GridColumnLength = Convert.ToInt32(dr["GridColumnLength"]);
                    objIAppObjInfoMap_Grid.IsOrderBy = Convert.ToInt32(dr["IsOrderBy"]);
                    objIAppObjInfoMap_Grid.ColumnSortOrder = Convert.ToInt32(dr["ColumnSortOrder"]);

                    if (!string.IsNullOrEmpty(dr["IsViewInMessage"].ToString()))
                    {
                        objIAppObjInfoMap_Grid.IsViewInMessage = Convert.ToInt32(dr["IsViewInMessage"]);
                    }

                    objAppObjInfoMap_GridList.Add(objIAppObjInfoMap_Grid);

                }
            }
            catch (DbException ex)
            {

                throw ex;
            }
            finally
            {

            }
            return objAppObjInfoMap_GridList;
        }
        public AppObjectInfoMapping_Report GetAppObjectInfoMappingReportByMapCode(string pModuleObjMapCode)
        {
            AppObjectInfoMapping_Report objAppObjectInfoMapping_Report = new AppObjectInfoMapping_Report();

            string vComTxt = @"SELECT A.ObjAppInfoMapRptCode
                                ,A.ModuleObjMapCode
                                ,A.ReportCode
                                ,B.ReportName
                                ,B.ReportId      
                                FROM LS_AppObjectInfoMapping_Report A 
                                inner join LS_ReportInformation B on A.ReportCode=B.ReportCode
                                where A.ModuleObjMapCode='" + pModuleObjMapCode + @"'
                                and A.ActionType <> 'Delete'";
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            dr = objDbCommand.ExecuteReader();

            try
            {
                while (dr.Read())
                {
                    objAppObjectInfoMapping_Report.ObjAppInfoMapRptCode_PK = dr["ObjAppInfoMapRptCode"].ToString();
                    objAppObjectInfoMapping_Report.ModuleObjMapCode = dr["ModuleObjMapCode"].ToString();
                    objAppObjectInfoMapping_Report.ReportCode = dr["ReportCode"].ToString();
                    objAppObjectInfoMapping_Report.ReportName_VW = dr["ReportName"].ToString();
                    objAppObjectInfoMapping_Report.ReportId_VW = dr["ReportId"].ToString();
                }
                if (!string.IsNullOrEmpty(objAppObjectInfoMapping_Report.ObjAppInfoMapRptCode_PK))
                {
                    objAppObjectInfoMapping_Report.AppObjInfoMap_RptParamsList_VW = GetAppObjInfoMap_RptParamsByMapRptCode(objAppObjectInfoMapping_Report.ObjAppInfoMapRptCode_PK);
                }

            }
            catch (DbException ex)
            {

                throw ex;
            }
            finally
            {

            }
            return objAppObjectInfoMapping_Report;
        }
        public  List<AppObjInfoMap_RptParams> GetAppObjInfoMap_RptParamsByMapRptCode(string pObjAppInfoMapRptCode)
        {
            List<AppObjInfoMap_RptParams> objAppObjInfoMap_RptParamsList = new List<AppObjInfoMap_RptParams>();
            string vComTxt = @"SELECT ObjectAppInfoMapRptParamCode
                                ,ObjAppInfoMapRptCode
                                ,RptParameterName
                                ,DBFieldForParameterValue
                                FROM LS_AppObjInfoMap_RptParams
                                where ObjAppInfoMapRptCode ='" + pObjAppInfoMapRptCode + @"'
                                and ActionType <> 'Delete'";
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            dr = objDbCommand.ExecuteReader();
            while (dr.Read())
            {
                AppObjInfoMap_RptParams objAppObjInfoMap_RptParams = new AppObjInfoMap_RptParams();

                objAppObjInfoMap_RptParams.ObjectAppInfoMapRptParamCode_PK = dr["ObjectAppInfoMapRptParamCode"].ToString();
                objAppObjInfoMap_RptParams.ObjAppInfoMapRptCode_FK = dr["ObjAppInfoMapRptCode"].ToString();
                objAppObjInfoMap_RptParams.RptParameterName = dr["RptParameterName"].ToString();
                objAppObjInfoMap_RptParams.DBFieldForParameterValue = dr["DBFieldForParameterValue"].ToString();
                objAppObjInfoMap_RptParams.IsNew = false;
                objAppObjInfoMap_RptParamsList.Add(objAppObjInfoMap_RptParams);
            }
            return objAppObjInfoMap_RptParamsList;
        }
        public List<AppObjectInfoMap> GetObjectListWithDataForApproval(string pModuleObjMapCode, AppObjectInfoMap objAppObjectInfoMap, int pApprovalStatus, string pFromDate, string pToDate, string pUserCode)
        {
            List<AppObjectInfoMap> objAppObjectInfoMapList = new List<AppObjectInfoMap>();
            int pResponseStatus = 0;

            if (objAppObjectInfoMap == null || objAppObjectInfoMap.AppObjInfoMap_GridList_VW == null || objAppObjectInfoMap.AppObjInfoMap_GridList_VW.Count == 0)
            {
                return objAppObjectInfoMapList;
            }

            if (pApprovalStatus != -1)
            {
                pResponseStatus = 1;
            }
            StringBuilder vComTxt = new StringBuilder();
            string vOrderByClause = "";
            vComTxt.Append(@"SELECT  A.ApprovalWaitingCode, A.ModuleObjMapCode, A.ObjPKValue, A.AppLevel, A.AppProcessBatch, A.AppUserCode, A.IsFinalLevel, A.ResponseStatus ,ISNULL(C.AppStatus, 0) as ApprovalStatus
                            , C.ResponseRemarks,C1.AppUserCode, E.Name as PreviousApprover ");
            foreach (AppObjInfoMap_Grid grid in objAppObjectInfoMap.AppObjInfoMap_GridList_VW)
            {
                if (!string.IsNullOrEmpty(grid.DBFunctionMapCode_FK))
                {
                    vComTxt.Append(", dbo." + grid.DBFunctionName_VW + "( B." + grid.DBFieldName + ") as '" + grid.GridColumnHeaderText + "'");
                }
                else
                {
                    vComTxt.Append(", B." + grid.DBFieldName + " as '" + grid.GridColumnHeaderText + "'");
                }
                if (grid.IsOrderBy == 1)
                {
                    if (string.IsNullOrEmpty(vOrderByClause))
                    {
                        vOrderByClause = "  Order by " + grid.DBFieldName;
                    }
                    else
                    {
                        vOrderByClause = vOrderByClause + " ," + grid.DBFieldName;
                    }
                }
            }
            if (objAppObjectInfoMap.AppObjectInfoMapping_Report_VW != null && objAppObjectInfoMap.AppObjectInfoMapping_Report_VW.AppObjInfoMap_RptParamsList_VW != null && objAppObjectInfoMap.AppObjectInfoMapping_Report_VW.AppObjInfoMap_RptParamsList_VW.Count > 0)
            {
                foreach (AppObjInfoMap_RptParams param in objAppObjectInfoMap.AppObjectInfoMapping_Report_VW.AppObjInfoMap_RptParamsList_VW)
                {
                    vComTxt.Append(" , B." + param.DBFieldForParameterValue);
                }
            }
            vComTxt.Append(@" FROM General_WaitingForApproval A ");
            vComTxt.Append(@"join " + objAppObjectInfoMap.DBObjName + " B on A.ObjPKValue= B." + objAppObjectInfoMap.PKColumnName + " ");
            vComTxt.Append(@"left outer join General_ApprovalInfo C on C.ApprovalWaitingCode=A.ApprovalWaitingCode
                                left outer join General_ApprovalInfo C1 on C1.ObjPKValue=A.ObjPKValue AND C1.AppStatus = 1 and C1.AppLevel= (A.AppLevel-1)
                                left outer join General_UserMapEmployee UME on UME.UserId=C1.AppUserCode
                                left outer join HR_Employee E on E.EmployeeCode=UME.EmployeeCode
                                where A.ModuleObjMapCode = '" + pModuleObjMapCode + @"' 
                                and A.ResponseStatus = " + pResponseStatus + @"
                                and A.ActionType <> 'Delete'");
            if (objAppObjectInfoMap.IsDynamicPage == 0)
            {
                vComTxt.Append(@"and B.ActionType <> 'Delete'");
            }
            vComTxt.Append(@"and A.AppUserCode='" + pUserCode + @"'");

            if (pApprovalStatus != -1)
            {
                vComTxt.Append(@" AND C.AppStatus = " + pApprovalStatus);
            }
            if (!String.IsNullOrEmpty(pFromDate) && !String.IsNullOrEmpty(pToDate))
            {
                vComTxt.Append(@" AND CONVERT(DATETIME,CONVERT(VARCHAR(11),A.ActionDate),111) BETWEEN CONVERT(DATETIME,CONVERT(VARCHAR(11),'" + pFromDate + "'),111) AND CONVERT(DATETIME,CONVERT(VARCHAR(11),'" + pToDate + "'),111) ");
            }

            if (!string.IsNullOrEmpty(vOrderByClause))
            {
                vComTxt.Append(vOrderByClause);
            }
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt.ToString(), connection);
            dr = objDbCommand.ExecuteReader();
            try
            {
                while (dr.Read())
                {
                    AppObjectInfoMap objAppObjectInfoMapNew= (AppObjectInfoMap)objAppObjectInfoMap.DeepClone();//new AppObjectInfoMap();
                    objAppObjectInfoMapNew.ApprovalWaitingCode_VW = dr["ApprovalWaitingCode"].ToString();
                    objAppObjectInfoMapNew.ModuleObjMapCode_PK = dr["ModuleObjMapCode"].ToString();
                    objAppObjectInfoMapNew.PKColumnValue_VW = dr["ObjPKValue"].ToString();
                    objAppObjectInfoMapNew.ApprovalLevel_VW = Convert.ToInt32(dr["AppLevel"]);
                    objAppObjectInfoMapNew.AppProcessBatch_VW = Convert.ToInt32(dr["AppProcessBatch"]);
                    objAppObjectInfoMapNew.ApprovalStatus_VW = Convert.ToInt32(dr["ApprovalStatus"]);
                    objAppObjectInfoMapNew.IsFinalLevelApproval_VW = Convert.ToInt32(dr["IsFinalLevel"]);
                    objAppObjectInfoMapNew.ResponseStatus_VW = Convert.ToInt32(dr["ResponseStatus"]);

                    if (dr["ResponseRemarks"] != DBNull.Value)
                    {
                        objAppObjectInfoMapNew.Remarks_VW = dr["ResponseRemarks"].ToString();
                    }

                    if (dr["PreviousApprover"] != DBNull.Value)
                    {
                        objAppObjectInfoMapNew.PreviousApprover_VW = dr["PreviousApprover"].ToString();
                    }
                    foreach (AppObjInfoMap_Grid grid in objAppObjectInfoMapNew.AppObjInfoMap_GridList_VW)
                    {
                        if (dr[grid.GridColumnHeaderText] != DBNull.Value)
                        {
                            try
                            {
                                grid.DBFieldValue_VW = dr.GetDateTime(dr.GetOrdinal(grid.GridColumnHeaderText)).ToString("dd-MM-yyyy"); 
                            }
                            catch
                            {
                                grid.DBFieldValue_VW = dr[grid.GridColumnHeaderText].ToString();
                            }
                        }
                    }
                    if (objAppObjectInfoMapNew.AppObjectInfoMapping_Report_VW != null && objAppObjectInfoMapNew.AppObjectInfoMapping_Report_VW.AppObjInfoMap_RptParamsList_VW != null && objAppObjectInfoMapNew.AppObjectInfoMapping_Report_VW.AppObjInfoMap_RptParamsList_VW.Count > 0)
                    {
                        foreach (AppObjInfoMap_RptParams param in objAppObjectInfoMapNew.AppObjectInfoMapping_Report_VW.AppObjInfoMap_RptParamsList_VW)
                        {
                            if (dr[param.DBFieldForParameterValue] != DBNull.Value)
                            {
                                param.DBFieldValue_VW = dr[param.DBFieldForParameterValue].ToString();
                            }
                        }
                    }
                    objAppObjectInfoMapList.Add(objAppObjectInfoMapNew);
                }
            }
            catch (DbException ex)
            {

                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return objAppObjectInfoMapList;
        }
        public string SaveApprovalData(List<GeneralWaitingForApproval> objIGeneralWaitingForApprovalList)
        {
            int pIsCommit = 0;
            int vSPReturnValue = -1;
            int vIsFinalLevel = -1;
            SqlParameter Success = null;
            SqlParameter IsFinalLevel = null;
            string vMSG = string.Empty;
            string vMSGFromOtherObject = string.Empty;
            List<string> FinalApprovedSOList = new List<string>();
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            using (SqlTransaction trans = connection.BeginTransaction())
            {
                try
                {

                    foreach (GeneralWaitingForApproval objIGeneralWaitingForApproval in objIGeneralWaitingForApprovalList)
                    {
                        SqlCommand command = new SqlCommand("USP_APP_GenerateApproval", connection, trans);
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@ModuleObjectID", SqlDbType.Int).Value = objIGeneralWaitingForApproval.AppModuleObjID;
                        command.Parameters.Add("@PKValue", SqlDbType.VarChar).Value = objIGeneralWaitingForApproval.ObjPKValue;
                        command.Parameters.Add("@CompanyCode", SqlDbType.Int).Value = objIGeneralWaitingForApproval.CompanyCode_FK;
                        command.Parameters.Add("@UserCode", SqlDbType.VarChar).Value = objIGeneralWaitingForApproval.UserCode;
                        command.Parameters.Add("@CurrentAppLevel", SqlDbType.Int).Value = objIGeneralWaitingForApproval.AppLevel;
                        command.Parameters.Add("@CurrentAppBatch", SqlDbType.Int).Value = objIGeneralWaitingForApproval.AppProcessBatch;
                        command.Parameters.Add("@WaitingAppCode", SqlDbType.VarChar).Value = objIGeneralWaitingForApproval.ApprovalWaitingCode;
                        command.Parameters.Add("@UserAPPStatus", SqlDbType.Int).Value = objIGeneralWaitingForApproval.ApprovalStatus_VW;
                        command.Parameters.Add("@ResponseRemarks", SqlDbType.VarChar).Value = objIGeneralWaitingForApproval.ResponseRemarks;

                        Success = new SqlParameter("@Success", SqlDbType.Int);
                        IsFinalLevel = new SqlParameter("@IsFinalLevel", SqlDbType.Int);
                        Success.Direction = ParameterDirection.Output;
                        IsFinalLevel.Direction = ParameterDirection.Output;
                        command.Parameters.Add(Success);
                        command.Parameters.Add(IsFinalLevel);

                        command.ExecuteNonQuery();
                        vSPReturnValue = Convert.ToInt32(command.Parameters["@Success"].Value.ToString());
                        vIsFinalLevel = Convert.ToInt32(command.Parameters["@IsFinalLevel"].Value.ToString());

                        if (!string.IsNullOrEmpty(vMSG))
                        {
                            vMSG = vMSG + ",";
                        }
                        if (vSPReturnValue == 1)
                        {
                            if (vIsFinalLevel == 1 && (objIGeneralWaitingForApproval.ApprovalStatus_VW != 3 && objIGeneralWaitingForApproval.ApprovalStatus_VW != 2))
                            {
                                objIGeneralWaitingForApproval.ApprovalStatus_VW = 1;
                                objIGeneralWaitingForApproval.IsFinalLevel = 1;
                            }
                            vMSGFromOtherObject = CallIndividualObjectApproval(objIGeneralWaitingForApproval, connection, trans);
                            if (!string.IsNullOrEmpty(vMSGFromOtherObject))
                            {
                                vMSG = vMSG + objIGeneralWaitingForApproval.ObjPKValue + ":1";

                                if (vIsFinalLevel == 1)// archive  to send mail for these order on last level approval
                                {
                                    _emailHelper.SendMail(objIGeneralWaitingForApproval.ObjPKValue, objIGeneralWaitingForApproval.AppModuleObjID, objIGeneralWaitingForApproval.UserCode);
                                    FinalApprovedSOList.Add(objIGeneralWaitingForApproval.ObjPKValue);
                                }
                            }
                            else
                            {
                                string vExcp = "";
                                if (vMSGFromOtherObject.Contains(EnumMessageId.LS205.ToString()) == true || vMSGFromOtherObject.Contains(EnumMessageId.LS251.ToString()) == true || vMSGFromOtherObject.Contains(EnumMessageId.LS200.ToString()) == true)
                                {
                                    vExcp = vMSGFromOtherObject;
                                }

                                if (string.IsNullOrEmpty(vExcp))
                                {
                                    throw new Exception("Exception occured while approving individual object");
                                }
                                else
                                {
                                    throw new Exception("Exception occured while approving individual object: \n" + vExcp);
                                }
                            }
                        }
                        else if (vSPReturnValue == 2)
                        {
                            vMSG = vMSG + objIGeneralWaitingForApproval.ObjPKValue + ":2";
                            trans.Rollback();
                        }
                        else if (vSPReturnValue == 3)
                        {
                            vMSG = vMSG + objIGeneralWaitingForApproval.ObjPKValue + ":3";
                        }
                        else if (vSPReturnValue == 0)
                        {
                            vMSG = vMSG + objIGeneralWaitingForApproval.ObjPKValue + ":0";
                            break;
                        }
                    }
                    trans.Commit();
                    pIsCommit = 1;
                }

                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }

            }
            return vMSG;

        }
        public  string CallIndividualObjectApproval(GeneralWaitingForApproval objIGeneralWaitingForApproval, SqlConnection objConnection, SqlTransaction objTransaction)
        {
            string vMSG = string.Empty;
            int vApprovalAction = 0;
            bool vIsApproved = false;
            vApprovalAction = objIGeneralWaitingForApproval.ApprovalStatus_VW;
            string object_id = string.Empty;
            if (objIGeneralWaitingForApproval.ApprovalStatus_VW == 1 && objIGeneralWaitingForApproval.IsFinalLevel == 1)
            {
                vIsApproved = true;
            }
            if (objIGeneralWaitingForApproval.AppModuleObjID == Convert.ToInt32(EnumModuleObjectId.SupplierProfile))
            {
                SupplierInfo objSupplierInfo = new SupplierInfo();
                objSupplierInfo = _supplierRepository.GetSupplierInfo(objIGeneralWaitingForApproval.ObjPKValue);
                object_id = objSupplierInfo.SupplierID;
                vMSG = _supplierRepository.ApproveActionUpdate(objIGeneralWaitingForApproval.ObjPKValue, vApprovalAction, vIsApproved, objConnection, objTransaction);
            }
            if (objIGeneralWaitingForApproval.AppModuleObjID == Convert.ToInt32(EnumModuleObjectId.SupplierDocuments))
            {
                
                vMSG = _uploadedFileRepository.ApproveActionUpdate(objIGeneralWaitingForApproval.ObjPKValue, vApprovalAction, vIsApproved, objConnection, objTransaction);
            }
            if (objIGeneralWaitingForApproval.AppModuleObjID == Convert.ToInt32(EnumModuleObjectId.RFPProcess))
            {

                vMSG = _purchaseRequsitionRepository.ApproveActionUpdate(objIGeneralWaitingForApproval.ObjPKValue, vApprovalAction, vIsApproved, objConnection, objTransaction);
            }
            return object_id;
        }
    }
    
}
