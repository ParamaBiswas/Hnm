using PMSModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSInterface
{
    public interface ITermsConditionRepository
    {
        string SaveTermsAndCondition(List<TermsAndCondition> objTermsAndCondition);
        List<TermsAndCondition> GetTermsAndCondition();
    }
}
