using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSModel
{
    public class PurchaseOrderDet : BaseModel
    {
        public string TableName_TBL { get; set; }
        public string ProductCode { get; set; }
        public string PurchaseOrderDetCode_PK { get; set; }
        public string PurchaseOrderCode_FK { get; set; }
        public decimal TotalVAT { get; set; }
        public decimal Quantity { get; set; }
        public int ProductType { get; set; }
        public int QunatityMOUCode { get; set; }
        public decimal VATRate { get; set; }
        public decimal Rate { get; set; }
    }
}
