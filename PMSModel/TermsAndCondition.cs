using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSModel
{
    public class TermsAndCondition :BaseModel
    {
        public string TableName_TBL { get; set; }
        public decimal ConditionValue { get; set; }
        public string ConditionCode_PK { get; set; }
        public string Condition { get; set; }
        public int ValueType { get; set; }
        public string ValueTypeName_VW { get; set; }
    }
}
