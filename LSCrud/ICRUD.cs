using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LSCrud
{
    public interface ICRUD
    {
        string CREATEQuery(object pModel);
        string CREATEQuery(object pModel, ArrayList pExcludeList);
        string CREATEQuery(Hashtable pHT);
        string READQuery(object pModel, Hashtable pFilteredColumns);
        string READQuery(Hashtable pHT, Hashtable pFilteredColumns);
        string READQuery(object pModel, bool pAll);
        string READQuery(Hashtable pHT, bool pAll);
        string READQuery(object pModel, bool pAll, bool pOrderBy, string[] pOrderColumns, bool pASC);
        string READQuery(Hashtable pHT, bool pAll, bool pOrderBy, string[] pOrderColumns, bool pASC);
        string UPDATEQuery(object pModel);
        string UPDATEQuery(Hashtable pHT);
        string UPDATEQuery(object pModel, Hashtable pFilteredColumns);
        string UPDATEQuery(Hashtable pHT, Hashtable pFilteredColumns);
        string UPDATEQuery(object pModel, ArrayList pExcludeList);

        string DELETEQuery(object pModel, bool pMarkDEL, Hashtable pMarkDELColNameValues);
        string DELETEQuery(Hashtable pHT, bool pMarkDEL, Hashtable pMarkDELColNameValues);
        string READQueryMaxValue(string pTableName, string pMaxOn, Hashtable pSelectCriteria);
    }
}
