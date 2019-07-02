using LS.General.ModelBiz;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CommonInterface
{
    public interface IGeneralCodeFile
    {
        List<GeneralCodeFile> GetGeneralCodeFileByFileTypeNFileLevel(Int32 FileTypeCode, Int32 LevelCode, Int32 COMPANY_CODE);
        List<GeneralCodeFile> GetGeneralCodeFileByFileTypeNParentFile(Int32 pFileTypeCode, Int32 pParentFileCode, Int32 COMPANY_CODE);
        List<GeneralCodeFile> GetGeneralCodeFileByFileType(Int32 FileTypeCode, string pCompanyCode);
        string SaveGeneralCodeFile(GeneralCodeFile objGeneralCodeFile);

    }
}
