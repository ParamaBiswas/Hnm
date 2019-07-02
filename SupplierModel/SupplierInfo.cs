using CommonModel;
using System.Collections.Generic;



namespace LS.LS.ModelBiz
{
    public class SupplierInfo: BaseModel
     {
           public string TableName_TBL { get; set; }
           public string SupplierName { get; set; }
           public string SupplierCode_PK { get; set; }
           public string SupplierID { get; set; }
           public string ZipCode { get; set; }
           public string MobileNumber { get; set; }
           public string Email { get; set; }
           public string EnlistmentDate { get; set; }
           public string SupplierAddress { get; set; }
           public string AlternateEmail { get; set; }
           public string Fax { get; set; }
           public int IsApproved { get; set; }
           public int ApprovalAction { get; set; }

        public List<SupplierContact> SupplierContactList_VW { get; set; }
        public List<SupplierAttachment> SupplierAttachmentList_VW { get; set; }
        public List<SupplierBusiness> SupplierBusinessList_VW { get; set; }




    }
}