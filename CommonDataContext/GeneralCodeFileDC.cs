using CommonInterface;
using CommonModel;
using ConnectionGateway;
using LS.General.ModelBiz;
using LSCrud;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace CommonDataContext
{
    public class GeneralCodeFileDC : IGeneralCodeFile
    {
        ISupplierDbContext _supplierDbContext;
        ICRUD _cRUD;
        public GeneralCodeFileDC(ISupplierDbContext supplierDbContext, ICRUD cRUD)
        {
            _supplierDbContext = supplierDbContext;
            _cRUD = cRUD;
        }
        public List<GeneralCodeFile> GetGeneralCodeFileByFileTypeNFileLevel(Int32 FileTypeCode, Int32 LevelCode, Int32 COMPANY_CODE)
        {
            List<GeneralCodeFile> listGeneralCodeFile = new List<GeneralCodeFile>();
            GeneralCodeFile objGeneralCodeFile;
            string vComTxt = @"SELECT [FileCode]
                            ,[UserCode]
                            ,[ActionType]
                            ,[ActionDate]
                            ,[FileId]
                           ,[FileName] 
                            ,[FileShortName]
                            ,[SortOrder]
                            ,[ParentFileCode]
                            ,[FileTypeCode]
                            ,[LevelCode]
                            ,[CompanyCode]
                            ,[ModuleCode]
                            ,[IsReadOnly]
                            FROM [dbo].[General_CodeFile]
                            where FileTypeCode=@FileTypeCode
                            and LevelCode=@LevelCode
                            AND CompanyCode = " + COMPANY_CODE + "" +
                            " order by [SortOrder]";
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            objDbCommand.Parameters.AddWithValue("FileTypeCode", FileTypeCode);
            objDbCommand.Parameters.AddWithValue("LevelCode", LevelCode);
            dr = objDbCommand.ExecuteReader();
            while (dr.Read())
            {
                objGeneralCodeFile = new GeneralCodeFile();
                objGeneralCodeFile.FileCode_PK = Convert.ToInt32(dr["FileCode"].ToString());
                objGeneralCodeFile.UserCode = dr["UserCode"].ToString();
                objGeneralCodeFile.ActionType = dr["ActionType"].ToString();
                objGeneralCodeFile.FileId = dr["FileId"].ToString();
                objGeneralCodeFile.FileName = dr["FileName"].ToString();
                objGeneralCodeFile.FileShortName = dr["FileShortName"].ToString();
                objGeneralCodeFile.SortOrder = dr["SortOrder"].ToString();
                objGeneralCodeFile.ParentFileCode = dr["ParentFileCode"].ToString();
                objGeneralCodeFile.FileTypeCode = Convert.ToInt32(dr["FileTypeCode"].ToString());
                objGeneralCodeFile.LevelCode = Convert.ToInt32(dr["LevelCode"].ToString());
                if (!string.IsNullOrEmpty(dr["CompanyCode"].ToString()))
                    objGeneralCodeFile.CompanyCode_FK = Convert.ToInt32(dr["CompanyCode"].ToString());
                objGeneralCodeFile.ModuleCode = dr["ModuleCode"].ToString();
                if (!string.IsNullOrEmpty(dr["IsReadOnly"].ToString()))
                    objGeneralCodeFile.IsReadOnly = Convert.ToBoolean(Convert.ToInt16(dr["IsReadOnly"].ToString()));
                listGeneralCodeFile.Add(objGeneralCodeFile);
            }
            objDbCommand.Dispose();
            return listGeneralCodeFile;

        }
        public  List<GeneralCodeFile> GetGeneralCodeFileByFileTypeNParentFile(Int32 pFileTypeCode, Int32 pParentFileCode, Int32 COMPANY_CODE)
        {
            List<GeneralCodeFile> listGeneralCodeFile = new List<GeneralCodeFile>();
            GeneralCodeFile objGeneralCodeFile;
            string vComTxt = @"SELECT gcf.[FileCode]
                            ,gcf.[UserCode]
                            ,gcf.[ActionType]
                            ,gcf.[ActionDate]
                            ,gcf.[FileId]
                            ,gcf.[FileName]
                            ,gcf.[FileShortName]
                            ,gcf.[SortOrder]
                            ,gcf.[ParentFileCode]
                            ,gcf.[FileTypeCode]
                            ,gcf.[LevelCode]
                            ,gcf.[CompanyCode]
                            ,gcf.[ModuleCode]
                            ,gcfl.[LevelName]
                            ,gcf.[IsReadOnly]
                            FROM [dbo].[General_CodeFile] gcf
                            Left outer join General_CodeFileLevel gcfl
                            ON gcf.LevelCode=gcfl.LevelCode  AND gcf.CompanyCode = gcfl.CompanyCode
                            WHERE gcf.FileTypeCode=@FileTypeCode
                            AND gcf.ParentFileCode=@ParentFileCode
                            AND gcf.CompanyCode = " + COMPANY_CODE + @"
                            order by gcf.[SortOrder]";
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            objDbCommand.Parameters.AddWithValue("FileTypeCode", pFileTypeCode);
            objDbCommand.Parameters.AddWithValue("ParentFileCode", pParentFileCode);
            dr = objDbCommand.ExecuteReader();
            while (dr.Read())
            {
                objGeneralCodeFile = new GeneralCodeFile();
                objGeneralCodeFile.FileCode_PK = Convert.ToInt32(dr["FileCode"].ToString());
                objGeneralCodeFile.UserCode = dr["UserCode"].ToString();
                objGeneralCodeFile.ActionType = dr["ActionType"].ToString();
                objGeneralCodeFile.FileId = dr["FileId"].ToString();
                objGeneralCodeFile.FileName = dr["FileName"].ToString();
                objGeneralCodeFile.FileShortName = dr["FileShortName"].ToString();
                if (!string.IsNullOrEmpty(dr["SortOrder"].ToString()))
                    objGeneralCodeFile.SortOrder = dr["SortOrder"].ToString();
                if (!string.IsNullOrEmpty(dr["ParentFileCode"].ToString()))
                    objGeneralCodeFile.ParentFileCode = dr["ParentFileCode"].ToString();
                objGeneralCodeFile.FileTypeCode = Convert.ToInt32(dr["FileTypeCode"].ToString());
                objGeneralCodeFile.LevelCode = Convert.ToInt32(dr["LevelCode"].ToString());
                if (!string.IsNullOrEmpty(dr["CompanyCode"].ToString()))
                    objGeneralCodeFile.CompanyCode_FK = Convert.ToInt32(dr["CompanyCode"].ToString());
                objGeneralCodeFile.ModuleCode = dr["ModuleCode"].ToString();
                objGeneralCodeFile.LevelName = dr["LevelName"].ToString();
                if (!string.IsNullOrEmpty(dr["IsReadOnly"].ToString()))
                    objGeneralCodeFile.IsReadOnly = Convert.ToBoolean(Convert.ToInt16(dr["IsReadOnly"].ToString()));
                listGeneralCodeFile.Add(objGeneralCodeFile);
            }
            objDbCommand.Dispose();
            return listGeneralCodeFile;
        }

        public List<GeneralCodeFile> GetGeneralCodeFileByFileType(Int32 pFileTypeCode, string pCompanyCode)
        {
            List<GeneralCodeFile> listGeneralCodeFile = new List<GeneralCodeFile>();
            GeneralCodeFile objGeneralCodeFile;
            string vComTxt = string.Empty;
            vComTxt = @"SELECT [FileCode]
                            ,[UserCode]
                            ,[ActionType]
                            ,[ActionDate]
                            ,[FileId]
                            ,[FileName]
                            ,[FileShortName]
                            ,[SortOrder]
                            ,[ParentFileCode]
                            ,[FileTypeCode]
                            ,[LevelCode]
                            ,[CompanyCode]
                            ,[ModuleCode]
                            ,[IsReadOnly]
                            FROM [dbo].[General_CodeFile] (nolock)
                            where FileTypeCode=@FileTypeCode
                            and (ParentFileCode is null or ParentFileCode=0)
                            AND ActionType<>'DELETE' AND CompanyCode = " + pCompanyCode + "" +
                            " order by [SortOrder]";


            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            objDbCommand.Parameters.AddWithValue("FileTypeCode", pFileTypeCode);
            objDbCommand.Parameters.AddWithValue("CompanyCode", pCompanyCode);
            dr = objDbCommand.ExecuteReader();

            while (dr.Read())
            {
                objGeneralCodeFile = new GeneralCodeFile();
                objGeneralCodeFile.FileCode_PK = Convert.ToInt32(dr["FileCode"].ToString());
                objGeneralCodeFile.UserCode = dr["UserCode"].ToString();
                objGeneralCodeFile.ActionType = dr["ActionType"].ToString();
                objGeneralCodeFile.FileId = dr["FileId"].ToString();
                objGeneralCodeFile.FileName = dr["FileName"].ToString();
                objGeneralCodeFile.FileShortName = dr["FileShortName"].ToString();
                if (!string.IsNullOrEmpty(dr["SortOrder"].ToString()))
                    objGeneralCodeFile.SortOrder = dr["SortOrder"].ToString();
                if (!string.IsNullOrEmpty(dr["ParentFileCode"].ToString()))
                    objGeneralCodeFile.ParentFileCode = dr["ParentFileCode"].ToString();
                objGeneralCodeFile.FileTypeCode = Convert.ToInt32(dr["FileTypeCode"].ToString());
                objGeneralCodeFile.LevelCode = Convert.ToInt32(dr["LevelCode"].ToString());
                if (!string.IsNullOrEmpty(dr["CompanyCode"].ToString()))
                    objGeneralCodeFile.CompanyCode_FK = Convert.ToInt32(dr["CompanyCode"].ToString());
                objGeneralCodeFile.ModuleCode = dr["ModuleCode"].ToString();
                if (!string.IsNullOrEmpty(dr["IsReadOnly"].ToString()))
                    objGeneralCodeFile.IsReadOnly = Convert.ToBoolean(Convert.ToInt16(dr["IsReadOnly"].ToString()));
                listGeneralCodeFile.Add(objGeneralCodeFile);
            }

            objDbCommand.Dispose();
            return listGeneralCodeFile;
        }

        static string GeneralCodeFile_TBL = "General_CodeFile";

        public string SaveGeneralCodeFile(GeneralCodeFile objGeneralCodeFile)
        {
            objGeneralCodeFile.TableName_TBL = GeneralCodeFile_TBL;
            ArrayList vQueryList = new ArrayList();
            string vOut = "Exception Occured !";
            int vResult = 0;
            int count = 0;
            int count_nm = 0;


            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();

            using (SqlTransaction objDbTransaction = connection.BeginTransaction())
            {
                try
                {
                    // Query for checking duplicate FileCode
                    string vValidationQuery = @"select Count(FileCode)cnt_filecode from " + GeneralCodeFile_TBL + " where CompanyCode = 1 AND FileTypeCode=" + objGeneralCodeFile.FileTypeCode + " and FileId=" + objGeneralCodeFile.FileId + "";

                    // Query for checking duplicate name
                    string vCheckDuplicateQuery = "select count(FileCode)cnt_filename  from General_CodeFile where CompanyCode = 1 AND FileTypeCode=" + objGeneralCodeFile.FileTypeCode + "  and FileName='" + objGeneralCodeFile.FileName + "'";


                    if (string.IsNullOrEmpty(objGeneralCodeFile.ParentFileCode))
                    {
                        vValidationQuery = vValidationQuery + " and ParentFileCode is null";
                        // Query for checking duplicate FileName
                        vCheckDuplicateQuery = vCheckDuplicateQuery + " and ParentFileCode is null";
                    }
                    else
                    {
                        vValidationQuery = vValidationQuery + " and ParentFileCode =" + objGeneralCodeFile.ParentFileCode + "";
                        // Query for checking duplicate FileName
                        vCheckDuplicateQuery = vCheckDuplicateQuery + " and ParentFileCode =" + objGeneralCodeFile.ParentFileCode + "";
                    }
                    if (!objGeneralCodeFile.IsNew)
                    {
                        vValidationQuery = vValidationQuery + " and FileCode!=@FileCode";
                        vCheckDuplicateQuery = vCheckDuplicateQuery + " and FileCode!=@FileCode";
                    }

                    SqlDataReader drCommandValidation;
                    //for checking duplicate FileCode
                    SqlCommand objDbCommandValidation = new SqlCommand(vValidationQuery, connection);

                    SqlDataReader drCommandCheck;
                    //for checking duplicate FileName
                    SqlCommand objDbCommandCheckDuplicate = new SqlCommand(vCheckDuplicateQuery, connection);


                    objDbCommandValidation.Parameters.AddWithValue("FileTypeCode", objGeneralCodeFile.FileTypeCode);
                    objDbCommandValidation.Parameters.AddWithValue("FileId", objGeneralCodeFile.FileId);

                    objDbCommandCheckDuplicate.Parameters.AddWithValue("FileTypeCode", objGeneralCodeFile.FileTypeCode);
                    objDbCommandCheckDuplicate.Parameters.AddWithValue("FileName", objGeneralCodeFile.FileName);

                    if (!string.IsNullOrEmpty(objGeneralCodeFile.ParentFileCode))
                    {
                        objDbCommandValidation.Parameters.AddWithValue("ParentFileCode", objGeneralCodeFile.ParentFileCode);
                        objDbCommandCheckDuplicate.Parameters.AddWithValue("ParentFileCode", objGeneralCodeFile.ParentFileCode);
                    }

                    if (!objGeneralCodeFile.IsNew)
                    {
                        objDbCommandValidation.Parameters.AddWithValue("FileCode", objGeneralCodeFile.FileCode_PK);
                        objDbCommandCheckDuplicate.Parameters.AddWithValue("FileCode", objGeneralCodeFile.FileCode_PK);
                    }

                    objDbCommandValidation.Transaction = objDbTransaction;
                    drCommandValidation = objDbCommandValidation.ExecuteReader();


                    if (drCommandValidation.Read())
                    {
                        //CommandValidationCount++;
                        count = Convert.ToUInt16(drCommandValidation["cnt_filecode"].ToString());
                    }

                    if (count > 0)
                    {
                        //connections.Close();
                        drCommandValidation.Close();
                        objDbTransaction.Rollback();
                        vOut = "Code is not unique within the scope";
                    }
                    else
                    {
                        drCommandValidation.Close();
                        objDbTransaction.Rollback();

                        using (SqlTransaction objDbTransactions = connection.BeginTransaction())
                        {
                            objDbCommandCheckDuplicate.Transaction = objDbTransactions;
                            drCommandCheck = objDbCommandCheckDuplicate.ExecuteReader();
                            //int CommandCheckCount = 0;
                            if (drCommandCheck.Read())
                            {
                                count_nm = Convert.ToUInt16(drCommandCheck["cnt_filename"].ToString());
                            }
                            if (count_nm > 0)
                            {
                                //connections.Close();
                                drCommandCheck.Close();
                                objDbTransactions.Rollback();
                                vOut = "Duplicate Name";
                            }
                            else
                            {
                                drCommandCheck.Close();
                                objDbTransactions.Rollback();

                                using (SqlTransaction trans = connection.BeginTransaction())
                                {
                                    string vComTxt = @"select isnull(max(FileCode),0) + 1  as FileCode_PK from General_CodeFile";

                                    //SqlConnection connection = _supplierDbContext.GetConn();
                                    //connection.Open();
                                    SqlDataReader dr;
                                    SqlCommand objDbCommand = new SqlCommand(vComTxt, connection, trans);
                                    dr = objDbCommand.ExecuteReader();

                                    while (dr.Read())
                                    {
                                        objGeneralCodeFile.FileCode_PK = Convert.ToInt32(dr["FileCode_PK"].ToString());
                                    }
                                    objDbCommand.Dispose();
                                    //connections.Close();
                                    dr.Close();

                                    vQueryList.Add(GetQuery(objGeneralCodeFile));

                                    vResult = 0;


                                    //SqlConnection connectionn = _supplierDbContext.GetConn();
                                    //connectionn.Open();

                                    try
                                    {
                                        using (SqlCommand command = _supplierDbContext.GetCommand())
                                        {
                                            foreach (string obj_temp in vQueryList)
                                            {

                                                command.CommandText = obj_temp;
                                                command.Transaction = trans;
                                                vResult = command.ExecuteNonQuery();
                                            }
                                            if (vResult > 0)
                                            {
                                                trans.Commit();
                                                vOut = "Records Saved.";
                                            }
                                            else
                                                vOut = " 0 Records Saved.";

                                            //if (vResult > 0 && string.IsNullOrEmpty(objGeneralCodeFile.ParentFileCode) && objGeneralCodeFile.FileTypeCode == 27)
                                            //{

                                            //}
                                        }
                                    }
                                    catch (DbException ex)
                                    {
                                        //connection.Close();
                                        //trans.Rollback();
                                        throw ex;

                                    }
                                    finally
                                    {
                                        connection.Close();
                                    }
                                    return vOut;
                                }
                            }
                        }

                    }

                }
                catch(DbException ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
                return vOut;
            }


            //using (SqlTransaction objDbTransaction = connections.BeginTransaction())
            //{
            //    // Query for checking duplicate FileCode
            //    string vValidationQuery = @"select Count(FileCode)cnt_filecode from " + GeneralCodeFile_TBL + " where CompanyCode = 1 AND FileTypeCode=" + objGeneralCodeFile.FileTypeCode + " and FileId=" + objGeneralCodeFile.FileId + "";

            //    // Query for checking duplicate name
            //    string vCheckDuplicateQuery = "select count(FileCode)cnt_filename  from General_CodeFile where CompanyCode = 1 AND FileTypeCode=" + objGeneralCodeFile.FileTypeCode + "  and FileName='" + objGeneralCodeFile.FileName + "'";


            //    if (string.IsNullOrEmpty(objGeneralCodeFile.ParentFileCode))
            //    {
            //        vValidationQuery = vValidationQuery + " and ParentFileCode is null";
            //        // Query for checking duplicate FileName
            //        vCheckDuplicateQuery = vCheckDuplicateQuery + " and ParentFileCode is null";
            //    }
            //    else
            //    {
            //        vValidationQuery = vValidationQuery + " and ParentFileCode =" + objGeneralCodeFile.ParentFileCode + "";
            //        // Query for checking duplicate FileName
            //        vCheckDuplicateQuery = vCheckDuplicateQuery + " and ParentFileCode =" + objGeneralCodeFile.ParentFileCode + "";
            //    }
            //    if (!objGeneralCodeFile.IsNew)
            //    {
            //        vValidationQuery = vValidationQuery + " and FileCode!=@FileCode";
            //        vCheckDuplicateQuery = vCheckDuplicateQuery + " and FileCode!=@FileCode";
            //    }

            //    SqlDataReader drCommandValidation;
            //    //for checking duplicate FileCode
            //    SqlCommand objDbCommandValidation = new SqlCommand(vValidationQuery, connections);


            //    SqlDataReader drCommandCheck;
            //    //for checking duplicate FileName
            //    SqlCommand objDbCommandCheckDuplicate = new SqlCommand(vCheckDuplicateQuery, connections);


            //    objDbCommandValidation.Parameters.AddWithValue("FileTypeCode", objGeneralCodeFile.FileTypeCode);
            //    objDbCommandValidation.Parameters.AddWithValue("FileId", objGeneralCodeFile.FileId);

            //    objDbCommandCheckDuplicate.Parameters.AddWithValue("FileTypeCode", objGeneralCodeFile.FileTypeCode);
            //    objDbCommandCheckDuplicate.Parameters.AddWithValue("FileName", objGeneralCodeFile.FileName);

            //    if (!string.IsNullOrEmpty(objGeneralCodeFile.ParentFileCode))
            //    {
            //        objDbCommandValidation.Parameters.AddWithValue("ParentFileCode", objGeneralCodeFile.ParentFileCode);
            //        objDbCommandCheckDuplicate.Parameters.AddWithValue("ParentFileCode", objGeneralCodeFile.ParentFileCode);
            //    }

            //    if (!objGeneralCodeFile.IsNew)
            //    {
            //        objDbCommandValidation.Parameters.AddWithValue("FileCode", objGeneralCodeFile.FileCode_PK);
            //        objDbCommandCheckDuplicate.Parameters.AddWithValue("FileCode", objGeneralCodeFile.FileCode_PK);
            //    }

            //    objDbCommandValidation.Transaction = objDbTransaction;
            //    drCommandValidation = objDbCommandValidation.ExecuteReader();
            //    //int CommandValidationCount = 0;

            //    if (drCommandValidation.Read())
            //    {
            //        //CommandValidationCount++;
            //        count = Convert.ToUInt16(drCommandValidation["cnt_filecode"].ToString());
            //    }
            //    if (count > 0)
            //    {
            //        //connections.Close();
            //        drCommandValidation.Close();
            //        objDbTransaction.Rollback();
            //        vOut = "Code is not unique within the scope";
            //    }
            //    else
            //    {
            //        drCommandValidation.Close();
            //        objDbTransaction.Rollback();
            //    }

            //    using (SqlTransaction objDbTransactions = connections.BeginTransaction())
            //    {
            //        objDbCommandCheckDuplicate.Transaction = objDbTransactions;
            //        drCommandCheck = objDbCommandCheckDuplicate.ExecuteReader();
            //        //int CommandCheckCount = 0;
            //        if (drCommandCheck.Read())
            //        {
            //            count_nm = Convert.ToUInt16(drCommandCheck["cnt_filename"].ToString());
            //        }
            //        if (count_nm > 0)
            //        {
            //            //connections.Close();
            //            drCommandCheck.Close();
            //            objDbTransactions.Rollback();
            //            vOut = "Duplicate Name";
            //        }
            //        else
            //        {
            //            drCommandCheck.Close();
            //            objDbTransactions.Rollback();
            //        }
            //    }


            //    using (SqlTransaction trans = connections.BeginTransaction())
            //    {
            //        string vComTxt = @"select isnull(max(FileCode),0) + 1  as FileCode_PK from General_CodeFile";

            //        //SqlConnection connection = _supplierDbContext.GetConn();
            //        //connection.Open();
            //        SqlDataReader dr;
            //        SqlCommand objDbCommand = new SqlCommand(vComTxt, connections, trans);
            //        dr = objDbCommand.ExecuteReader();

            //        while (dr.Read())
            //        {
            //            objGeneralCodeFile.FileCode_PK = Convert.ToInt32(dr["FileCode_PK"].ToString());
            //        }
            //        objDbCommand.Dispose();
            //        //connections.Close();
            //        dr.Close();

            //        vQueryList.Add(GetQuery(objGeneralCodeFile));

            //        vResult = 0;


            //        //SqlConnection connectionn = _supplierDbContext.GetConn();
            //        //connectionn.Open();

            //        try
            //        {
            //            using (SqlCommand command = _supplierDbContext.GetCommand())
            //            {
            //                foreach (string obj_temp in vQueryList)
            //                {

            //                    command.CommandText = obj_temp;
            //                    command.Transaction = trans;
            //                    vResult = command.ExecuteNonQuery();
            //                }
            //                if (vResult > 0)
            //                {
            //                    trans.Commit();
            //                    vOut = "Records Saved.";
            //                }
            //                else
            //                    vOut = " 0 Records Saved.";

            //                //if (vResult > 0 && string.IsNullOrEmpty(objGeneralCodeFile.ParentFileCode) && objGeneralCodeFile.FileTypeCode == 27)
            //                //{

            //                //}
            //            }
            //        }
            //        catch (DbException ex)
            //        {
            //            //connection.Close();
            //            //trans.Rollback();
            //            throw ex;

            //        }
            //        finally
            //        {
            //            connections.Close();
            //        }
            //        return vOut;
            //    }

            //}
            
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
    }
}
