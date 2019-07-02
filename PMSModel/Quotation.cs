using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSModel
{
    public class Quotation:BaseModel
    {
        public string TableName_TBL { get; set; }
        public string QuotationNo { get; set; }
        public string QuotationDate { get; set; }
        public string Remarks { get; set; }
        public string RFProcessCode { get; set; }
        public string SupplierCode { get; set; }
        public string PurchaseOrderCode { get; set; }
        public int IsFinalSelection { get; set; }
        public decimal TotatlQuotationScore { get; set; }
        public int ApprovalAction { get; set; }
        public int IsApproved { get; set; }
        public string QuotationCode_PK { get; set; }
        public string SupplierID_VW { get; set; }
        public string SupplierName_VW { get; set; }
        public string RequisitionCode_VW { get; set; }

        public List<QuotationCondition> QuotationConditionList_VW { get; set; }
        public List<QuotationItem> QuotationItemList_VW { get; set; }

    }
}
