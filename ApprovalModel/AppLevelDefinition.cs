using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApprovalModel
{
    public class AppLevelDefinition
    {
        public string TableName_TBL { get; set; }
        public int AppLevelType { get; set; }
        public string AppLvDefinitionCode_PK { get; set; }
        public int ActiveYN_VW { get; set; }
        public string ModuleObjMapCode_FK { get; set; }
        public string ModuleObjId_VW { get; set; }
        public string ModuleName_VW { get; set; }
        public string BusinessObjectName_VW { get; set; }
        public List<StaticItem> AppLevelTypeList_VW
        {
            get;
        }
        public List<AppLevelDefinitionDet> objIAppLevelDefinitionDetList_VW { get; set; }
    }
}
