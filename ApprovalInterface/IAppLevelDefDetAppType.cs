using ApprovalModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApprovalInterface
{
    public interface IAppLevelDefDetAppType
    {
        List<AppLevelDefDetAppType> GetApproverSelection(string appLvDefinitionDetCode_FK);
        string SaveApproverSelection(List<AppLevelDefDetAppType> objIAppLevelDefDetAppType_List);
        int IsTransactionFound(string pAppLvDefinitionDetCode);
    }
}
