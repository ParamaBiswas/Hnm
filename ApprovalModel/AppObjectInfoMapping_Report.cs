using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApprovalModel
{
    [Serializable()]
    public class AppObjectInfoMapping_Report: BaseModel
    {
        public string TableName_TBL { get; set; }

        public string ObjAppInfoMapRptCode_PK { get; set; }
        public string ModuleObjMapCode { get; set; }
        public string ReportCode { get; set; }

        public string ReportName_VW { get; set; }
        public string ReportId_VW { get; set; }

        public List<AppObjInfoMap_RptParams> AppObjInfoMap_RptParamsList_VW { get; set; }
    }
}
