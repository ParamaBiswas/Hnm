using LS.General.ModelBiz;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CommonInterface
{
    public interface IGeneralCodeFileLevel
    {
        List<GeneralCodeFileLevel> GetGeneralCodeFileLevelByFileType(Int32 fileTypeCode_PK, Int32 companyCode_PK);
        string SaveGeneralCodeFileLevel(GeneralCodeFileLevel objGeneralCodeFileLevel);
    }
}
