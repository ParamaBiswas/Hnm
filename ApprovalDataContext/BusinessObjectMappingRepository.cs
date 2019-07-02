using ApprovalInterface;
using ApprovalModel;
using CommonInterface;
using CommonModel;
using ConnectionGateway;
using LSCrud;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace ApprovalDataContext
{
    public class BusinessObjectMappingRepository: IAppModuleObjectMapping
    {
        ISupplierDbContext _supplierDbContext;
        ICRUD _cRUD;
        IIDGenCriteriaInfo _iIDGenCriteriaInfo;
        public BusinessObjectMappingRepository(ISupplierDbContext supplierDbContext, ICRUD cRUD, IIDGenCriteriaInfo iDGenCriteriaInfo)
        {
            _supplierDbContext = supplierDbContext;
            _cRUD = cRUD;
            _iIDGenCriteriaInfo = iDGenCriteriaInfo;
        }

        static readonly string LS_AppModuleObjectMapping_TBL = "LS_AppModuleObjectMapping";
        static readonly string LS_AppObjectInfoMap_TBL = "LS_AppObjectInfoMap";
        static readonly string LS_ModuleList_Table = "LS_Module";

        public List<StaticItem> GetModuleName()
        {
            List<StaticItem> objOfModuleMappingList = new List<StaticItem>();
            string vComTxt = @"select ModuleCode,ModuleName from LS_Module ";
            try
            {
                SqlConnection connection = _supplierDbContext.GetConn();
                connection.Open();
                SqlDataReader dr;
                SqlCommand command = new SqlCommand(vComTxt, connection);
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    StaticItem objModuleObjMapping = new StaticItem();

                    objModuleObjMapping.DataValue = dr["ModuleCode"].ToString();
                    objModuleObjMapping.TextValue = dr["ModuleName"].ToString();

                    objOfModuleMappingList.Add(objModuleObjMapping);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objOfModuleMappingList;

        }

        public string SaveMappedBusinessObject(List<AppModuleObjectMapping> objIAppModuleObjectMappingList)
        {
            int vResult = 0;
            int vResult1 = 0;
            string vComTxt = string.Empty;
            string vOut = "Exception Occured !";
            List<string> vQueryList = new List<string>();
            
            foreach (AppModuleObjectMapping objIApp in objIAppModuleObjectMappingList)
            {
                objIApp.TableName_TBL = "LS_AppModuleObjectMapping";


                if (objIApp.IsNew)
                {
                    objIApp.ModuleObjMapCode_PK = Guid.NewGuid().ToString();
                }
            }
            if (objIAppModuleObjectMappingList != null && objIAppModuleObjectMappingList.Count > 0)
            {

                for (int i = 0; i < objIAppModuleObjectMappingList.Count; i++)
                {

                    if (objIAppModuleObjectMappingList[i].IsNew == true && objIAppModuleObjectMappingList[i].IsDeleted == false)
                    {


                        vQueryList.Add(_cRUD.CREATEQuery(objIAppModuleObjectMappingList[i]));
                    }

                    else if (objIAppModuleObjectMappingList[i].IsNew == false && objIAppModuleObjectMappingList[i].IsDeleted == false)
                    {
                        vQueryList.Add(_cRUD.UPDATEQuery(objIAppModuleObjectMappingList[i]));
                    }
                    else if (objIAppModuleObjectMappingList[i].IsNew == false && objIAppModuleObjectMappingList[i].IsDeleted == true)
                    {
                        Hashtable pMarkDELColNameValues = new Hashtable();
                        pMarkDELColNameValues.Clear();
                        objIAppModuleObjectMappingList[i].ActionType = "DELETE";
                        pMarkDELColNameValues.Add("ActionType", objIAppModuleObjectMappingList[i].ActionType);
                        pMarkDELColNameValues.Add("ActionDate", objIAppModuleObjectMappingList[i].ActionDate);
                        pMarkDELColNameValues.Add("UserCode", objIAppModuleObjectMappingList[i].UserCode);
                        pMarkDELColNameValues.Add("CompanyCode", objIAppModuleObjectMappingList[i].CompanyCode_FK);

                        vQueryList.Add(_cRUD.DELETEQuery(objIAppModuleObjectMappingList[i], true, pMarkDELColNameValues));
                    }

                }

            }
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
                            vOut = "Information Saved Successfully";
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
        public List<AppModuleObjectMapping> GetMappingBusinessObjectDetails()
        {
            List<AppModuleObjectMapping> objIBusinessObjectDetailsList = new List<AppModuleObjectMapping>();
            string vComTxt = @"select Distinct
                                A.ModuleObjMapCode,
		                        A.ModuleCode,
		                        C.ModuleName,
		                        A.ModuleObjName,
		                        A.ModuleObjId,
		                        B.ModuleObjMapCode as MapCode
                              FROM " + LS_AppModuleObjectMapping_TBL + @" A
                              LEFT JOIN " + LS_AppObjectInfoMap_TBL + @"  B ON (A.ModuleObjId=B.ModuleObjId and B.ActionType<>'DELETE' AND A.CompanyCode=B.CompanyCode) 
                              LEFT JOIN " + LS_ModuleList_Table + @" C on  (A.ModuleCode=C.ModuleCode) 
                            where A.CompanyCode = 1 AND A.ActionType<>'DELETE' order by ModuleObjId";
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand command = new SqlCommand(vComTxt, connection);
            dr = command.ExecuteReader();
            while (dr.Read())
            {
                AppModuleObjectMapping objOfGettingBusinessObjectDetails = new AppModuleObjectMapping();

                if (dr["ModuleObjMapCode"] != DBNull.Value)
                    objOfGettingBusinessObjectDetails.ModuleObjMapCode_PK = dr["ModuleObjMapCode"].ToString();

                if (dr["ModuleCode"] != DBNull.Value)
                    objOfGettingBusinessObjectDetails.ModuleCode = Convert.ToInt32(dr["ModuleCode"]);

                if (dr["ModuleName"] != DBNull.Value)
                    objOfGettingBusinessObjectDetails.ModuleName_VW = dr["ModuleName"].ToString();

                if (dr["ModuleObjName"] != DBNull.Value)
                    objOfGettingBusinessObjectDetails.ModuleObjName = dr["ModuleObjName"].ToString();

                if (dr["ModuleObjId"] != DBNull.Value)
                    objOfGettingBusinessObjectDetails.ModuleObjId = Convert.ToInt32(dr["ModuleObjId"]);

                if (dr["MapCode"] != DBNull.Value)
                    objOfGettingBusinessObjectDetails.ObjMapGridCode_VW = dr["MapCode"].ToString();

                objOfGettingBusinessObjectDetails.IsNew = false;

                objIBusinessObjectDetailsList.Add(objOfGettingBusinessObjectDetails);

            }
            return objIBusinessObjectDetailsList;
        }
    }
}
