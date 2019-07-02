using CommonModel;

namespace SupplierModel
{
    public class UploadedFile : BaseModel
    {
        public string TableName_TBL { get; set; }
        public string UploadedCode_PK { get; set; }
        public string SupplierCode_FK { get; set; }
        public string AttachmentFileCode { get; set; }
        public string AttachmentName { get; set; }
        public string UploadDate { get; set; }
        public string FileLocationPath { get; set; }
        public int IsApproved { get; set; }
        public int ApprovalAction { get; set; }
    }
}
