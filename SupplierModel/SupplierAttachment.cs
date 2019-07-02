using CommonModel;

namespace LS.LS.ModelBiz
{
    public class SupplierAttachment :BaseModel
       {
           public string TableName_TBL { get; set; }
           public string SupplierCode_FK { get; set; }
           public string AttachmentCode_PK { get; set; }
           public string CertificateNumber { get; set; }
           public string AttachmentName { get; set; }
           public string IssueDate { get; set; }
           public string ExpiryDate { get; set; }
           public string Remarks { get; set; }
           public string FileLocationPath { get; set; }


        }
}