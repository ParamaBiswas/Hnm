using ApprovalDataContext;
using CommonInterface;
using CommonModel;
using ConnectionGateway;
using LS.LS.ModelBiz;
using LSCrud;
using SupplierInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace SupplierDataContext
{
    public class SupplierRepository : ISupplierRepository
    {
        ISupplierDbContext _supplierDbContext;
        ICRUD _cRUD;
        IIDGenCriteriaInfo _iIDGenCriteriaInfo; 
        public SupplierRepository(ISupplierDbContext supplierDbContext, ICRUD cRUD, IIDGenCriteriaInfo iDGenCriteriaInfo)
        {
            _supplierDbContext = supplierDbContext;
            _cRUD = cRUD;
            _iIDGenCriteriaInfo = iDGenCriteriaInfo;
        }
        static readonly string SupplierInfo_TBL = "LSP_PMS_SupplierInfo";
        static readonly string SupplierContact_TBL = "LSP_PMS_SupplierContact";
        static readonly string SupplierBusiness_TBL = "LSP_PMS_SupplierBusiness";
        static readonly string SupplierAttachment_TBL = "LSP_PMS_SupplierDocuments";

        public string SaveSupplier(SupplierInfo objsupplierInfo)
        {
            int vResult = 0;
            int vApprovalResult = 0;
            string vOut = "Exception Occured !";
            ArrayList vQueryList = new ArrayList();
            objsupplierInfo.TableName_TBL = SupplierInfo_TBL;
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            using (SqlTransaction trans = connection.BeginTransaction())
            {
                if (string.IsNullOrEmpty(objsupplierInfo.SupplierCode_PK))
                {
                    objsupplierInfo.SupplierCode_PK = Guid.NewGuid().ToString();
                    objsupplierInfo.SupplierID = _iIDGenCriteriaInfo.GenerateID(trans, objsupplierInfo, EnumIdCategory.SupplierID);
                }
                vQueryList.Add(GetQuery(objsupplierInfo));
                foreach (SupplierContact supplierContact in objsupplierInfo.SupplierContactList_VW)
                {
                    if (string.IsNullOrEmpty(supplierContact.ContactPersonCode_PK))
                    {
                        supplierContact.ContactPersonCode_PK = Guid.NewGuid().ToString();
                        supplierContact.TableName_TBL = SupplierContact_TBL;
                        supplierContact.SupplierCode_FK = objsupplierInfo.SupplierCode_PK;

                    }
                    vQueryList.Add(GetQuery(supplierContact));
                }
                foreach (SupplierAttachment supplierAttachment in objsupplierInfo.SupplierAttachmentList_VW)
                {
                    if (string.IsNullOrEmpty(supplierAttachment.AttachmentCode_PK))
                    {
                        supplierAttachment.AttachmentCode_PK = Guid.NewGuid().ToString();
                        supplierAttachment.TableName_TBL = SupplierAttachment_TBL;
                        supplierAttachment.SupplierCode_FK = objsupplierInfo.SupplierCode_PK;
                    }
                    vQueryList.Add(GetQuery(supplierAttachment));
                }
                foreach (SupplierBusiness supplierBusiness in objsupplierInfo.SupplierBusinessList_VW)
                {
                    if (supplierBusiness.BusinessTypeCode_PK != 0)
                    {
                        supplierBusiness.TableName_TBL = SupplierBusiness_TBL;
                        supplierBusiness.SupplierCode_FK = objsupplierInfo.SupplierCode_PK;
                    }
                    vQueryList.Add(GetQuery(supplierBusiness));
                }

                try
                {
                    using (SqlCommand command = new SqlCommand("", connection, trans))
                    {
                        foreach (string obj_temp in vQueryList)
                        {
                            command.CommandText = obj_temp;
                            vResult = command.ExecuteNonQuery();
                        }

                    }
                    if (vResult > 0)
                    {
                        vApprovalResult = AppObjectInfoMapDC.GenerateApprovalFromOtherObject(27, objsupplierInfo.SupplierCode_PK, objsupplierInfo.UserCode, objsupplierInfo.CompanyCode_FK, connection, trans);
                        if (vApprovalResult > 0)
                        {
                            trans.Commit();
                            vOut = "Supplier Information Saved Successfully";
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
        //public string SaveSupplier(SupplierInfo objsupplierInfo)
        //{
        //    int vResult = 0;
        //    string vOut ="Exception Occured !";
        //    string vSupplierCode = Guid.NewGuid().ToString();
        //    string vSupplierID = "001";
        //    string vContactPersonID = "001";
        //    SqlConnection connection = _supplierDbContext.GetConn();
        //    connection.Open();
        //    SqlDataReader dr;
        //    using (SqlTransaction trans = connection.BeginTransaction())
        //    {
        //        try
        //    {

        //            string sSqlSupplier = @"INSERT INTO SupplierInfo 
        //                            (SupplierCode,SupplierID,SupplierName,
        //                            Email,MobileNumber,EnlistmentDate,SupplierAddress
        //                            ,AlternateEmail,ZipCode,Fax,CompanyCode
        //                            UserCode,ActionDate,ActionType)
        //                            VALUES
        //                            (@SupplierCode,@SupplierID,@SupplierName,
        //                            @Email,@MobileNumber,@EnlistmentDate,@SupplierAddress
        //                            ,@AlternateEmail,@ZipCode,@Fax,@CompanyCode
        //                            @UserCode,@ActionDate,@ActionType)";
        //            SqlCommand command = new SqlCommand(sSqlSupplier, connection);
        //            command.Parameters.AddWithValue("SupplierCode", vSupplierCode);
        //            command.Parameters.AddWithValue("SupplierID", vSupplierID);
        //            command.Parameters.AddWithValue("SupplierName", objsupplierInfo.SupplierName);
        //            command.Parameters.AddWithValue("Email", objsupplierInfo.Email);
        //            command.Parameters.AddWithValue("MobileNumber", objsupplierInfo.MobileNumber);
        //            command.Parameters.AddWithValue("EnlistmentDate", objsupplierInfo.EnlistmentDate);
        //            command.Parameters.AddWithValue("SupplierAddress", objsupplierInfo.SupplierAddress);
        //            command.Parameters.AddWithValue("AlternateEmail", objsupplierInfo.AlternateEmail);
        //            command.Parameters.AddWithValue("ZipCode", objsupplierInfo.ZipCode);
        //            command.Parameters.AddWithValue("Fax", objsupplierInfo.Fax);
        //            command.Parameters.AddWithValue("CompanyCode", objsupplierInfo.CompanyCode_FK);
        //            command.Parameters.AddWithValue("UserCode", objsupplierInfo.UserCode);
        //            command.Parameters.AddWithValue("ActionDate", objsupplierInfo.ActionDate);
        //            command.Parameters.AddWithValue("ActionType", "Insert");
        //            vResult = command.ExecuteNonQuery();

        //            if (vResult > 0)
        //            {
        //                foreach (SupplierContact supplierContact in objsupplierInfo.SupplierContactList)
        //                {
        //                    string sSqlContactPerson = @"INSERT INTO SupplierContact 
        //                            (ContactPersonCode,ContactPersonID,SupplierCode,
        //                            ContactName,JobRole,Email,MobileNumber
        //                            ,Designation,TelephoneNumber,CompanyCode
        //                            UserCode,ActionDate,ActionType)
        //                            VALUES
        //                            (@ContactPersonCode,@ContactPersonID,@SupplierCode,
        //                            @ContactName,@JobRole,@Email,@MobileNumber
        //                            ,@Designation,@TelephoneNumber,@CompanyCode
        //                            @UserCode,@ActionDate,@ActionType)";
        //                    SqlCommand sqlContact = new SqlCommand(sSqlContactPerson, connection);
        //                    sqlContact.Parameters.AddWithValue("ContactPersonCode", Guid.NewGuid().ToString());
        //                    sqlContact.Parameters.AddWithValue("ContactPersonID", vContactPersonID);
        //                    sqlContact.Parameters.AddWithValue("SupplierCode", vSupplierCode);
        //                    sqlContact.Parameters.AddWithValue("ContactName", supplierContact.ContactName);
        //                    sqlContact.Parameters.AddWithValue("JobRole", supplierContact.JobRole);
        //                    sqlContact.Parameters.AddWithValue("Email", supplierContact.Email);
        //                    sqlContact.Parameters.AddWithValue("MobileNumber", supplierContact.MobileNumber);
        //                    sqlContact.Parameters.AddWithValue("Designation", supplierContact.Designation);
        //                    sqlContact.Parameters.AddWithValue("TelephoneNumber", supplierContact.TelephoneNumber);
        //                    sqlContact.Parameters.AddWithValue("CompanyCode", objsupplierInfo.CompanyCode_FK);
        //                    sqlContact.Parameters.AddWithValue("UserCode", objsupplierInfo.UserCode);
        //                    sqlContact.Parameters.AddWithValue("ActionDate", objsupplierInfo.ActionDate);
        //                    sqlContact.Parameters.AddWithValue("ActionType", "Insert");
        //                    vResult = sqlContact.ExecuteNonQuery();

        //                }
        //                if (vResult > 0)
        //                {
        //                    foreach (SupplierAttachment supplierAttachment in objsupplierInfo.SupplierAttachmentList)
        //                    {
        //                        string sSqlSupplierAttachment = @"INSERT INTO SupplierAttachment 
        //                            (AttachmentCode,AttachmentID,SupplierCode,
        //                             AttachmentName,CertificateNumber,IssueDate
        //                             ,ExpiryDate,Remarks,CompanyCode
        //                            UserCode,ActionDate,ActionType)
        //                            VALUES
        //                            (@AttachmentCode,@AttachmentID,@SupplierCode,
        //                             @AttachmentName,@CertificateNumber,@IssueDate
        //                             ,@ExpiryDate,@Remarks,@CompanyCode
        //                            @UserCode,@ActionDate,@ActionType)";

        //                        SqlCommand sqlAttachment = new SqlCommand(sSqlSupplierAttachment, connection);
        //                        sqlAttachment.Parameters.AddWithValue("AttachmentCode", Guid.NewGuid().ToString());
        //                        sqlAttachment.Parameters.AddWithValue("AttachmentID", vContactPersonID);
        //                        sqlAttachment.Parameters.AddWithValue("SupplierCode", vSupplierCode);
        //                        sqlAttachment.Parameters.AddWithValue("AttachmentName", supplierAttachment.AttachmentName);
        //                        sqlAttachment.Parameters.AddWithValue("CertificateNumber", supplierAttachment.CertificateNumber);
        //                        sqlAttachment.Parameters.AddWithValue("IssueDate", supplierAttachment.IssueDate);
        //                        sqlAttachment.Parameters.AddWithValue("ExpiryDate", supplierAttachment.ExpiryDate);
        //                        sqlAttachment.Parameters.AddWithValue("Remarks", supplierAttachment.Remarks);
        //                        sqlAttachment.Parameters.AddWithValue("CompanyCode", objsupplierInfo.CompanyCode_FK);
        //                        sqlAttachment.Parameters.AddWithValue("UserCode", objsupplierInfo.UserCode);
        //                        sqlAttachment.Parameters.AddWithValue("ActionDate", objsupplierInfo.ActionDate);
        //                        sqlAttachment.Parameters.AddWithValue("ActionType", "Insert");
        //                        vResult = sqlAttachment.ExecuteNonQuery();
        //                    }
        //                    if (vResult > 0)
        //                    {
        //                        foreach (SupplierBusiness supplierBusiness in objsupplierInfo.SupplierBusinessList)
        //                        {
        //                            string sSqlSupplierBusiness = @"INSERT INTO SupplierBusiness 
        //                            (SupplierCode,ItemCode,CompanyCode
        //                            UserCode,ActionDate,ActionType)
        //                            VALUES
        //                            (@SupplierCode,@ItemCode,@CompanyCode
        //                            @UserCode,@ActionDate,@ActionType)";
        //                            SqlCommand sqlBussiness = new SqlCommand(sSqlSupplierBusiness, connection);
        //                            sqlBussiness.Parameters.AddWithValue("SupplierCode", vSupplierCode);
        //                            sqlBussiness.Parameters.AddWithValue("ItemCode", supplierBusiness.ItemCode_PK);
        //                            sqlBussiness.Parameters.AddWithValue("CompanyCode", objsupplierInfo.CompanyCode_FK);
        //                            sqlBussiness.Parameters.AddWithValue("UserCode", objsupplierInfo.UserCode);
        //                            sqlBussiness.Parameters.AddWithValue("ActionDate", objsupplierInfo.ActionDate);
        //                            sqlBussiness.Parameters.AddWithValue("ActionType", "Insert");
        //                            vResult = sqlBussiness.ExecuteNonQuery();
        //                            //trans.Commit();
        //                            vOut = "Supplier saved Successfully";

        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            trans.Rollback();
        //        }
        //    }



        //    return vOut;

        //}
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
        public SupplierInfo GetSupplierInfo(string suppliercode)
        {
            SupplierInfo objSupplierInfo=new SupplierInfo(); 
            List<SupplierAttachment> objSupplierAttachment = new List<SupplierAttachment>();
            List<SupplierBusiness> objSupplierBusiness = new List<SupplierBusiness>();
            List<SupplierContact> objSupplierContact = new List<SupplierContact>();
            string vSupplierText = @"SELECT [SupplierCode]
                                      ,[SupplierID]
                                      ,[SupplierName]
                                      ,[Email]
                                      ,[MobileNumber]
                                      ,[EnlistmentDate]
                                      ,[SupplierAddress]
                                      ,[AlternateEmail]
                                      ,[ZipCode]
                                      ,[Fax]
                                      ,[IsApproved]
                                      ,[ApprovalAction]
                                  FROM [LSP_PMS_SupplierInfo] where  SupplierCode = '" + suppliercode + "'";
            string vAttachmentText = @"SELECT [AttachmentCode]
                                              ,[SupplierCode]
                                              ,[AttachmentName]
                                              ,[CertificateNumber]
                                              ,[IssueDate]
                                              ,[ExpiryDate]
                                              ,[FileLocationPath]
                                              ,[Remarks]
                                          FROM [LSP_PMS_SupplierDocuments] where  SupplierCode = '" + suppliercode + "'";

            string vContactText = @"SELECT [ContactPersonCode]
                                          ,[SupplierCode]
                                          ,[ContactName]
                                          ,[JobRole]
                                          ,[Email]
                                          ,[MobileNumber]
                                          ,[Designation]
                                          ,[TelephoneNumber]
                                      FROM [LSP_PMS_SupplierContact] where  SupplierCode = '" + suppliercode + "'";
            string vBusinessText = @"SELECT [SupplierCode]
                                              ,[BusinessTypeCode]
                                              ,filename
                                          FROM [LSP_PMS_SupplierBusiness]  b
                                           join general_codefile f on b.[BusinessTypeCode]=f.filecode
                                           where  SupplierCode = '" + suppliercode + "'";
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlCommand objDbCommand = new SqlCommand(vSupplierText, connection);
            using (SqlDataReader dr = objDbCommand.ExecuteReader())
            {
                if (dr.Read())
                {
                    objSupplierInfo = new SupplierInfo();
                    objSupplierInfo.SupplierCode_PK = dr["SupplierCode"].ToString();
                    objSupplierInfo.SupplierID = dr["SupplierID"].ToString();
                    objSupplierInfo.SupplierName = dr["SupplierName"].ToString();
                    objSupplierInfo.Email = dr["Email"].ToString();
                    objSupplierInfo.MobileNumber = dr["MobileNumber"].ToString();
                    objSupplierInfo.EnlistmentDate = dr.GetDateTime(dr.GetOrdinal("EnlistmentDate")).ToString("dd-MM-yyyy");
                    objSupplierInfo.SupplierAddress = dr["SupplierAddress"].ToString();
                    objSupplierInfo.AlternateEmail = dr["AlternateEmail"].ToString();
                    objSupplierInfo.ZipCode = dr["ZipCode"].ToString();
                    objSupplierInfo.Fax = dr["Fax"].ToString();
                    objSupplierInfo.IsApproved = Convert.ToInt16(dr["IsApproved"].ToString());
                    objSupplierInfo.ApprovalAction = Convert.ToInt16(dr["ApprovalAction"].ToString());

                }
            }
            SqlCommand objDbCommandAttachment = new SqlCommand(vAttachmentText, connection);
            using (SqlDataReader dr = objDbCommandAttachment.ExecuteReader())
            {
                while (dr.Read())
                {
                    SupplierAttachment supplierAttachment = new SupplierAttachment();
                    supplierAttachment.AttachmentCode_PK = dr["AttachmentCode"].ToString();
                    supplierAttachment.SupplierCode_FK = dr["SupplierCode"].ToString();
                    supplierAttachment.AttachmentName = dr["AttachmentName"].ToString();
                    supplierAttachment.CertificateNumber = dr["CertificateNumber"].ToString();
                    if (!string.IsNullOrEmpty(dr["IssueDate"].ToString()))
                        supplierAttachment.IssueDate = dr.GetDateTime(dr.GetOrdinal("IssueDate")).ToString("dd-MM-yyyy");
                    if(!string.IsNullOrEmpty(dr["ExpiryDate"].ToString()))
                    supplierAttachment.ExpiryDate = dr.GetDateTime(dr.GetOrdinal("ExpiryDate")).ToString("dd-MM-yyyy");
                    supplierAttachment.FileLocationPath = dr["FileLocationPath"].ToString();
                    supplierAttachment.Remarks = dr["Remarks"].ToString();

                    objSupplierAttachment.Add(supplierAttachment);
                }
            }
            SqlCommand objDbCommandBusiness = new SqlCommand(vBusinessText, connection);
            using (SqlDataReader dr = objDbCommandBusiness.ExecuteReader())
            {
                while (dr.Read())
                {
                    SupplierBusiness supplierBusiness = new SupplierBusiness();
                    supplierBusiness.SupplierCode_FK= dr["SupplierCode"].ToString();
                    supplierBusiness.BusinessTypeCode_PK = Convert.ToInt16(dr["BusinessTypeCode"].ToString());
                    supplierBusiness.filename_VW = dr["filename"].ToString();
                    objSupplierBusiness.Add(supplierBusiness);
                }

            }
            SqlCommand objDbCommandContact = new SqlCommand(vContactText, connection);
            using (SqlDataReader dr = objDbCommandContact.ExecuteReader())
            {
                while (dr.Read())
                {
                    SupplierContact supplierContact = new SupplierContact();
                    supplierContact.SupplierCode_FK= dr["SupplierCode"].ToString();
                    supplierContact.ContactPersonCode_PK= dr["ContactPersonCode"].ToString();
                    supplierContact.ContactName = dr["ContactName"].ToString();
                    supplierContact.JobRole = dr["JobRole"].ToString();
                    supplierContact.Email = dr["Email"].ToString();
                    supplierContact.MobileNumber = dr["MobileNumber"].ToString();
                    supplierContact.Designation = dr["Designation"].ToString();
                    supplierContact.TelephoneNumber = dr["TelephoneNumber"].ToString();
                    objSupplierContact.Add(supplierContact);
                }
            }

            objSupplierInfo.SupplierAttachmentList_VW = objSupplierAttachment;
            objSupplierInfo.SupplierBusinessList_VW = objSupplierBusiness;
            objSupplierInfo.SupplierContactList_VW = objSupplierContact;
            return objSupplierInfo;

        }
        public  string ApproveActionUpdate(string pSupplierCode, int pApprovalAction, bool pIsApproved, SqlConnection objDbConnection, SqlTransaction objDbTransaction)
        {
            string vOut = string.Empty;
            int vResult = 0;
            string vUpdateQuery = string.Empty;
            string vComTxt = string.Empty;
            string vUpdateSOStatus = string.Empty;
           


            try
            {
                SupplierInfo objSupplierInfo = new SupplierInfo();
                objSupplierInfo = GetSupplierInfo(pSupplierCode);
                
                if (pIsApproved == true)
                {
                    vUpdateQuery = "UPDATE " + SupplierInfo_TBL +
                                      " SET ApprovalAction = " + pApprovalAction +
                                      ",  IsApproved = 1 " +
                                     " WHERE SupplierCode = '" + pSupplierCode + "' and [ActionType] <> 'Delete'";

                }

                else
                {

                    vUpdateQuery = "UPDATE " + SupplierInfo_TBL + " SET ApprovalAction = " + pApprovalAction + " "
                                          + " WHERE SupplierCode = '" + pSupplierCode + "' and [ActionType] <> 'Delete' ";

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
