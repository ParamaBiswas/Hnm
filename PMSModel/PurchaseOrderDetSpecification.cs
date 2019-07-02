using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSModel
{
    public class PurchaseOrderDetSpecification : BaseModel
    {
        public string TableName_TBL { get; set; }
        public string ProductSpecification { get; set; }
        public string QuotationDetCode { get; set; }
        public string PurchaseOrderDetCode_PK { get; set; }

    }
}
