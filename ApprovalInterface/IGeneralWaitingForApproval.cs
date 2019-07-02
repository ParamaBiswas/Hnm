using ApprovalModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApprovalInterface
{
    public interface IGeneralWaitingForApproval
    {
        List<GeneralWaitingForApproval> GetDashBoardData(string pUserId);
        AppObjectInfoMap GetAppObjectInfoMapByMapCode(string pModuleObjMapCode);
        List<AppObjectInfoMap> GetObjectListWithDataForApproval(string pModuleObjMapCode, AppObjectInfoMap objAppObjectInfoMap, int pApprovalStatus, string pFromDate, string pToDate, string pUserCode);
        string SaveApprovalData(List<GeneralWaitingForApproval> objIGeneralWaitingForApprovalList);

    }
}
