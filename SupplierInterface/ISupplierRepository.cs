using LS.LS.ModelBiz;
using System.Data.SqlClient;

namespace SupplierInterface
{
    public interface ISupplierRepository
    {
        string SaveSupplier(SupplierInfo objsupplierInfo);
        SupplierInfo GetSupplierInfo(string suppliercode); 
        string ApproveActionUpdate(string pSupplierCode, int pApprovalAction, bool pIsApproved, SqlConnection objDbConnection, SqlTransaction objDbTransaction);
    }
}
