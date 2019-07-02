using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSModel
{
    public class QuotationCondition : BaseModel
    {
        public string TableName_TBL { get; set; }
        public int ConSLNo { get; set; }
        public string QuotationCode { get; set; }
        public string ConditionCode_PK { get; set; }
        public string PurchaseOrderCode { get; set; }
        public string Remarks { get; set; }
        public int IsSatisfied { get; set; }
        public decimal ConditionValue { get; set; }
        public decimal AchivedValue { get; set; }
        public string Condition_VW { get; set; }
    }
}
