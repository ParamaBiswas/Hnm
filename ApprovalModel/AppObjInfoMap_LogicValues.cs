using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApprovalModel
{
    [Serializable()]
    public class AppObjInfoMap_LogicValues:BaseModel
    {
        public string TableName_TBL { get; set; }

        public string ObjAppInfoMapLogicValueCode_PK { set; get; }
        public string ObjAppInfoMappLogicCode_FK { set; get; }
        public int LogicSerialNo { set; get; }
        public string OptionValue { set; get; }
        public string OptionText { set; get; }
        public int OptionFor { set; get; }

        public string OptionFor_VW { set; get; }

        public int SL_VW { set; get; }
        public string ModuleObjMapCode_VW { set; get; }
        public bool IsEdit_VW { set; get; }
        public string DBFieldName_VW { set; get; }

    }
}
