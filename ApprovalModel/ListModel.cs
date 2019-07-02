using System;
using System.Collections.Generic;
using System.Text;

namespace ApprovalModel
{
    public class ListModel
    {
        public List<AppModuleObjectMapping> objIAppModuleObjectMappingList { get; set; }
        public List<AppLevelDefDetAppType> appLevelDefDet { get; set; }
        public List<GeneralWaitingForApproval> objIGeneralWaitingForApprovalList { get; set; }
    }
}
