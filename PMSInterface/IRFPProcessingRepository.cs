using PMSModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSInterface
{
    public interface IRFPProcessingRepository
    {
        string SaveRFPProcessing(RFProcessing objRFProcessing);
        List<RFProcessing> GetRFProcessing();
        RFProcessing GetRFProcessingByCode(string rFProcessCode);
    }
}
