using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSModel
{
    public class SupplierInvoice:BaseModel
    {
        public string TableName_TBL { get; set; }
        public string InvoiceNo { get; set; }
        public decimal TotalVATAmount { get; set; }
        public int IsApproved { get; set; }
        public decimal InvoiceTotal { get; set; }
        public int DepotCode { get; set; }
        public string InvoiceDate { get; set; }
        public decimal GrandTotal { get; set; }
        public int ApprovalAction { get; set; }
        public decimal AdjustedAmount { get; set; }
        public int IsAdvance { get; set; }
        public int IsSettled { get; set; }
        public string SupplierCode { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal SpecialDiscount { get; set; }
        public string InvoiceCode_PK { get; set; }

        public List<SupplierInvoiceLine> SupplierInvoiceLine_VW { get; set; }
    }
}
