using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApprovalModel
{
    [Serializable()]
    public class AppObjInfoMap_Logic:BaseModel
    {
        public string TableName_TBL { get; set; }

        public string ObjAppInfoMapLogicCode_PK { set; get; }
        public string ModuleObjMapCode_FK { set; get; }
        public int IsFieldPolicy { set; get; }
        public int FieldType1 { set; get; }
        public string DBFieldName1 { set; get; }
        public int FieldType2 { set; get; }
        public string DBFieldName2 { set; get; }
        public int FieldType3 { set; get; }
        public string DBFieldName3 { set; get; }
        // public int ActiveYN { set; get; }
        public List<StaticItem> DBFieldName1_VW { set; get; }
        public List<StaticItem> DBFieldName2_VW { set; get; }
        public List<StaticItem> DBFieldName3_VW { set; get; }
        public int SL_VW { set; get; }
        public bool IsEdit_VW { set; get; }

        public List<AppObjInfoMap_LogicValues> AppObjInfoMap_LogicValues_List_VW { set; get; }

    }
}
