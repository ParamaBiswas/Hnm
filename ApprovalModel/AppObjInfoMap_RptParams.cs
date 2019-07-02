using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApprovalModel
{
    [Serializable()]
    public class AppObjInfoMap_RptParams:BaseModel
    {
        public string TableName_TBL { get; set; }

        public string ObjectAppInfoMapRptParamCode_PK { get; set; }
        public string ObjAppInfoMapRptCode_FK { get; set; }

        public string RptParameterName { get; set; }
        public string DBFieldForParameterValue { get; set; }

        public string DBFieldValue_VW { get; set; }
        public bool IsEdit_VW { set; get; }

    }
}
