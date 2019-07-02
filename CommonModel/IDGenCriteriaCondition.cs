using System;
using System.Collections.Generic;
using System.Text;

namespace CommonModel
{
   public class IDGenCriteriaCondition : BaseModel
    {
        public string TableNm_TBL { get; set; }
        public string ConditionCode_PK { get; set; }
        public int SortOrder { get; set; }
        public string ConditionText { get; set; }
        public string ConditionType { get; set; }
        public string ConditionTypeText_VW { get; set; }
        public int ConditionValueLength { get; set; }
        public string ConditionValue { get; set; }
        public string CriteriaCode_FK { get; set; }
        public string AutoIncrementCriteria { get; set; }
        public string PaddingText { get; set; }
    }
}
