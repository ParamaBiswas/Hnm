using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApprovalModel
{
    [Serializable()]
    public class AppObjInfoMap_Grid : BaseModel
    {
        public string TableName_TBL { get; set; }

        public string ObjAppInfoMapGridCode_PK { get; set; }
        public string ModuleObjMapCode_FK { get; set; }
        public string DBFieldName { get; set; }
        public string DBFunctionMapCode_FK { get; set; }
        public string GridColumnHeaderText { get; set; }
        public int GridColumnLength { get; set; }
        public int IsOrderBy { get; set; }
        public int ColumnSortOrder { get; set; }
        public int IsViewInMessage { get; set; }
        public string DBFieldValue_VW { get; set; }
        public string DBFunctionName_VW { get; set; }
    }
}
