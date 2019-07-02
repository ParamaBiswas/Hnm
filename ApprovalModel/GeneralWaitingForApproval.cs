using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApprovalModel
{
    public class GeneralWaitingForApproval: BaseModel
    {
        #region properties

        public string TableName_TBL { get; set; }
        public string CompanyName_VW { get; set; }
        public string ApprovalWaitingCode { get; set; }
        public string ModuleObjMapCode { get; set; }
        public int AppModuleObjID { get; set; }
        public string AppLvDefinitionDetCode { get; set; }
        public string ObjPKValue { get; set; }
        public int AppProcessBatch { get; set; }
        public int AppProcessSerial { get; set; }
        public int AppLevel { get; set; }
        public int ApproverSLNo { get; set; }
        public string AppUserCode { get; set; }
        public int IsFinalLevel { get; set; }
        public int ResponseStatus { get; set; }
        public string ResponseRemarks { get; set; }

        public int NoOfObjectToApprove_VW { get; set; }
        public int NoOfObjectToUnApprove_VW { get; set; } //added by asif at 31.03.2016
        public string AppModuleObjName_VW { get; set; }
        public string DashBoardCaption_VW { get; set; }
        public int ApprovalStatus_VW { get; set; }
        #endregion 
    }
}
