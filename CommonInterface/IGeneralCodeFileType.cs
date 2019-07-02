using CommonModel;
using LS.General.ModelBiz;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CommonInterface
{
    public interface IGeneralCodeFileType
    {
        List<GeneralCodeFileType> GetGeneralCodeFileTypeAll(string pModuleCode, Int32 COMPANY_CODE);
        string SaveGeneralCodeFileType(GeneralCodeFileType objGeneralCodeFileType);
        GeneralCodeFileType GetGeneralCodeFileTypeByKey(Int32 FileTypeCode, Int32 COMPANY_CODE);
        List<StaticItem> GetModuleList();

    }
}
