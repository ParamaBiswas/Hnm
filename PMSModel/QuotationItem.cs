using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSModel
{
    public class QuotationItem : BaseModel
    {
        public string TableName_TBL { get; set; }
        public string QuotationCode { get; set; }
        public int QunatityMOUCode { get; set; }
        public string RequisitionCode { get; set; }
        public int ProductType { get; set; }
        public decimal Quantity { get; set; }
        public string RequisitionDetCode { get; set; }
        public decimal VATRate { get; set; }
        public string ProductCode { get; set; }
        public string ProductID_VW { get; set; }
        public string ProductName_VW { get; set; }
        public string ProductCatagory_NM { get; set; }
        public string QunatityMOU_VW { get; set; }
        public decimal TotalVAT { get; set; }
        public decimal Rate { get; set; }
        public string QuotationDetCode_PK { get; set; }

        public List<QuotationItemSpecification> QuotationItemSpecificationList_VW { get; set; }
    }
}
