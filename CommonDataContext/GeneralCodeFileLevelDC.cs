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
    public class GeneralCodeFileLevelDC : IGeneralCodeFileLevel
    {
        ISupplierDbContext _supplierDbContext;
        ICRUD _cRUD;
        public GeneralCodeFileLevelDC(ISupplierDbContext supplierDbContext, ICRUD cRUD)
        {
            _supplierDbContext = supplierDbContext;
            _cRUD = cRUD;
        }

        private static string GeneralCodeFileType_TBL = "General_CodeFileLevel";

        public List<GeneralCodeFileLevel> GetGeneralCodeFileLevelByFileType(Int32 fileTypeCode_PK, Int32 companyCode_PK)
        {
            List<GeneralCodeFileLevel> listGeneralCodeFileLevel = new List<GeneralCodeFileLevel>();
            GeneralCodeFileLevel objGeneralCodeFileLevel;
            string vComTxt = @"SELECT [LevelCode]
                            ,[UserCode]
                            ,[ActionType]
                            ,[ActionDate]
                            ,[LevelId]
                            ,[LevelName]
                            ,[FileTypeCode]
                            ,[CompanyCode]
                            ,[FileIdStartFrom]
                            ,[ModuleCode]
                            FROM [dbo].[General_CodeFileLevel]
                            where FileTypeCode=" + fileTypeCode_PK + " And CompanyCode = " + companyCode_PK + "";


            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            dr = objDbCommand.ExecuteReader();

            while (dr.Read())
            {
                objGeneralCodeFileLevel = new GeneralCodeFileLevel();
                objGeneralCodeFileLevel.LevelCode_PK = Convert.ToInt32(dr["LevelCode"].ToString());
                objGeneralCodeFileLevel.UserCode = dr["UserCode"].ToString();
                objGeneralCodeFileLevel.ActionType = dr["ActionType"].ToString();
                objGeneralCodeFileLevel.LevelId = dr["LevelId"].ToString();
                objGeneralCodeFileLevel.LevelName = dr["LevelName"].ToString();
                objGeneralCodeFileLevel.FileTypeCode = Convert.ToInt32(dr["FileTypeCode"].ToString());
                if (!string.IsNullOrEmpty(dr["CompanyCode"].ToString()))
                    objGeneralCodeFileLevel.CompanyCode_FK = Convert.ToUInt16(dr["CompanyCode"].ToString());
                objGeneralCodeFileLevel.FileIdStartFrom = dr["FileIdStartFrom"].ToString();
                objGeneralCodeFileLevel.ModuleCode = dr["ModuleCode"].ToString();
                listGeneralCodeFileLevel.Add(objGeneralCodeFileLevel);
            }

            return listGeneralCodeFileLevel;
        }

        public string SaveGeneralCodeFileLevel(GeneralCodeFileLevel objGeneralCodeFileLevel)
        {

            objGeneralCodeFileLevel.TableNm_TBL = GeneralCodeFileType_TBL;
            ArrayList vQueryList = new ArrayList();
            string vOut = "Exception Occured !";

            if (objGeneralCodeFileLevel.IsNew)
            {
                string vComTxt = @"select (isnull(max(LevelCode),0) +1) as LevelCode_PK from General_CodeFileLevel";

                SqlConnection connections = _supplierDbContext.GetConn();
                connections.Open();
                SqlDataReader dr;
                SqlCommand objDbCommand = new SqlCommand(vComTxt, connections);
                dr = objDbCommand.ExecuteReader();

                while (dr.Read())
                {
                    objGeneralCodeFileLevel.LevelCode_PK = Convert.ToInt32(dr["LevelCode_PK"].ToString());
                }
                objDbCommand.Dispose();
                connections.Close();

                vQueryList.Add(GetQuery(objGeneralCodeFileLevel));

                int vResult = 0;
                
                
                SqlConnection connection = _supplierDbContext.GetConn();
                connection.Open();

                using (SqlTransaction trans = connection.BeginTransaction())
                {


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
                                vOut = "Code File Level Saved Successfully";
                            }
                            else
                                vOut = "Level Code is duplicate";
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
    }
}
