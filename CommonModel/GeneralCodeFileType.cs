using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;



namespace LS.General.ModelBiz
{
     public class GeneralCodeFileType : BaseModel
    {
        public string TableName_TBL { get; set; }
        public int FileTypeCode_PK { get; set; }
        public string FileTypeName { get; set; }
        public string ModuleCode { get; set; }
        public bool IsHidden { get; set; }
        public int CountLevel_VW { get; set; }
        public int ErrorMessage_VW { get; set; }
        
        
    }
}