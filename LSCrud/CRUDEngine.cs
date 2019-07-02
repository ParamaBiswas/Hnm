using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LSCrud
{
    public class CRUDEngine : ICRUD
    {
        public string CREATEQuery(object pModel)
        {
            Hashtable vHT = CRUDHelper.GetColumnValueCol(pModel);
            return CRUDHelper.BuildCREATEQuery(vHT, null).ToString();
        }
        public string CREATEQuery(object pModel, ArrayList pExcludeList)
        {
            Hashtable vHT = CRUDHelper.GetColumnValueCol(pModel);
            return CRUDHelper.BuildCREATEQuery(vHT, pExcludeList).ToString();
        }
        public string CREATEQuery(Hashtable pHT)
        {
            if (pHT == null)
                throw new ArgumentNullException("HashTable pHT contains no value.");
            else if (pHT.Count < 0)
                throw new ArgumentNullException("HashTable pHT contains 0 elements.");
            return CRUDHelper.BuildCREATEQuery(pHT, null).ToString();
        }
        public string READQuery(object pModel, Hashtable pFilteredColumns)
        {
            //Start to create the query
            StringBuilder vReadQuery = new StringBuilder("SELECT ");
            Hashtable vHT = CRUDHelper.GetColumnValueCol(pModel);
            //Build the READ query with PK
            vReadQuery = CRUDHelper.BuildREADQuery(vHT, vReadQuery, false);
            //Adding additional filter
            foreach (DictionaryEntry de in pFilteredColumns)
            {
                vReadQuery.Append(" AND ");
                vReadQuery.Append(de.Key.ToString());
                vReadQuery.Append(" = ");
                vReadQuery.Append(CRUDHelper.GetFormatedColumnValue(de.Value));
            }
            return vReadQuery.ToString();
        }
        public string READQuery(Hashtable pHT, Hashtable pFilteredColumns)
        {
            //Start to create the query
            StringBuilder vReadQuery = new StringBuilder("SELECT ");
            //Build the READ query with PK
            vReadQuery = CRUDHelper.BuildREADQuery(pHT, vReadQuery, false);
            //Adding additional filter
            foreach (DictionaryEntry de in pFilteredColumns)
            {
                vReadQuery.Append(" AND ");
                vReadQuery.Append(de.Key.ToString());
                vReadQuery.Append(" = ");
                vReadQuery.Append(CRUDHelper.GetFormatedColumnValue(de.Value));
            }
            return vReadQuery.ToString();
        }
        public string READQuery(object pModel, bool pAll)
        {
            //Start to create the query
            StringBuilder vReadQuery = new StringBuilder("SELECT ");
            Hashtable vHT = CRUDHelper.GetColumnValueCol(pModel);
            vReadQuery = CRUDHelper.BuildREADQuery(vHT, vReadQuery, pAll);

            return vReadQuery.ToString();
        }
        public string READQuery(Hashtable pHT, bool pAll)
        {
            //Start to create the query
            StringBuilder vReadQuery = new StringBuilder("SELECT ");
            vReadQuery = CRUDHelper.BuildREADQuery(pHT, vReadQuery, pAll);

            return vReadQuery.ToString();
        }
        public string READQuery(object pModel, bool pAll, bool pOrderBy, string[] pOrderColumns, bool pASC)
        {
            //Start to create the query
            StringBuilder vReadQuery = new StringBuilder("SELECT ");
            Hashtable vHT = CRUDHelper.GetColumnValueCol(pModel);
            vReadQuery = CRUDHelper.BuildREADQuery(vHT, vReadQuery, pAll);

            if (pOrderBy)//Adding Order By
            {
                vReadQuery.Append(CRUDHelper.CreateOrderBy(pOrderColumns));
                if (pASC)
                    vReadQuery.Append(" ASC ");
                else
                    vReadQuery.Append(" DESC ");
            }

            return vReadQuery.ToString();
        }
        public string READQuery(Hashtable pHT, bool pAll, bool pOrderBy, string[] pOrderColumns, bool pASC)
        {
            //Start to create the query
            StringBuilder vReadQuery = new StringBuilder("SELECT ");
            vReadQuery = CRUDHelper.BuildREADQuery(pHT, vReadQuery, pAll);
            if (pOrderBy)//Adding Order By
            {
                vReadQuery.Append(CRUDHelper.CreateOrderBy(pOrderColumns));
                if (pASC)
                    vReadQuery.Append(" ASC ");
                else
                    vReadQuery.Append(" DESC ");
            }
            return vReadQuery.ToString();
        }
        public string UPDATEQuery(object pModel)
        {
            //Taking the model object in hashtable
            Hashtable vHT = CRUDHelper.GetColumnValueCol(pModel);
            StringBuilder vUpdateQuery = new StringBuilder();
            vUpdateQuery = CRUDHelper.BuildUPDATEQuery(vHT, null);
            return vUpdateQuery.ToString();
        }
        public string UPDATEQuery(object pModel, ArrayList pExcludeList)
        {
            //Taking the model object in hashtable
            Hashtable vHT = CRUDHelper.GetColumnValueCol(pModel);
            StringBuilder vUpdateQuery = new StringBuilder();
            vUpdateQuery = CRUDHelper.BuildUPDATEQuery(vHT, pExcludeList);
            return vUpdateQuery.ToString();
        }
        public string UPDATEQuery(Hashtable pHT)
        {
            StringBuilder vUpdateQuery = new StringBuilder();
            vUpdateQuery = CRUDHelper.BuildUPDATEQuery(pHT, null);
            return vUpdateQuery.ToString();
        }
        public string UPDATEQuery(object pModel, Hashtable pFilteredColumns)
        {
            //Taking the model object in hashtable
            Hashtable vHT = CRUDHelper.GetColumnValueCol(pModel);
            StringBuilder vUpdateQuery = new StringBuilder();
            vUpdateQuery = CRUDHelper.BuildUPDATEQuery(vHT, null);

            //Adding additional filter
            foreach (DictionaryEntry de in pFilteredColumns)
            {
                vUpdateQuery.Append(" AND ");
                vUpdateQuery.Append(de.Key.ToString());
                vUpdateQuery.Append(" = ");
                vUpdateQuery.Append(CRUDHelper.GetFormatedColumnValue(de.Value));
            }
            return vUpdateQuery.ToString();
        }
        public string UPDATEQuery(object pModel, Hashtable pFilteredColumns, ArrayList pExcludeList)
        {
            //Taking the model object in hashtable
            Hashtable vHT = CRUDHelper.GetColumnValueCol(pModel);
            StringBuilder vUpdateQuery = new StringBuilder();
            vUpdateQuery = CRUDHelper.BuildUPDATEQuery(vHT, pExcludeList);

            //Adding additional filter
            foreach (DictionaryEntry de in pFilteredColumns)
            {
                vUpdateQuery.Append(" AND ");
                vUpdateQuery.Append(de.Key.ToString());
                vUpdateQuery.Append(" = ");
                vUpdateQuery.Append(CRUDHelper.GetFormatedColumnValue(de.Value));
            }
            return vUpdateQuery.ToString();
        }
        public string UPDATEQuery(Hashtable pHT, Hashtable pFilteredColumns)
        {
            StringBuilder vUpdateQuery = new StringBuilder();
            vUpdateQuery = CRUDHelper.BuildUPDATEQuery(pHT, null);

            //Adding additional filter
            foreach (DictionaryEntry de in pFilteredColumns)
            {
                vUpdateQuery.Append(" AND ");
                vUpdateQuery.Append(de.Key.ToString());
                vUpdateQuery.Append(" = ");
                vUpdateQuery.Append(CRUDHelper.GetFormatedColumnValue(de.Value));
            }
            return vUpdateQuery.ToString();
        }
        public string DELETEQuery(object pModel, bool pMarkDEL, Hashtable pMarkDELColNameValues)
        {
            Hashtable vHT = CRUDHelper.GetColumnValueCol(pModel);
            StringBuilder vDeleteQuery = new StringBuilder();
            vDeleteQuery = CRUDHelper.BuildDELETEQuery(vHT, pMarkDEL, pMarkDELColNameValues);
            return vDeleteQuery.ToString();
        }
        public string DELETEQuery(Hashtable pHT, bool pMarkDEL, Hashtable pMarkDELColNameValues)
        {
            StringBuilder vDeleteQuery = new StringBuilder();
            vDeleteQuery = CRUDHelper.BuildDELETEQuery(pHT, pMarkDEL, pMarkDELColNameValues);
            return vDeleteQuery.ToString();
        }
        public string READQueryMaxValue(string pTableName, string pMaxOn, Hashtable pSelectCriteria)
        {
            StringBuilder vSB = new StringBuilder("SELECT MAX(CAST( ");
            //Adding column name
            vSB.Append(pMaxOn);
            vSB.Append(" AS int)) AS ");
            vSB.Append(pMaxOn);
            vSB.Append(" FROM ");
            //Adding table name
            vSB.Append(pTableName);

            if (pSelectCriteria != null)
            {
                //Adding WHERE clause
                vSB.Append(" WHERE ");
                foreach (DictionaryEntry de in pSelectCriteria)
                {
                    vSB.Append(de.Key.ToString());
                    vSB.Append(" = '");
                    vSB.Append(de.Value.ToString());
                    vSB.Append("' AND ");
                }
                //Removing last AND
                vSB.Remove(vSB.Length - 5, 5);
            }
            return vSB.ToString();
        }

    }
}
