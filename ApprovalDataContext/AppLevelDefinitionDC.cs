using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using ApprovalInterface;
using ApprovalModel;
using ConnectionGateway;
using LSCrud;

namespace ApprovalDataContext
{
    public class AppLevelDefinitionDC: IAppObjInfoMap_Logic,IAppLevelDefinition
    {
        ISupplierDbContext _supplierDbContext;
        ICRUD _CRUDBuilder;
        public AppLevelDefinitionDC(ISupplierDbContext supplierDbContext, ICRUD CRUDBuilder)
        {
            _supplierDbContext = supplierDbContext;
            _CRUDBuilder = CRUDBuilder;
        }
        public AppObjInfoMap_Logic GetIAppObjInfoMap_Logic(AppObjInfoMap_Logic objIAppObjInfoMap_Logic)
        {

            string vComTxt = @"SELECT Distinct  A.[ObjAppInfoMapLogicCode]
                                  ,A.[ModuleObjMapCode]
                                  ,A.[IsFieldPolicy]
                                  ,A.[FieldType1]
                                  ,A.[DBFieldName1]
                                  ,A.[FieldType2]
                                  ,A.[DBFieldName2]
                                  ,A.[FieldType3]
                                  ,A.[DBFieldName3]
                              FROM [LS_AppObjInfoMap_Logic] A
                                        JOIN LS_AppObjectInfoMap B ON A.ModuleObjMapCode = B.ModuleObjMapCode
                                    AND A.CompanyCode = 1 WHERE A.ActionType<>'DELETE'  AND B.Active=1";


            if (!String.IsNullOrEmpty(objIAppObjInfoMap_Logic.ModuleObjMapCode_FK))
                vComTxt += @" And  A.ModuleObjMapCode ='" + objIAppObjInfoMap_Logic.ModuleObjMapCode_FK + "'";
            SqlConnection connection = _supplierDbContext.GetConn();
            try
            {
                
                connection.Open();
                SqlDataReader dr;
                SqlCommand command = new SqlCommand(vComTxt, connection);
                dr = command.ExecuteReader();
                //while (dr.Read())
                //{
                    if (dr.Read())
                    {

                        objIAppObjInfoMap_Logic.ObjAppInfoMapLogicCode_PK = dr["ObjAppInfoMapLogicCode"].ToString();
                        objIAppObjInfoMap_Logic.ModuleObjMapCode_FK = dr["ModuleObjMapCode"].ToString();

                        objIAppObjInfoMap_Logic.IsFieldPolicy = Convert.ToInt32(dr["IsFieldPolicy"]);

                        if (objIAppObjInfoMap_Logic.IsFieldPolicy == 1)
                        {
                            if (!string.IsNullOrEmpty(dr["FieldType1"].ToString()))
                            {
                                objIAppObjInfoMap_Logic.FieldType1 = Convert.ToInt32(dr["FieldType1"]);
                            }

                            if (!string.IsNullOrEmpty(dr["DBFieldName1"].ToString()))
                            {
                                objIAppObjInfoMap_Logic.DBFieldName1 = dr["DBFieldName1"].ToString();
                            }
                            if (!string.IsNullOrEmpty(dr["FieldType2"].ToString()))
                            {
                                objIAppObjInfoMap_Logic.FieldType2 = Convert.ToInt32(dr["FieldType2"]);
                            }
                            if (!string.IsNullOrEmpty(dr["DBFieldName2"].ToString()) && dr["DBFieldName2"].ToString() != "-1")
                            {
                                objIAppObjInfoMap_Logic.DBFieldName2 = dr["DBFieldName2"].ToString();
                            }

                            if (!string.IsNullOrEmpty(dr["FieldType3"].ToString()))
                            {
                                objIAppObjInfoMap_Logic.FieldType3 = Convert.ToInt32(dr["FieldType3"]);
                            }

                            if (!string.IsNullOrEmpty(dr["DBFieldName3"].ToString()) && dr["DBFieldName3"].ToString() != "-1")
                            {
                                objIAppObjInfoMap_Logic.DBFieldName3 = dr["DBFieldName3"].ToString();
                            }

                            //if (!string.IsNullOrEmpty(dr["ActiveYN"].ToString()))
                            //{
                            //    objIAppObjInfoMap_Logic.ActiveYN = Convert.ToInt32(dr["ActiveYN"]);
                            //}
                            GetAppObjInfoMapLogicValuesList(objIAppObjInfoMap_Logic);
                        }
                    }
                    else
                    {
                        objIAppObjInfoMap_Logic = null;
                    }
                //}

            }

            catch (DbException ex)
            {
                
                    throw ex;
            }
            finally
            {
                connection.Close();
            }


            return objIAppObjInfoMap_Logic;

        }

        private  void GetAppObjInfoMapLogicValuesList(AppObjInfoMap_Logic objAppObjInfoMap_Logic)
        {
            List<AppObjInfoMap_LogicValues> objNewAppObjInfoMap_LogicValuesList = new List<AppObjInfoMap_LogicValues>();

            string vComTxt = @"SELECT A.[ObjAppInfoMapLogicValueCode]
                                  ,A.[ObjAppInfoMappLogicCode]
                                  ,A.[LogicSerialNo]
                                  ,A.[OptionValue]
                                  ,A.[OptionText]
                                  ,A.[OptionFor]
                              FROM [LS_AppObjInfoMap_LogicValues] A
                            WHERE A.ObjAppInfoMappLogicCode = '" + objAppObjInfoMap_Logic.ObjAppInfoMapLogicCode_PK + "' AND A.ActionType<>'DELETE'";
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand command = new SqlCommand(vComTxt, connection);
            dr = command.ExecuteReader();
            
                while (dr.Read())
                {
                    AppObjInfoMap_LogicValues objAppLogicValues = new AppObjInfoMap_LogicValues();

                    objAppLogicValues.ObjAppInfoMapLogicValueCode_PK = dr["ObjAppInfoMapLogicValueCode"].ToString();
                    objAppLogicValues.ObjAppInfoMappLogicCode_FK = dr["ObjAppInfoMappLogicCode"].ToString();

                    if (!string.IsNullOrEmpty(dr["LogicSerialNo"].ToString()))
                    {
                        objAppLogicValues.LogicSerialNo = Convert.ToInt32(dr["LogicSerialNo"]);
                    }

                    objAppLogicValues.OptionValue = dr["OptionValue"].ToString();
                    objAppLogicValues.OptionText = dr["OptionText"].ToString();
                    objAppLogicValues.OptionFor = Convert.ToInt32(dr["OptionFor"]);

                    objNewAppObjInfoMap_LogicValuesList.Add(objAppLogicValues);

                }


            if (objNewAppObjInfoMap_LogicValuesList != null)
            {
                objAppObjInfoMap_Logic.AppObjInfoMap_LogicValues_List_VW = objNewAppObjInfoMap_LogicValuesList;
            }
        }

        public AppLevelDefinition GetAppLevelDefinition(AppLevelDefinition objI)
        {
            string vComTxt = @"Select DISTINCT A.AppLvDefinitionCode
                                      ,A.ModuleObjMapCode
                                      ,A.AppLevelType,B.ActiveYN
                                from LS_AppLevelDefinition A JOIN LS_AppLevelDefinitionDet B ON A.AppLvDefinitionCode = B.AppLvDefinitionCode
                            WHERE A.ModuleObjMapCode = '" + objI.ModuleObjMapCode_FK + "' "
                           + " AND A.ActionType<>'DELETE' AND  B.ActionType<>'DELETE' AND B.ActiveYN = 1 AND A.CompanyCode = 1";

            SqlConnection connection = _supplierDbContext.GetConn();
            try
            {
                
                connection.Open();
                SqlDataReader dr;
                SqlCommand command = new SqlCommand(vComTxt, connection);
                dr = command.ExecuteReader();

                //while (dr.Read())
                //{
                    if (dr.Read())
                    {

                        objI.AppLvDefinitionCode_PK = dr["AppLvDefinitionCode"].ToString();
                        objI.ModuleObjMapCode_FK = dr["ModuleObjMapCode"].ToString();

                        objI.AppLevelType = Convert.ToInt32(dr["AppLevelType"]);
                        objI.ActiveYN_VW = Convert.ToInt32(dr["ActiveYN"]);

                        GetAppLevelDefinitionDet(objI);

                    }
                    else
                    {
                        objI = new AppLevelDefinition();
                    }
                //}
                   

            }

            catch (DbException ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return objI;
        }

        private void GetAppLevelDefinitionDet(AppLevelDefinition objI)
        {
            string vComTxt = @"SELECT A.[AppLvDefinitionDetCode]
                                  ,A.[AppLvDefinitionCode]
                                  ,A.[ObjAppInfoMapLogicCode]
                                  ,A.[ValueFrom1]
                                  ,A.[ValueTo1]
                                  ,A.[ValueFrom2]
                                  ,A.[ValueTo2]
                                  ,A.[ValueFrom3]
                                  ,A.[ValueTo3]
                                  ,A.[NoOfAppLevel]
                                  ,B.OptionText AS OptionText1
                                  ,C.OptionText AS OptionText2
                                  ,D.OptionText AS OptionText3
                                  ,A.SLNo
                                  ,A.ActiveYN
                                    
                              FROM [LS_AppLevelDefinitionDet]  A
                                LEFT JOIN (Select ObjAppInfoMappLogicCode,OptionText,OptionValue from LS_AppObjInfoMap_LogicValues where OptionFor = 1  AND ActionType<>'DELETE')  B
                                on A.ObjAppInfoMapLogicCode = B.ObjAppInfoMappLogicCode AND A.ValueFrom1 = B.OptionValue
	
                                LEFT JOIN (Select ObjAppInfoMappLogicCode,OptionText,OptionValue from LS_AppObjInfoMap_LogicValues where OptionFor = 2 AND ActionType<>'DELETE')  C
                                on A.ObjAppInfoMapLogicCode = C.ObjAppInfoMappLogicCode AND A.ValueFrom2 = C.OptionValue
	
                                LEFT JOIN (Select ObjAppInfoMappLogicCode,OptionText,OptionValue from LS_AppObjInfoMap_LogicValues where OptionFor = 3 AND ActionType<>'DELETE')  D
                                on A.ObjAppInfoMapLogicCode = D.ObjAppInfoMappLogicCode AND A.ValueFrom3 = D.OptionValue
                            WHERE A.AppLvDefinitionCode = '" + objI.AppLvDefinitionCode_PK + "' AND A.ActionType<>'DELETE' AND  A.ActiveYN = 1" +
                            " Order BY A.SLNo ";

            SqlConnection connection = _supplierDbContext.GetConn();
            try
            {
                connection.Open();
                SqlDataReader dr;
                SqlCommand command = new SqlCommand(vComTxt, connection);
                dr = command.ExecuteReader();
                objI.objIAppLevelDefinitionDetList_VW = new List<AppLevelDefinitionDet>();
                while (dr.Read())
                    {
                        AppLevelDefinitionDet objAppLevelDefinitionDet = new AppLevelDefinitionDet();

                        objAppLevelDefinitionDet.AppLvDefinitionDetCode_PK = dr["AppLvDefinitionDetCode"].ToString();
                        objAppLevelDefinitionDet.AppLvDefinitionCode_FK = dr["AppLvDefinitionCode"].ToString();
                        objAppLevelDefinitionDet.ObjAppInfoMapLogicCode_FK = dr["ObjAppInfoMapLogicCode"].ToString();

                        if (!string.IsNullOrEmpty(dr["SLNo"].ToString()))
                        {
                            objAppLevelDefinitionDet.SlNo = Convert.ToInt32(dr["SLNo"]);
                        }

                        if (!string.IsNullOrEmpty(dr["ValueFrom1"].ToString()))
                        {
                            objAppLevelDefinitionDet.ValueFrom1 = Convert.ToDecimal(dr["ValueFrom1"]);
                        }

                        if (!string.IsNullOrEmpty(dr["ValueTo1"].ToString()))
                        {
                            objAppLevelDefinitionDet.ValueTo1 = Convert.ToDecimal(dr["ValueTo1"]);
                        }

                        if (!string.IsNullOrEmpty(dr["ValueFrom2"].ToString()))
                        {
                            objAppLevelDefinitionDet.ValueFrom2 = Convert.ToDecimal(dr["ValueFrom2"]);
                        }
                        if (!string.IsNullOrEmpty(dr["ValueTo2"].ToString()))
                        {
                            objAppLevelDefinitionDet.ValueTo2 = Convert.ToDecimal(dr["ValueTo2"]);
                        }

                        if (!string.IsNullOrEmpty(dr["ValueFrom3"].ToString()))
                        {
                            objAppLevelDefinitionDet.ValueFrom3 = Convert.ToDecimal(dr["ValueFrom3"]);
                        }

                        if (!string.IsNullOrEmpty(dr["ValueTo3"].ToString()))
                        {
                            objAppLevelDefinitionDet.ValueTo3 = Convert.ToDecimal(dr["ValueTo3"]);
                        }

                        if (!string.IsNullOrEmpty(dr["NoOfAppLevel"].ToString()))
                        {
                            objAppLevelDefinitionDet.NoOfAppLevel = Convert.ToInt32(dr["NoOfAppLevel"]);
                        }

                        if (!string.IsNullOrEmpty(dr["OptionText1"].ToString()))
                        {
                            objAppLevelDefinitionDet.OptionText1_VW = dr["OptionText1"].ToString();
                        }

                        if (!string.IsNullOrEmpty(dr["OptionText2"].ToString()))
                        {
                            objAppLevelDefinitionDet.OptionText2_VW = dr["OptionText2"].ToString();
                        }

                        if (!string.IsNullOrEmpty(dr["OptionText3"].ToString()))
                        {
                            objAppLevelDefinitionDet.OptionText3_VW = dr["OptionText3"].ToString();
                        }


                        if (!string.IsNullOrEmpty(dr["ActiveYN"].ToString()))  //added by Tinne
                        {
                            objAppLevelDefinitionDet.ActiveYN = Convert.ToInt32(dr["ActiveYN"]);
                        }

                        objI.objIAppLevelDefinitionDetList_VW.Add(objAppLevelDefinitionDet);

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
        }
    }
}
