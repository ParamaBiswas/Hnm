using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApprovalModel
{
    public class AppModuleObjectMapping : BaseModel
    {
        
        public string TableName_TBL { get; set; }
        public string ModuleObjMapCode_PK { get; set; }
        public int ModuleCode { get; set; }
        public string ModuleObjName { get; set; }
        public int ModuleObjId { get; set; }
        public string ModuleName_VW { get; set; }
        public string ObjMapGridCode_VW { set; get; }
        public int SL_VW { set; get; }
        public bool IsEdit_VW { set; get; }
    }
}
