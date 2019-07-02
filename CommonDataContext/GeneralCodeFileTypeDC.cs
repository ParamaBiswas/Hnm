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
    public class GeneralCodeFileTypeDC : IGeneralCodeFileType
    {
        ISupplierDbContext _supplierDbContext;
        ICRUD _cRUD;
        public GeneralCodeFileTypeDC(ISupplierDbContext supplierDbContext, ICRUD cRUD)
        {
            _supplierDbContext = supplierDbContext;
            _cRUD = cRUD;
        }

        private static string GeneralCodeFileType_TBL = "General_CodeFileType";

        public string SaveGeneralCodeFileType(GeneralCodeFileType objGeneralCodeFileType)
        {
            int vResult = 0;
            string vOut = "Exception Occured !";
            ArrayList vQueryList = new ArrayList();
            objGeneralCodeFileType.TableName_TBL = GeneralCodeFileType_TBL;
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();


            using (SqlTransaction trans = connection.BeginTransaction())
            {

                vQueryList.Add(GetQuery(objGeneralCodeFileType));

                try
                {
                    using (SqlCommand command = _supplierDbContext.GetCommand())
                    {
                        //command.Parameters.AddWithValue("FileTypeCode", objGeneralCodeFileType.FileTypeCode_PK);
                        foreach (string obj_temp in vQueryList)
                        {
                            command.CommandText = obj_temp;
                            command.Transaction = trans;
                            vResult = command.ExecuteNonQuery();
                        }
                        if (vResult > 0)
                        {
                            trans.Commit();
                            vOut = "Code File Type Saved Successfully";
                        }
                        else
                            vOut = "File Type Code is duplicate";
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

        public List<GeneralCodeFileType> GetGeneralCodeFileTypeAll(string pModuleCode, Int32 COMPANY_CODE)
        {
            List<GeneralCodeFileType> listGeneralCodeFileType = new List<GeneralCodeFileType>();
            GeneralCodeFileType objGeneralCodeFileType;
            string vComTxt = @"SELECT T.[FileTypeCode]
                            ,T.[UserCode]
                            ,T.[ActionType]
                            ,T.[ActionDate]
                            ,T.[FileTypeName]
                            ,T.[CompanyCode]
                            ,T.[ModuleCode]
                            ,L.[CountLevel]
                            FROM [dbo].[General_CodeFileType] as  T left outer join (Select FileTypeCode, Count(LevelCode) as CountLevel from  [dbo].[General_CodeFileLevel] group by FileTypeCode) as L
                            on T.FileTypeCode=L.FileTypeCode
                            where T.FileTypeCode !=0  
                            and T.IsHidden !=1 
                            and T.ModuleCode=" + pModuleCode + " And T.CompanyCode = " + COMPANY_CODE + " ORDER BY T.[ActionDate] DESC";

            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            objDbCommand.Parameters.AddWithValue("ModuleCode", pModuleCode);
            objDbCommand.Parameters.AddWithValue("CompanyCode", COMPANY_CODE);
            dr = objDbCommand.ExecuteReader();

            while (dr.Read())
            {
                objGeneralCodeFileType = new GeneralCodeFileType();
                objGeneralCodeFileType.FileTypeCode_PK = Convert.ToInt32(dr["FileTypeCode"].ToString());
                objGeneralCodeFileType.UserCode = dr["UserCode"].ToString();
                objGeneralCodeFileType.ActionType = dr["ActionType"].ToString();
                objGeneralCodeFileType.FileTypeName = dr["FileTypeName"].ToString();
                if (!string.IsNullOrEmpty(dr["CompanyCode"].ToString()))
                    objGeneralCodeFileType.CompanyCode_FK = Convert.ToInt32(dr["CompanyCode"].ToString());
                objGeneralCodeFileType.ModuleCode = dr["ModuleCode"].ToString();
                if (!string.IsNullOrEmpty(dr["CountLevel"].ToString()))
                    objGeneralCodeFileType.CountLevel_VW = Convert.ToUInt16(dr["CountLevel"].ToString());
                else
                    objGeneralCodeFileType.CountLevel_VW = 0;
                listGeneralCodeFileType.Add(objGeneralCodeFileType);
            }
            objDbCommand.Dispose();
            return listGeneralCodeFileType;
        }

        public GeneralCodeFileType GetGeneralCodeFileTypeByKey(Int32 FileTypeCode, Int32 COMPANY_CODE)
        {
            GeneralCodeFileType objGeneralCodeFileType = new GeneralCodeFileType();
            objGeneralCodeFileType.TableName_TBL = GeneralCodeFileType_TBL;
            objGeneralCodeFileType.FileTypeCode_PK = FileTypeCode;
            objGeneralCodeFileType.CompanyCode_FK = COMPANY_CODE;

            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            string vComTxt = GetData(objGeneralCodeFileType);
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            dr = objDbCommand.ExecuteReader();

            if (dr.Read())
            {
                objGeneralCodeFileType.FileTypeCode_PK = Convert.ToInt32(dr["FileTypeCode"].ToString());
                objGeneralCodeFileType.UserCode = dr["UserCode"].ToString();
                objGeneralCodeFileType.ActionType = dr["ActionType"].ToString();
                objGeneralCodeFileType.FileTypeName = dr["FileTypeName"].ToString();
                objGeneralCodeFileType.CompanyCode_FK = Convert.ToInt32(dr["CompanyCode"].ToString());
                objGeneralCodeFileType.ModuleCode = dr["ModuleCode"].ToString();
                if (!string.IsNullOrEmpty(dr["IsHidden"].ToString()))
                    objGeneralCodeFileType.IsHidden = Convert.ToBoolean(Convert.ToUInt16(dr["IsHidden"].ToString()));
                objGeneralCodeFileType.IsNew = false;
                
                
            }

            //objGeneralCodeFileType.ListGeneralCodeFileLevel = GeneralCodeFileLevelDC.GetGeneralCodeFileLevelByFileType(FileTypeCode);
            return objGeneralCodeFileType;
        }

        public string GetData(object objObject)
        {
            BaseModel obj_Temp = (BaseModel)objObject;
            string vQuery = "";

            vQuery = _cRUD.READQuery(objObject, false);
            
            return vQuery;
        }

        public List<StaticItem> GetModuleList()
        {
            List<StaticItem> ListModule = new List<StaticItem>();
            StaticItem objStaticItemSelect = new StaticItem();
            objStaticItemSelect.DataValue = "0";
            objStaticItemSelect.TextValue = "Select";
            ListModule.Add(objStaticItemSelect);
            string sSql = @"select ModuleCode, ModuleName from LS_Module";

            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(sSql, connection);
            dr = objDbCommand.ExecuteReader();

            while (dr.Read())
            {
                StaticItem objStaticItem = new StaticItem();
                objStaticItem.DataValue = dr["ModuleCode"].ToString();
                objStaticItem.TextValue = dr["ModuleName"].ToString();
                ListModule.Add(objStaticItem);
            }

            return ListModule;
        }

    }
}
