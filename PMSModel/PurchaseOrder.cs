using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSModel
{
    public class PurchaseOrder:BaseModel
    {
        public string TableName_TBL { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string QuotationCode { get; set; }
        public string SupplierCode { get; set; }
        public string Remarks { get; set; }
        public string OrderDate { get; set; }
        public string RequisitionCode { get; set; }
        public int IsAcceptedBySupplier { get; set; }
        public decimal TotatlQuotationScore { get; set; }
        public int ApprovalAction { get; set; }
        public int IsApproved { get; set; }
        public string PurchaseOrderCode_PK { get; set; }

        public string SupplierID_VW { get; set; }
        public string SupplierName_VW { get; set; }
        public string SupplierAddress_VW { get; set; }
        public string RequisitionID_VW { get; set; }
        public string RequisitionTitle_VW { get; set; }

        public List<QuotationItem> QuotationItemList_VW { get; set; }
    }
}
