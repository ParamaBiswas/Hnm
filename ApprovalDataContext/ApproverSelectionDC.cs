using ConnectionGateway;
using System;
using System.Collections.Generic;
using ApprovalModel;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.Data.Common;
using LSCrud;
using ApprovalInterface;

namespace ApprovalDataContext
{
    public class ApproverSelectionDC: IAppLevelDefDetAppType
    {
        ISupplierDbContext _supplierDbContext;
        ICRUD _CRUDBuilder;
        static readonly string ObjAppLevelDefDetAppTypeTable = "LS_AppLevelDefDetAppType";
        static readonly string ObjAppLevelDetAppTypeApproverTable = "LS_AppLevelDetAppTypeApprover";
        public ApproverSelectionDC(ISupplierDbContext supplierDbContext, ICRUD CRUDBuilder)
        {
            _supplierDbContext = supplierDbContext;
            _CRUDBuilder = CRUDBuilder;
        }
        public List<AppLevelDefDetAppType> GetApproverSelection(string appLvDefinitionDetCode_FK)
        {
            //string vComTxt = string.Empty;
           // string vOut = "Exception Occured !";



            List<AppLevelDefDetAppType> objIAppLevelDefDetAppType_List = new List<AppLevelDefDetAppType>();

            string vComTxt = @"SELECT [AppLvDefDetAppTypeCode]
                                      ,[AppLvDefinitionDetCode]
                                      ,[ApproverLevelNo]
                                      ,[ApproverType]
                                      ,[CompanyCode]
                                      ,[UserCode]
                                      ,[ActionDate]
                                      ,[ActionType]
                           FROM  LS_AppLevelDefDetAppType  
                            where AppLvDefinitionDetCode='" + appLvDefinitionDetCode_FK + @"'
                           and  ActionType <> 'Delete' 
                           AND  CompanyCode=1"
                          + @" order by ApproverLevelNo";
            SqlConnection connection = _supplierDbContext.GetConn();

            try
            {
               
            connection.Open();
            SqlDataReader dr;
            SqlCommand command = new SqlCommand(vComTxt, connection);
            dr = command.ExecuteReader();
            while (dr.Read())
                    {
                       
                        AppLevelDefDetAppType objIAppLevelDefDetAppType_temp = new AppLevelDefDetAppType();

                        if (dr["AppLvDefDetAppTypeCode"] != DBNull.Value)
                            objIAppLevelDefDetAppType_temp.AppLvDefDetAppTypeCode_PK = dr["AppLvDefDetAppTypeCode"].ToString();

                        if (dr["AppLvDefinitionDetCode"] != DBNull.Value)
                            objIAppLevelDefDetAppType_temp.AppLvDefinitionDetCode_FK = dr["AppLvDefinitionDetCode"].ToString();

                        if (dr["ApproverLevelNo"] != DBNull.Value)
                            objIAppLevelDefDetAppType_temp.ApproverLevelNo = Convert.ToInt32(dr["ApproverLevelNo"]);

                        if (dr["ApproverType"] != DBNull.Value)
                            objIAppLevelDefDetAppType_temp.ApproverType = Convert.ToInt32(dr["ApproverType"]);

                        objIAppLevelDefDetAppType_temp.IsNew = false;

                        if (objIAppLevelDefDetAppType_temp != null)
                        {
                            objIAppLevelDefDetAppType_temp.objAppLevelDetAppTypeApproverList_VW = GetAppLevelDetAppTypeApproverList(objIAppLevelDefDetAppType_temp.AppLvDefDetAppTypeCode_PK);
                        }

                        objIAppLevelDefDetAppType_List.Add(objIAppLevelDefDetAppType_temp);
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

            return objIAppLevelDefDetAppType_List;

        }

        private List<AppLevelDetAppTypeApprover> GetAppLevelDetAppTypeApproverList(string pAppLvDefDetAppTypeCode_PK)
        {

            SqlConnection connection = _supplierDbContext.GetConn();

            List<AppLevelDetAppTypeApprover> objIAppLevelDetAppTypeApprover_List = new List<AppLevelDetAppTypeApprover>();


            string vComTxt = @"SELECT  A.[AppLvDetAppTypeApprover]
                                      ,A.[AppLvDefDetAppTypeCode]
                                      ,[ApproverEmpCode]
                                      ,B.Name as 'EmployeeName'
                                      ,[DesignationType]
                                      ,A.[DesignationCode]
                                      ,C.FileName as 'DesignationName'
                                      ,[ApproverOfDept]
                                      ,[ApproverForDept]
                                      ,A.[CompanyCode]
                                      ,A.[UserCode]
                                      ,A.[ActionDate]
                                      ,A.[ActionType]
                                      ,D.FileName as 'ApproverForDeptName'
                                      ,E.FileName as 'ApproverOfDeptName'
									  ,A.[ActiveYN]
                                   FROM [LS_AppLevelDetAppTypeApprover] A 
                                   left join HR_Employee B on a.ApproverEmpCode=b.EmployeeCode 
                                   left join General_CodeFile C on C.FileCode=A.DesignationCode AND A.CompanyCode = C.CompanyCode
                                   left join General_CodeFile D on D.FileCode=A.ApproverForDept AND A.CompanyCode = D.CompanyCode
                                   left join General_CodeFile E on E.FileCode=A.ApproverOfDept AND A.CompanyCode = E.CompanyCode
                                   where  AppLvDefDetAppTypeCode='" + pAppLvDefDetAppTypeCode_PK + @"' 
                                   and A.[ActionType] <> 'DELETE'";



            try
            {
                connection.Open();
                SqlDataReader dr;
                SqlCommand command = new SqlCommand(vComTxt, connection);
                dr = command.ExecuteReader();

                while (dr.Read())
                    {
                        AppLevelDetAppTypeApprover objIAppLevelDetAppTypeApprover = new AppLevelDetAppTypeApprover();

                        if (dr["AppLvDetAppTypeApprover"] != DBNull.Value)
                            objIAppLevelDetAppTypeApprover.AppLvDetAppTypeApprover_PK = dr["AppLvDetAppTypeApprover"].ToString();

                        if (dr["AppLvDefDetAppTypeCode"] != DBNull.Value)
                            objIAppLevelDetAppTypeApprover.AppLvDefDetAppTypeCode_FK = dr["AppLvDefDetAppTypeCode"].ToString();

                        if (dr["ApproverEmpCode"] != DBNull.Value)
                            objIAppLevelDetAppTypeApprover.ApproverEmpCode = dr["ApproverEmpCode"].ToString();

                        if (dr["EmployeeName"] != DBNull.Value)
                            objIAppLevelDetAppTypeApprover.ApproverEmpName_VW = dr["EmployeeName"].ToString();

                        if (dr["DesignationType"] != DBNull.Value)
                            objIAppLevelDetAppTypeApprover.DesignationType = Convert.ToInt32(dr["DesignationType"]);

                        if (dr["DesignationCode"] != DBNull.Value)
                            objIAppLevelDetAppTypeApprover.DesignationCode = Convert.ToInt32(dr["DesignationCode"]);

                        if (dr["DesignationName"] != DBNull.Value)
                            objIAppLevelDetAppTypeApprover.Designation_VW = dr["DesignationName"].ToString();

                        if (dr["ApproverOfDept"] != DBNull.Value)
                            objIAppLevelDetAppTypeApprover.ApproverOfDept = Convert.ToInt32(dr["ApproverOfDept"]);

                        if (dr["ApproverOfDeptName"] != DBNull.Value)
                            objIAppLevelDetAppTypeApprover.ApproverOfDept_Name_VW = dr["ApproverOfDeptName"].ToString();

                        if (dr["ApproverForDept"] != DBNull.Value)
                            objIAppLevelDetAppTypeApprover.ApproverForDept = Convert.ToInt32(dr["ApproverForDept"]);

                        if (dr["ApproverForDeptName"] != DBNull.Value)
                            objIAppLevelDetAppTypeApprover.ApproverForDept_Name_VW = dr["ApproverForDeptName"].ToString();

                        if (dr["ActiveYN"] != DBNull.Value)
                            objIAppLevelDetAppTypeApprover.ActiveYN = Convert.ToInt32(dr["ActiveYN"]);

                        objIAppLevelDetAppTypeApprover.IsNew = false;

                        objIAppLevelDetAppTypeApprover_List.Add(objIAppLevelDetAppTypeApprover);
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

            return objIAppLevelDetAppTypeApprover_List;

        }

        public string SaveApproverSelection(List<AppLevelDefDetAppType> objIAppLevelDefDetAppType_List)
        {
            int vResult = 0;
            string vOut = "Exception Occured !";
            SqlConnection connection = _supplierDbContext.GetConn();
            

            List<string> vQueryList = new List<string>();

            foreach (AppLevelDefDetAppType objIAppLevelDefDetAppType in objIAppLevelDefDetAppType_List)
            {
                objIAppLevelDefDetAppType.TableName_TBL = ObjAppLevelDefDetAppTypeTable;

                if (objIAppLevelDefDetAppType.IsNew)
                {
                    objIAppLevelDefDetAppType.ActionType = "CREATE";
                    vQueryList.Add(_CRUDBuilder.CREATEQuery(objIAppLevelDefDetAppType));
                }
                else
                {
                    if (objIAppLevelDefDetAppType.IsDeleted == false)
                        objIAppLevelDefDetAppType.ActionType = "UPDATE";
                    else
                        objIAppLevelDefDetAppType.ActionType = "DELETE";
                    vQueryList.Add(_CRUDBuilder.UPDATEQuery(objIAppLevelDefDetAppType));
                }

                foreach (AppLevelDetAppTypeApprover objIAppLevelDetAppTypeApprover in objIAppLevelDefDetAppType.objAppLevelDetAppTypeApproverList_VW)
                {
                    //details

                    objIAppLevelDetAppTypeApprover.TableName_TBL = ObjAppLevelDetAppTypeApproverTable;

                    if (objIAppLevelDetAppTypeApprover.IsNew)
                    {
                        objIAppLevelDetAppTypeApprover.ActionType = "CREATE";
                        vQueryList.Add(_CRUDBuilder.CREATEQuery(objIAppLevelDetAppTypeApprover));
                    }
                    else if (objIAppLevelDetAppTypeApprover.IsDeleted == false)
                    {
                        objIAppLevelDetAppTypeApprover.ActionType = "UPDATE";
                        vQueryList.Add(_CRUDBuilder.UPDATEQuery(objIAppLevelDetAppTypeApprover));
                    }
                    else
                    {
                        Hashtable pMarkDELColNameValues = new Hashtable();
                        pMarkDELColNameValues.Clear();
                        objIAppLevelDetAppTypeApprover.ActionType = "DELETE";
                        pMarkDELColNameValues.Add("ActionType", objIAppLevelDetAppTypeApprover.ActionType);
                        pMarkDELColNameValues.Add("ActionDate", objIAppLevelDetAppTypeApprover.ActionDate);
                        pMarkDELColNameValues.Add("UserCode", objIAppLevelDetAppTypeApprover.UserCode);
                        pMarkDELColNameValues.Add("CompanyCode", objIAppLevelDetAppTypeApprover.CompanyCode_FK);

                        vQueryList.Add(_CRUDBuilder.DELETEQuery(objIAppLevelDetAppTypeApprover, true, pMarkDELColNameValues));
                    }
                }
            }
                

            

          
                connection.Open();
                using (SqlTransaction trans = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand command = _supplierDbContext.GetCommand())
                        {
                            if (vQueryList.Count > 0)
                            {
                                foreach (string obj_temp in vQueryList)
                                {
                                    command.CommandText = obj_temp;
                                    command.Transaction = trans;
                                    vResult = command.ExecuteNonQuery();
                                    if (vResult < 1)
                                    {
                                        trans.Rollback();
                                        vOut = "0 Records saved!";
                                        return vOut;
                                    }
                                }

                                trans.Commit();
                                vOut = "Information Saved Successfully";

                            }
                            else
                            {
                                trans.Rollback();
                                vOut = "0 Records saved!";
                            }

                        }
                    }
                    catch (DbException ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }

                }


            

            return vOut;
        }

        public int IsTransactionFound(string pAppLvDefinitionDetCode)
        {
          
            int vIsExist = 0;
            SqlConnection connection = _supplierDbContext.GetConn();
            string vComTxt = @" IF EXISTS (select  *  from  General_WaitingForApproval where ResponseStatus=0  
                                and  AppLvDefinitionDetCode='" + pAppLvDefinitionDetCode + @"' and [ActionType] <> 'DELETE')

                                select 1 as isExist
                                else select 0 as isExist

                              ";

            try
            {
                connection.Open();
                SqlDataReader dr;
                SqlCommand command = new SqlCommand(vComTxt, connection);
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    
                        if (dr["isExist"] != DBNull.Value)
                            vIsExist = Convert.ToInt32(dr["isExist"]);

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
            return vIsExist;
        }
    }
}
