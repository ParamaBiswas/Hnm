using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSModel
{
    public class PurchaseReqItemSpecification:BaseModel
    {
        public string TableName_TBL { get; set; }
        public string ProductSpecification { get; set; }
        public string SpecificationCode_PK { get; set; }
        public string RequisitionDetCode { get; set; }
    }
}
