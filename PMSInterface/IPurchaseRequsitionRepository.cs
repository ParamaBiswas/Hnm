using PMSModel;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PMSInterface
{
    public interface IPurchaseRequsitionRepository
    {
        string SavePurchaseRequsition(PurchaseRequisition objPMS_PurchaseRequisition);
        List<PurchaseRequisition> GetRequisition(string requisitionDate);
        PurchaseRequisition GetRequisitionByCode(string requisitionCode);
        string ApproveActionUpdate(string prequisitionCode, int pApprovalAction, bool pIsApproved, SqlConnection objDbConnection, SqlTransaction objDbTransaction);
    }
}
