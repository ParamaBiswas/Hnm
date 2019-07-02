using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSModel
{
    public class PurchaseRequisitionCondition : BaseModel
    {
        public string TableName_TBL { get; set; }
        public string Remarks { get; set; }
        public string ConditionCode_PK { get; set; }
        public string Condition_VW { get; set; }
        public int ConSLNo { get; set; }
        public string RequisitionCode { get; set; }
    }
}
