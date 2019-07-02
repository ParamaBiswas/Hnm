using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSModel
{
    public class SupplierInvoiceLine:BaseModel
    {
        public string TableName_TBL { get; set; }
        public decimal Quantity { get; set; }
        public string ProductCode { get; set; }
        public decimal VATRate { get; set; }
        public int QuantityMoUCode { get; set; }
        public int DepotCode { get; set; }
        public decimal OtherDiscountAmount { get; set; }
        public string InvoiceCode { get; set; }
        public decimal ReturnQuantity { get; set; }
        public decimal SubtotalAmount { get; set; }
        public decimal VATAmount { get; set; }
        public int BonusQuantity { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Rate { get; set; }
        public string InvoiceLineCode_PK { get; set; }
        public string ReceiveCode { get; set; }
    }
}
