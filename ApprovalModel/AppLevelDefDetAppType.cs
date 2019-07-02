using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;


namespace ApprovalModel
{
    public class AppLevelDefDetAppType:BaseModel
    {
        #region Properties

        public string TableName_TBL { get; set; }
        public string AppLvDefDetAppTypeCode_PK { set; get; }
        public string AppLvDefinitionDetCode_FK { set; get; }
        public int? ApproverLevelNo { set; get; }
        public int? ApproverType { set; get; }
        public string ModuleName_VW { get; set; }
        public string BusinessObjectName_VW { get; set; }
        public string ModuleObjId_VW { get; set; }
        public int? SLNo_VW { set; get; }
        public int? NoOfAppLevel_VW { set; get; }

        // public int? AppLevelType_VW { set; get; }

        public string AppLevelType_VW { set; get; }
       // public List<StaticItem> ApproverLevelNo_VW { set; get; }
        public string ModuleObjMapCode_VW { set; get; }
        public string IS_NEW_Test_VW { set; get; }
        public string IsNew_Flag_VW { set; get; }
        public List<AppLevelDetAppTypeApprover> objAppLevelDetAppTypeApproverList_VW { set; get; }



        #endregion
    }
}
