using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSModel
{
    public class PurchaseRequisitionItem :BaseModel
    {
        public string TableName_TBL { get; set; }
        public string ProductCode { get; set; }
        public string ProductID_VW { get; set; }
        public string ProductName_VW { get; set; }
        public string RequisitionDetCode_PK { get; set; }
        public string RequisitionCode { get; set; }
        public decimal Quantity { get; set; }
        public int? ProductType { get; set; }
        public string ProductCatagory_VW { get; set; }
        public int QunatityMOUCode { get; set; }
        public string QunatityMOU_VW { get; set; }

        public List<PurchaseReqItemSpecification> PurchaseReqItemSpecificationList_VW { get; set; }
    }
}
