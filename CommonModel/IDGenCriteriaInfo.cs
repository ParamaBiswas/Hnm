using System;
using System.Collections.Generic;
using System.Text;

namespace CommonModel
{
    public class IDGenCriteriaInfo :BaseModel
    {
        public string TableNm_TBL { get; set; }
        public string CriteriaCode_PK { get; set; }
        public string CriteriaID { get; set; }
        public string CriteriaName { get; set; }
        public string CriteriaConditionText { get; set; }
        public List<IDGenCriteriaCondition> IDGenCriteriaConditionList_VW { get; set; }

    }
}
