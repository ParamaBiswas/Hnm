using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;


namespace ApprovalModel
{
    public class AppLevelDetAppTypeApprover: BaseModel
    {
        public string TableName_TBL { get; set; }
        public string AppLvDetAppTypeApprover_PK { set; get; }
        public string AppLvDefDetAppTypeCode_FK { set; get; }
        public string ApproverEmpCode { set; get; }
        public int DesignationType { set; get; }
        public int DesignationCode { set; get; }
        public int ApproverOfDept { set; get; }
        public int ApproverForDept { set; get; }
        public int ActiveYN { set; get; }
        public int LevelNo { set; get; }
        public string ApproverEmpName_VW { set; get; }
        public string Designation_VW { set; get; }
        public string ApproverLevelNo_VW { set; get; }
        public string ApproverType_VW { set; get; }
       // public string IS_NEW_Test_VW { set; get; }
        public int SL_VW { get; set; }
        public string ApproverOfDept_Name_VW { set; get; }
        public string ApproverForDept_Name_VW { set; get; }
        public string DesignationType_Name_VW { set; get; }
        public int ApproverTypeVal_VW { set; get; }
        //public AppLevelDefDetAppType objIAppLevelDefDetAppType_VW { set; get; }

        public GeneralWaitingForApproval objGeneralWaitingForApproval_VW { set; get; }

        public string ApprovalWaitingCode_VW { set; get; }

       // public List<AppLevelDetAppTypeApprover> objAppLevelDetAppTypeApprover_List_VW { set; get; }
    }
}
