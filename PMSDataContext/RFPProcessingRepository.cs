using ApprovalDataContext;
using CommonInterface;
using CommonModel;
using ConnectionGateway;
using LSCrud;
using PMSInterface;
using PMSModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace PMSDataContext
{
    public class RFPProcessingRepository: IRFPProcessingRepository
    {
        ISupplierDbContext _supplierDbContext;
        ICRUD _cRUD;
        IIDGenCriteriaInfo _iIDGenCriteriaInfo;
        public RFPProcessingRepository(ISupplierDbContext supplierDbContext, ICRUD cRUD, IIDGenCriteriaInfo iDGenCriteriaInfo)
        {
            _supplierDbContext = supplierDbContext;
            _cRUD = cRUD;
            _iIDGenCriteriaInfo = iDGenCriteriaInfo;
        }
        static readonly string PMS_RFProcessing_TBL = "PMS_RFProcessing";
        public string SaveRFPProcessing(RFProcessing objRFProcessing)
        {
            int vResult = 0;
            int vApprovalResult = 0;
            string vOut = "Exception Occured !";
            ArrayList vQueryList = new ArrayList();
            objRFProcessing.TableName_TBL = PMS_RFProcessing_TBL;
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            using (SqlTransaction trans = connection.BeginTransaction())
            {
                if (string.IsNullOrEmpty(objRFProcessing.RFProcessCode_PK))
                {
                    objRFProcessing.RFProcessCode_PK = Guid.NewGuid().ToString();
                    objRFProcessing.RFProcessId = _iIDGenCriteriaInfo.GenerateID(trans, objRFProcessing, EnumIdCategory.RFPProcessing);
                }
                vQueryList.Add(GetQuery(objRFProcessing));
                
                try
                {
                    using (SqlCommand command = _supplierDbContext.GetCommand())
                    {
                        foreach (string obj_temp in vQueryList)
                        {
                            command.CommandText = obj_temp;
                            vResult = command.ExecuteNonQuery();
                        }

                    }
                    if (vResult > 0)
                    {
                        vApprovalResult = AppObjectInfoMapDC.GenerateApprovalFromOtherObject(30, objRFProcessing.RFProcessCode_PK, objRFProcessing.UserCode, objRFProcessing.CompanyCode_FK, connection, trans);
                        if (vApprovalResult > 0)
                        {
                            trans.Commit();
                            vOut = "RFP Saved Successfully";
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
        public string GetQuery(object objObject)
        {
            BaseModel obj_Temp = (BaseModel)objObject;
            string vQuery = "";
            if (obj_Temp.IsNew)
            {

                vQuery = _cRUD.CREATEQuery(objObject);
            }
            else if (obj_Temp.IsDeleted == true)
            {
                Hashtable pMarkDELColNameValues = new Hashtable();
                pMarkDELColNameValues.Clear();
                obj_Temp.ActionType = "DELETE";
                pMarkDELColNameValues.Add("ActionType", obj_Temp.ActionType);
                pMarkDELColNameValues.Add("ActionDate", obj_Temp.ActionDate);
                pMarkDELColNameValues.Add("UserCode", obj_Temp.UserCode);
                pMarkDELColNameValues.Add("CompanyCode", obj_Temp.CompanyCode_FK);
                vQuery = _cRUD.DELETEQuery(objObject, true, pMarkDELColNameValues);
            }
            else
            {
                vQuery = _cRUD.UPDATEQuery(objObject);
            }
            return vQuery;
        }
        public List<RFProcessing> GetRFProcessing()
        {
            List<RFProcessing> rFProcessings = new List<RFProcessing>();
            RFProcessing objRFPProcessing;
            string vComTxt = @"SELECT row_number()OVER(ORDER BY RFProcessId) sl_no
                                      ,[RFProcessCode]
                                      ,r.[RequisitionCode]
                                      ,[PublishDate]
                                      ,[OpeningDate]
                                      ,[ClosingDate]
                                      ,[Status]
                                      ,(case when [Status]=1 then 'Active' else 'Inactive' end) status_nm
                                      ,[InvitingAuthority]
                                      ,r.[Remarks]
                                      ,r.[IsApproved]
                                      ,r.[ApprovalAction]
                                      ,[RFProcessId]
                                      ,[RequisitionType]
                                      ,(select [ItemText] from [LS_StaticDropDownListItem]
                                        where [ItemCode]=[RequisitionType] and [DropDownCode]=94) RequisitionTypeName
                                      ,e.name
                                      ,e.employeeid
                                      ,DesignationCode
                                      ,dbo.fxn_FileName(DesignationCode) AS Designation
                                      ,PermanentAddress
                                      ,Email
                                      ,p.RequisitionTitle
                                  FROM [PMS_RFProcessing] r
                                  join HR_Employee e on r.[InvitingAuthority]=e.EmployeeCode
                                  join PMS_PurchaseRequisition p on p.RequisitionCode=r.RequisitionCode";
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            dr = objDbCommand.ExecuteReader();
            while (dr.Read())
            {
                objRFPProcessing = new RFProcessing();
                objRFPProcessing.RFProcessCode_PK = dr["RFProcessCode"].ToString();
                objRFPProcessing.RFProcessId = dr["RFProcessId"].ToString();
                objRFPProcessing.RequisitionCode_FK = dr["RequisitionCode"].ToString();
                objRFPProcessing.OpeningDate = dr.GetDateTime(dr.GetOrdinal("OpeningDate")).ToString("dd-MM-yyyy");
                objRFPProcessing.PublishDate = dr.GetDateTime(dr.GetOrdinal("PublishDate")).ToString("dd-MM-yyyy");
                objRFPProcessing.ClosingDate = dr.GetDateTime(dr.GetOrdinal("ClosingDate")).ToString("dd-MM-yyyy");
                objRFPProcessing.Status = Convert.ToInt16(dr["Status"].ToString());
                objRFPProcessing.Status_VW = dr["status_nm"].ToString();
                objRFPProcessing.Remarks = dr["Remarks"].ToString();
                objRFPProcessing.InvitingAuthority = dr["InvitingAuthority"].ToString();
                objRFPProcessing.EmployeeName_VW= dr["name"].ToString();
                objRFPProcessing.EmployeeID_VW = dr["employeeid"].ToString();
                objRFPProcessing.Designation_VW = dr["Designation"].ToString();
                objRFPProcessing.IsApproved = Convert.ToInt32(dr["IsApproved"].ToString());
                objRFPProcessing.ApprovalAction = Convert.ToInt32(dr["ApprovalAction"].ToString());
                objRFPProcessing.Contact_VW= dr["Email"].ToString();
                objRFPProcessing.Address_VW= dr["PermanentAddress"].ToString();
                objRFPProcessing.RequisitionType_VW = Convert.ToInt32(dr["RequisitionType"].ToString());
                objRFPProcessing.RequisitionTypeName_VW = dr["RequisitionTypeName"].ToString();
                objRFPProcessing.RequisitionTitle_VW = dr["RequisitionTitle"].ToString();
                objRFPProcessing.SLNO_VW = Convert.ToInt32(dr["sl_no"].ToString());

                rFProcessings.Add(objRFPProcessing);
            }
            dr.Close();
            return rFProcessings;
        }
        public RFProcessing GetRFProcessingByCode(string rFProcessCode)
        {
            RFProcessing objRFPProcessing = new RFProcessing();

            string vComTxt = @"SELECT [RFProcessCode]
                                      ,r.[RequisitionCode]
                                      ,[PublishDate]
                                      ,[OpeningDate]
                                      ,[ClosingDate]
                                      ,[Status]
                                      ,(case when [Status]=1 then 'Active' else 'Inactive' end) status_nm
                                      ,[InvitingAuthority]
                                      ,r.[Remarks]
                                      ,r.[IsApproved]
                                      ,r.[ApprovalAction]
                                      ,[RFProcessId]
                                      ,[RequisitionType]
                                      ,(select [ItemText] from [LS_StaticDropDownListItem]
                                        where [ItemCode]=[RequisitionType] and [DropDownCode]=94) RequisitionTypeName
                                      ,e.name
                                      ,e.employeeid
                                      ,DesignationCode
                                      ,dbo.fxn_FileName(DesignationCode) AS Designation
                                      ,PermanentAddress
                                      ,Email
                                  FROM [PMS_RFProcessing] r
                                  join HR_Employee e on r.[InvitingAuthority]=e.EmployeeCode
                                  join PMS_PurchaseRequisition p on p.RequisitionCode=r.RequisitionCode where [RFProcessCode]= '" + rFProcessCode + "'";
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            dr = objDbCommand.ExecuteReader();
            if (dr.Read())
            {
                objRFPProcessing = new RFProcessing();
                objRFPProcessing.RFProcessCode_PK = dr["RFProcessCode"].ToString();
                objRFPProcessing.RFProcessId = dr["RFProcessId"].ToString();
                objRFPProcessing.RequisitionCode_FK = dr["RequisitionCode"].ToString();
                objRFPProcessing.OpeningDate = dr.GetDateTime(dr.GetOrdinal("OpeningDate")).ToString("dd-MM-yyyy");
                objRFPProcessing.PublishDate = dr.GetDateTime(dr.GetOrdinal("PublishDate")).ToString("dd-MM-yyyy");
                objRFPProcessing.ClosingDate = dr.GetDateTime(dr.GetOrdinal("ClosingDate")).ToString("dd-MM-yyyy");
                objRFPProcessing.Status = Convert.ToInt16(dr["Status"].ToString());
                objRFPProcessing.Status_VW = dr["status_nm"].ToString();
                objRFPProcessing.Remarks = dr["Remarks"].ToString();
                objRFPProcessing.InvitingAuthority = dr["InvitingAuthority"].ToString();
                objRFPProcessing.EmployeeName_VW = dr["name"].ToString();
                objRFPProcessing.EmployeeID_VW = dr["employeeid"].ToString();
                objRFPProcessing.Designation_VW = dr["Designation"].ToString();
                objRFPProcessing.IsApproved = Convert.ToInt32(dr["IsApproved"].ToString());
                objRFPProcessing.ApprovalAction = Convert.ToInt32(dr["ApprovalAction"].ToString());
                objRFPProcessing.Contact_VW = dr["Email"].ToString();
                objRFPProcessing.Address_VW = dr["PermanentAddress"].ToString();
                objRFPProcessing.RequisitionType_VW = Convert.ToInt32(dr["RequisitionType"].ToString());
                objRFPProcessing.RequisitionTypeName_VW = dr["RequisitionTypeName"].ToString();
            }
            dr.Close();
            return objRFPProcessing;
        }
        public string ApproveActionUpdate(string pRFProcessCode, int pApprovalAction, bool pIsApproved, SqlConnection objDbConnection, SqlTransaction objDbTransaction)
        {
            string vOut = string.Empty;
            int vResult = 0;
            string vUpdateQuery = string.Empty;
            string vComTxt = string.Empty;
            string vUpdateSOStatus = string.Empty;



            try
            {
                RFProcessing objRFProcessing = new RFProcessing();
                objRFProcessing = GetRFProcessingByCode(pRFProcessCode);

                if (pIsApproved == true)
                {
                    vUpdateQuery = "UPDATE " + PMS_RFProcessing_TBL +
                                      " SET ApprovalAction = " + pApprovalAction +
                                      ",  IsApproved = 1 " +
                                     " WHERE RFProcessCode = '" + pRFProcessCode + "' and [ActionType] <> 'Delete'";

                }

                else
                {

                    vUpdateQuery = "UPDATE " + PMS_RFProcessing_TBL + " SET ApprovalAction = " + pApprovalAction + " "
                                          + " WHERE RFProcessCode = '" + pRFProcessCode + "' and [ActionType] <> 'Delete' ";

                }
                using (SqlCommand command = new SqlCommand("", objDbConnection, objDbTransaction))
                {
                    command.CommandText = vUpdateQuery;
                    vResult = command.ExecuteNonQuery();
                }

                if (vResult > 0)
                {
                    //objDbTransaction.Commit();

                    vOut = EnumMessageId.LS101.ToString();
                }
                else
                {
                    //objDbTransaction.Rollback();
                    vOut = EnumMessageId.LS200.ToString();

                }
            }
            catch (DbException ex)
            {
                //objDbTransaction.Rollback();
                vOut = EnumMessageId.LS251.ToString() + ": \n " + ex.Message;
            }
            finally
            {
                //objDbConnection.Close();
            }


            return vOut;

        }
    }
}
