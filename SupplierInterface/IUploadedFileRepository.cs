using SupplierModel;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SupplierInterface
{
    public interface IUploadedFileRepository
    {
        string SaveUploadedFiles(List<UploadedFile> objUploadedFileList);
        string ApproveActionUpdate(string pFileUploadedCode, int pApprovalAction, bool pIsApproved, SqlConnection objDbConnection, SqlTransaction objDbTransaction);
        List<UploadedFile> GetUploadedFileBySupplierCode(string SupplierCode);
    }
}
