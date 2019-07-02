using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApprovalModel
{
    [Serializable()]
    public class AppObjectInfoMap : BaseModel
    {
        public string TableName_TBL { get; set; }

        public string ModuleObjMapCode_PK { get; set; }

        public int ModuleObjId { get; set; }
        public string DBObjName { get; set; }
        public string DashBoardCaption { get; set; }
        public string PKColumnName { get; set; }

        public int IsDynamicPage { get; set; }
        public int PageCode { get; set; }
        public int Active { set; get; }  

        public string ModuleObjName_VW { get; set; }
        public string PKColumnValue_VW { get; set; }
        public int ApprovalStatus_VW { get; set; }
        public int ApprovalLevel_VW { get; set; }
        public int IsFinalLevelApproval_VW { get; set; }
        public int ResponseStatus_VW { get; set; }
        public string Remarks_VW { get; set; }
        public string ApprovalWaitingCode_VW { get; set; }
        public int AppProcessBatch_VW { get; set; }

        public string FromDateForSearch_VW { get; set; }
        public string ToDateForSearch_VW { get; set; }
        public int ApprovalStatusForSearch_VW { get; set; }
        public bool IsForView_VW { get; set; }
        public string PreviousApprover_VW { get; set; }
        public List<AppObjInfoMap_Grid> AppObjInfoMap_GridList_VW { get; set; }
        public AppObjectInfoMapping_Report AppObjectInfoMapping_Report_VW { get; set; }
    }
}
