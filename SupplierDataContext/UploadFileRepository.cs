using ApprovalDataContext;
using CommonInterface;
using CommonModel;
using ConnectionGateway;
using LSCrud;
using SupplierInterface;
using SupplierModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace SupplierDataContext
{
    public class UploadFileRepository : IUploadedFileRepository
    {
        ISupplierDbContext _supplierDbContext;
        ICRUD _cRUD;
        IIDGenCriteriaInfo _iIDGenCriteriaInfo;
        public UploadFileRepository(ISupplierDbContext supplierDbContext, ICRUD cRUD, IIDGenCriteriaInfo iDGenCriteriaInfo)
        {
            _supplierDbContext = supplierDbContext;
            _cRUD = cRUD;
            _iIDGenCriteriaInfo = iDGenCriteriaInfo;
        }
        static readonly string UploadedFilesInfo_TBL = "LSP_PMS_UploadedFiles";

        public string SaveUploadedFiles(List<UploadedFile> objUploadedFileList)
        {
            int vResult = 0;
            int vApprovalResult = 0;
            string vOut = "Exception Occured !";
            ArrayList vQueryList = new ArrayList();
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            using (SqlTransaction trans = connection.BeginTransaction())
            {
                foreach (UploadedFile objUploadedFile in objUploadedFileList)
                {
                    if (string.IsNullOrEmpty(objUploadedFile.UploadedCode_PK))
                    {
                        objUploadedFile.TableName_TBL = UploadedFilesInfo_TBL;
                        objUploadedFile.UploadedCode_PK = Guid.NewGuid().ToString();

                    }
                    vQueryList.Add(GetQuery(objUploadedFile));

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
                        foreach (UploadedFile objUploadedFile in objUploadedFileList)
                        {

                            vApprovalResult = AppObjectInfoMapDC.GenerateApprovalFromOtherObject(28, objUploadedFile.UploadedCode_PK, objUploadedFile.UserCode, objUploadedFile.CompanyCode_FK, connection, trans);
                            if (vApprovalResult == 0)
                                break;
                        }
                        if (vApprovalResult > 0)
                        {
                            trans.Commit();
                            vOut = "Supplier Document Uploaded Successfully";
                        }
                        else
                            trans.Rollback();
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
        public UploadedFile GetUploadedFile(string UploadedFileCode)
        {
            UploadedFile objUploadedFile = new UploadedFile();
        
            string vQueryText = @"SELECT [UploadedCode]
                              ,[AttachmentFileCode]
                              ,[AttachmentName]
                              ,[UploadDate]
                              ,[FileLocationPath]
                              ,[SupplierCode]
                              ,[IsApproved]
                              ,[ApprovalAction]
      
                          FROM [LSP_PMS_UploadedFiles] where  UploadedCode = '" + UploadedFileCode + "'";
           
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlCommand objDbCommand = new SqlCommand(vQueryText, connection);
            using (SqlDataReader dr = objDbCommand.ExecuteReader())
            {
                if (dr.Read())
                {
                    objUploadedFile = new UploadedFile();
                    objUploadedFile.UploadedCode_PK = dr["UploadedCode"].ToString();
                    objUploadedFile.UploadDate = dr.GetDateTime(dr.GetOrdinal("UploadDate")).ToString("dd-MM-yyyy");
                    objUploadedFile.SupplierCode_FK = dr["SupplierCode"].ToString();
                    objUploadedFile.IsApproved = Convert.ToInt16(dr["IsApproved"].ToString());
                    objUploadedFile.ApprovalAction = Convert.ToInt16(dr["ApprovalAction"].ToString());
                    objUploadedFile.FileLocationPath = dr["FileLocationPath"].ToString();
                    objUploadedFile.AttachmentFileCode = dr["AttachmentFileCode"].ToString();
                    objUploadedFile.AttachmentName = dr["AttachmentName"].ToString();

                }
            }
                     
            return objUploadedFile;

        }
        public string ApproveActionUpdate(string pFileUploadedCode, int pApprovalAction, bool pIsApproved, SqlConnection objDbConnection, SqlTransaction objDbTransaction)
        {
            string vOut = string.Empty;
            int vResult = 0;
            string vUpdateQuery = string.Empty;
            string vComTxt = string.Empty;
            string vUpdateSOStatus = string.Empty;

            try
            {
                UploadedFile objUploadedFile = new UploadedFile();
                objUploadedFile = GetUploadedFile(pFileUploadedCode);
                if (pIsApproved == true)
                {
                    vUpdateQuery = "UPDATE " + UploadedFilesInfo_TBL +
                                      " SET ApprovalAction = " + pApprovalAction +
                                      ",  IsApproved = 1 " +
                                     " WHERE UploadedCode = '" + pFileUploadedCode + "' and [ActionType] <> 'Delete'";

                }

                else
                {

                    vUpdateQuery = "UPDATE " + UploadedFilesInfo_TBL + " SET ApprovalAction = " + pApprovalAction + " "
                                          + " WHERE UploadedCode = '" + pFileUploadedCode + "' and [ActionType] <> 'Delete' ";

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
        public List<UploadedFile> GetUploadedFileBySupplierCode(string SupplierCode)
        {
            UploadedFile objUploadedFile = new UploadedFile();
            List<UploadedFile> ObjList = new List<UploadedFile>();
            string vQueryText = @"SELECT [UploadedCode]
                              ,[AttachmentFileCode]
                              ,[AttachmentName]
                              ,[UploadDate]
                              ,[FileLocationPath]
                              ,[SupplierCode]
                              ,[IsApproved]
                              ,[ApprovalAction]
      
                          FROM [LSP_PMS_UploadedFiles] where  SupplierCode = '" + SupplierCode + "'";

            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlCommand objDbCommand = new SqlCommand(vQueryText, connection);
            using (SqlDataReader dr = objDbCommand.ExecuteReader())
            {
                while (dr.Read())
                {
                    objUploadedFile = new UploadedFile();
                    objUploadedFile.UploadedCode_PK = dr["UploadedCode"].ToString();
                    objUploadedFile.UploadDate = dr.GetDateTime(dr.GetOrdinal("UploadDate")).ToString("dd-MM-yyyy");
                    objUploadedFile.SupplierCode_FK = dr["SupplierCode"].ToString();
                    objUploadedFile.IsApproved = Convert.ToInt16(dr["IsApproved"].ToString());
                    objUploadedFile.ApprovalAction = Convert.ToInt16(dr["ApprovalAction"].ToString());
                    objUploadedFile.FileLocationPath = dr["FileLocationPath"].ToString();
                    objUploadedFile.AttachmentFileCode = dr["AttachmentFileCode"].ToString();
                    objUploadedFile.AttachmentName = dr["AttachmentName"].ToString();
                    ObjList.Add(objUploadedFile);
                }
            }

            return ObjList;

        }
    }
}
