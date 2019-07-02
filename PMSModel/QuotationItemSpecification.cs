using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSModel
{
    public class QuotationItemSpecification : BaseModel
    {
        public string TableName_TBL { get; set; }
        public string SpecificationCode { get; set; }
        public string QuotationDetCode_FK { get; set; }
        public string ProductCode { get; set; }
        public int IsSatisfied { get; set; }
        public string QuotationItemSpecificationCode_PK { get; set; }
        public string ProductSpecification_VW { get; set; }
    }
}
