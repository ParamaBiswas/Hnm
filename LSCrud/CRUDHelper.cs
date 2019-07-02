using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LSCrud
{
    public sealed class CRUDHelper
    {
        internal static string RemoveSuffix(string pOriginalString, string pRemovee)
        {
            string vReturnString = "";
            vReturnString = pOriginalString.Remove(pOriginalString.Length - pRemovee.Length, pRemovee.Length);
            return vReturnString;
        }
        internal static string GetFormatedColumnValue(PropertyInfo pProperty, object pObj, string pFormat)
        {
            object vReturnString;
            string vType = "";
            if (pProperty.PropertyType.IsGenericType)
                vType = Nullable.GetUnderlyingType(pProperty.PropertyType).FullName;
            else
                vType = pProperty.PropertyType.FullName;

            switch (vType)
            {
                case "System.Int16": vReturnString = pProperty.GetValue(pObj, null); break;
                case "System.Int32": vReturnString = pProperty.GetValue(pObj, null); break;
                case "System.Int64": vReturnString = pProperty.GetValue(pObj, null); break;
                case "System.Double": vReturnString = pProperty.GetValue(pObj, null); break;
                case "System.SByte": vReturnString = pProperty.GetValue(pObj, null); break;
                case "System.Boolean":
                    if (pProperty.GetValue(pObj, null).Equals(true))
                        vReturnString = "1";
                    else
                        vReturnString = "0";
                    break;
                case "System.Byte": vReturnString = pProperty.GetValue(pObj, null); break;
                case "System.Decimal": vReturnString = pProperty.GetValue(pObj, null); break;
                case "System.Single": vReturnString = pProperty.GetValue(pObj, null); break;

                default:
                    if (pProperty.GetValue(pObj, null) == null)
                        vReturnString = null;
                    else if (pProperty.GetValue(pObj, null).ToString() == "")
                        vReturnString = null;
                    else
                        vReturnString = "'" + pProperty.GetValue(pObj, null).ToString().Replace("'", "''") + "'";
                    break;
            }
            if (vReturnString != null)
                return vReturnString.ToString();
            else
                return "NULL";
        }
        internal static string GetFormatedColumnValue(object pObj)
        {
            if (pObj == null || pObj.Equals("") == true)
                return "NULL";

            string vReturnString;
            string vType = pObj.GetType().ToString();

            switch (vType)
            {
                case "System.Int16": vReturnString = pObj.ToString(); break;
                case "System.Int32": vReturnString = pObj.ToString(); break;
                case "System.Int64": vReturnString = pObj.ToString(); break;
                case "System.Double": vReturnString = pObj.ToString(); break;
                case "System.SByte": vReturnString = pObj.ToString(); break;
                case "System.Byte": vReturnString = pObj.ToString(); break;
                case "System.Decimal": vReturnString = pObj.ToString(); break;
                case "System.Single": vReturnString = pObj.ToString(); break;
                case "System.Boolean":
                    bool v = (bool)pObj;
                    if (v)
                        vReturnString = "1";
                    else
                        vReturnString = "0";
                    break;

                default:
                    vReturnString = "'" + pObj.ToString().Replace("'", "''") + "'";
                    break;
            }
            return vReturnString;
        }
        internal static string CreateOrderBy(string[] pOrderColumns)
        {
            string vReturnOrderBy = "";
            if (pOrderColumns != null)
            {
                StringBuilder vOrderBy = new StringBuilder(" ORDER BY ");
                for (int i = 0; i < pOrderColumns.Length; i++)
                {
                    vOrderBy.Append(pOrderColumns[i]);
                    vOrderBy.Append(",");
                }
                vOrderBy.Remove(vOrderBy.Length - 1, 1);
                vReturnOrderBy = vReturnOrderBy.ToString();
            }
            return vReturnOrderBy;
        }
        internal static bool CheckFilteredColumn(string pColumnName)
        {
            //If the following column names exist then return false otherwise return true
            if (!pColumnName.Contains("_VW") && !pColumnName.Contains("_TBL") && !pColumnName.Contains("IsNew") && !pColumnName.Contains("IsDeleted") && !pColumnName.Contains("IsDirty"))
                return true;
            else
                return false;
        }
        internal static Hashtable GetColumnValueCol(object pModel)
        {
            Hashtable vHT = new Hashtable();
            PropertyInfo[] vPI = pModel.GetType().GetProperties();

            foreach (PropertyInfo pi in vPI)
            {
                vHT.Add(pi.Name, pi.GetValue(pModel, null));
            }
            return vHT;
        }
        internal static StringBuilder BuildREADQuery(Hashtable pHT, StringBuilder pReadQuery, bool pAll)
        {
            //This will add the select column names
            StringBuilder vRead = new StringBuilder();
            //This will add the Column values
            StringBuilder vWhere = new StringBuilder(" WHERE ");
            string vTableName = "";
            string vColumnName = "";
            bool vPK = false;
            bool vTBL = false;

            //PropertyInfo[] vPI = pModel.GetType().GetProperties();
            foreach (DictionaryEntry de in pHT)
            //foreach (PropertyInfo pi in vPI)
            {
                //vColumnName = pi.Name;
                vColumnName = de.Key.ToString();
                if (CheckFilteredColumn(vColumnName))
                {
                    if (vColumnName.Contains("_FK"))
                    {
                        vColumnName = CRUDHelper.RemoveSuffix(vColumnName, "_FK");
                        vRead.Append(vColumnName);
                        vRead.Append(",");
                    }
                    //Adding column name followed by comma
                    else if (vColumnName.Contains("_PK"))//Property with _PK contains primary column name
                    {
                        vPK = true;//Setting true to ensure that where clause will be applied 
                        //Removing _PK from the property name. 
                        vColumnName = CRUDHelper.RemoveSuffix(vColumnName, "_PK");
                        vRead.Append(vColumnName);
                        vRead.Append(",");
                        //Generating where clause
                        vWhere.Append(vColumnName);
                        vWhere.Append(" = ");

                        string vColumnValue = CRUDHelper.GetFormatedColumnValue(de.Value);
                        vWhere.Append(vColumnValue);
                        vWhere.Append(" AND ");
                    }
                    else
                    {
                        vRead.Append(vColumnName);
                        vRead.Append(",");
                    }
                }
                else if (vColumnName.Contains("_TBL"))//Property with _TBL contains table name
                {
                    vTBL = true;
                    //Getting table name
                    vTableName = de.Value.ToString();
                }
            }//foreach(DictionaryEntry de in vHT)

            if (!vPK)
                throw new ApplicationException("Error in LS_CRUD.CRUDBuilder! Primary Key property for the parameter model is not found.");
            else if (!vTBL)
                throw new ApplicationException("Error in LS_CRUD.CRUDBuilder! Table Name property for the parameter model is not found.");

            vRead.Remove(vRead.Length - 1, 1);//Removing last comma
            //Adding FROM keyword
            vRead.Append(" FROM ");
            if (vPK)
                vWhere.Remove(vWhere.Length - 5, 5);//Removing last ' AND ' from where clause

            //Adding select column names
            pReadQuery.Append(vRead.ToString());
            //Adding table name
            pReadQuery.Append(vTableName);
            //If there is primary key and pAll is false then load by PK otherwise load all data...no where clause..
            if (vPK == true && pAll == false)
                pReadQuery.Append(vWhere.ToString());//Adding where clause

            return pReadQuery;
        }
        internal static StringBuilder BuildCREATEQuery(Hashtable pHT, ArrayList pExcludeList)
        {
            //Start to create the query
            StringBuilder vCreateQuery = new StringBuilder("INSERT INTO ");
            //This will add the Column names
            StringBuilder vInsert = new StringBuilder("(");
            //This will add the Column values
            StringBuilder vValues = new StringBuilder("VALUES(");
            string vTableName = "";
            string vColumnName = "";
            bool vTBL = false;


            //PropertyInfo[] vPI = pModel.GetType().GetProperties();

            //foreach (PropertyInfo pi in vPI)
            foreach (DictionaryEntry de in pHT)
            {
                vColumnName = de.Key.ToString();
                //For INSERT query it is not necessary to build null or empty values in the INSERT SQL. 
                //Items in the exclude list need required in the SQL
                if (de.Value == null)
                    continue;
                if (de.Value.ToString() == "")
                    continue;
                if (pExcludeList != null)
                    if (pExcludeList.Contains(vColumnName))
                        continue;

                if (CheckFilteredColumn(vColumnName))
                {
                    if (vColumnName.Contains("_PK"))
                        vColumnName = CRUDHelper.RemoveSuffix(vColumnName, "_PK");
                    else if (vColumnName.Contains("_FK"))
                        vColumnName = CRUDHelper.RemoveSuffix(vColumnName, "_FK");

                    //Adding column name with comma
                    vInsert.Append(vColumnName);
                    vInsert.Append(",");
                    //Adding column values with comma
                    string vColumnValue = CRUDHelper.GetFormatedColumnValue(de.Value);
                    if (vColumnName.ToUpper() == "ACTIONDATE")
                        vValues.Append("getdate()");
                    else
                        vValues.Append(vColumnValue);

                    //vValues.Append(CRUDHelper.GetFormatedColumnValue(pi, pModel, ""));
                    vValues.Append(",");
                }
                else if (de.Key.ToString().Contains("_TBL"))//Property with _TBL contains table name
                {
                    vTBL = true;
                    //Getting table name
                    vTableName = de.Value.ToString();
                }
            }

            vInsert.Remove(vInsert.Length - 1, 1);//Removing last comma
            vValues.Remove(vValues.Length - 1, 1);//Removing last comma

            vInsert.Append(")");//Adding ) after last column name            
            vValues.Append(")");//Adding ) after last column values
            //Adding table name
            vCreateQuery.Append(vTableName);
            //Adding column names
            vCreateQuery.Append(vInsert.ToString());
            //Adding column values
            vCreateQuery.Append(vValues.ToString());

            if (!vTBL)
                throw new ApplicationException("Error in LS_CRUD.CRUDBuilder.CREATEQuery(...)! Table Name property for the parameter model is not found.");
            return vCreateQuery;
        }
        internal static StringBuilder BuildUPDATEQuery(Hashtable pHT, ArrayList pExcludeList)
        {
            //test code starts here
            //Start to create the query
            StringBuilder vUpdateQuery = new StringBuilder("UPDATE ");
            //This will add the Column names
            StringBuilder vUpdate = new StringBuilder(" SET ");
            //This will add the Column values
            StringBuilder vWhere = new StringBuilder(" WHERE ");
            string vTableName = "";
            string vColumnName = "";
            bool vPK = false;
            bool vTBL = false;

            foreach (DictionaryEntry de in pHT)
            {
                vColumnName = de.Key.ToString();
                //Items in the exclude list need required in the SQL
                if (pExcludeList != null)
                    if (pExcludeList.Contains(vColumnName))
                        continue;
                if (CRUDHelper.CheckFilteredColumn(vColumnName) && !vColumnName.Contains("_PK"))
                {
                    if (vColumnName.Contains("_FK"))
                        vColumnName = CRUDHelper.RemoveSuffix(vColumnName, "_FK");

                    //Adding column name followed by '= column values'
                    vUpdate.Append(vColumnName);
                    vUpdate.Append(" = ");
                    if (vColumnName.ToUpper() == "ACTIONDATE")
                        vUpdate.Append("Getdate()");
                    else
                        vUpdate.Append(CRUDHelper.GetFormatedColumnValue(de.Value));

                    vUpdate.Append(",");

                    if (vColumnName.Contains("CompanyCode"))
                    {
                        //Generating where clause with companycode consulting with Anam vi
                        vWhere.Append("CompanyCode");
                        vWhere.Append(" = ");

                        vWhere.Append(CRUDHelper.GetFormatedColumnValue(de.Value));
                        vWhere.Append(" AND ");
                    }
                }
                else if (vColumnName.Contains("_TBL"))//Property with _TBL contains table name
                {
                    vTBL = true;
                    //Getting table name
                    vTableName = de.Value.ToString();
                }
                else if (vColumnName.Contains("_PK"))//Property with _PK contains primary column name
                {
                    vPK = true;//Setting true to ensure that where clause will be applied 
                    //Removing _PK from the property name. 
                    vColumnName = CRUDHelper.RemoveSuffix(vColumnName, "_PK");
                    //Generating where clause
                    vWhere.Append(vColumnName);
                    vWhere.Append(" = ");

                    vWhere.Append(CRUDHelper.GetFormatedColumnValue(de.Value));
                    vWhere.Append(" AND ");
                }

            }//End of foreach (PropertyInfo pi in vPI)

            vUpdate.Remove(vUpdate.Length - 1, 1);//Removing last comma
            if (vPK)
                vWhere.Remove(vWhere.Length - 5, 5);//Removing last ' AND ' from where clause

            //Adding table name
            vUpdateQuery.Append(vTableName);
            //Adding update values
            vUpdateQuery.Append(vUpdate.ToString());
            if (vPK)
                //Adding where clause
                vUpdateQuery.Append(vWhere.ToString());

            if (!vPK)
                throw new ApplicationException("Error in LS_CRUD.CRUDBuilder.UPDATEQuery(...)! Primary Key property for the parameter model is not found.");
            else if (!vTBL)
                throw new ApplicationException("Error in LS_CRUD.CRUDBuilder.UPDATEQuery(...)! Table Name property for the parameter model is not found.");
            else
                return vUpdateQuery;
        }
        internal static StringBuilder BuildDELETEQuery(Hashtable pHT, bool pMarkDEL, Hashtable pMarkDELColNameValues)
        {
            StringBuilder vDeleteQuery = new StringBuilder();
            //This will add the Column values
            StringBuilder vWhere = new StringBuilder(" WHERE ");
            string vTableName = "";
            string vColumnName = "";

            bool vPK = false;
            bool vTBL = false;

            foreach (DictionaryEntry de in pHT)
            {
                vColumnName = de.Key.ToString();
                if (vColumnName.Contains("_TBL"))//Property with _TBL contains table name
                {
                    vTBL = true;
                    //Getting table name
                    vTableName = de.Value.ToString();
                }
                else if (vColumnName.Contains("_PK"))//Property with _PK contains primary column name
                {
                    vPK = true;//Setting true to ensure that where clause will be applied 
                    //Removing _PK from the property name. 
                    vColumnName = CRUDHelper.RemoveSuffix(vColumnName, "_PK");
                    //Generating where clause
                    vWhere.Append(vColumnName);
                    vWhere.Append(" = ");

                    vWhere.Append(CRUDHelper.GetFormatedColumnValue(de.Value));
                    vWhere.Append(" AND ");
                }
            }//End of foreach (PropertyInfo pi in vPI)

            if (vPK)
                vWhere.Remove(vWhere.Length - 5, 5);//Removing last ' AND ' from where clause           

            if (!pMarkDEL)//pMarkDEL is false ie. DELETE FROM DB
            {
                vDeleteQuery.Append("DELETE FROM ");//Start to create the query
                //Adding table name
                vDeleteQuery.Append(vTableName);
            }
            else// Dont Delete from DB, rather mark delete
            {
                vDeleteQuery.Append("UPDATE  ");
                vDeleteQuery.Append(vTableName);//Adding table name
                vDeleteQuery.Append(" SET ");
                foreach (DictionaryEntry de in pMarkDELColNameValues)
                {
                    if (de.Key.ToString().ToUpper() == "ACTIONDATE")
                    {
                        vDeleteQuery.Append(de.Key.ToString());
                        vDeleteQuery.Append(" = ");
                        vDeleteQuery.Append("getdate()");
                        vDeleteQuery.Append(" ,");
                    }
                    else
                    {
                        vDeleteQuery.Append(de.Key.ToString());
                        vDeleteQuery.Append(" = '");
                        vDeleteQuery.Append(de.Value.ToString());
                        vDeleteQuery.Append("' ,");
                    }
                }
                vDeleteQuery.Remove(vDeleteQuery.Length - 1, 1);//Removing last comma
            }

            if (vPK)//If there is PK then need to add where clause
                vDeleteQuery.Append(vWhere.ToString());//Adding where clause

            if (!vPK)
                throw new ApplicationException("Error in LS_CRUD.CRUDBuilder.DELETEQuery(...)! Primary Key property for the parameter model is not found.");
            if (!vTBL)
                throw new ApplicationException("Error in LS_CRUD.CRUDBuilder.DELETEQuery(...)! Table Name property for the parameter model is not found.");
            return vDeleteQuery;
        }
    }
}

