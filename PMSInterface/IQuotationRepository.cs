using PMSModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSInterface
{
    public interface IQuotationRepository
    {
        string SaveQuotation(Quotation objQuotation);
        List<Quotation> GetQuotation();
        Quotation GetQuotationByCode(string QuotationCode);
        List<Quotation> GetQuotationByRFP(string pREFProcessCode);
        int CompareQuotation(string RFProcess);
    }
}
