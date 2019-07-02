using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;



namespace LS.General.ModelBiz
{
     public class GeneralCodeFileLevel : BaseModel
    {
        public string TableNm_TBL { get; set; }
        public int LevelCode_PK { get; set; }
        public string LevelId { get; set; }
        public string LevelName { get; set; }
        public int FileTypeCode { get; set; }
        public string FileIdStartFrom { get; set; }
        public string ModuleCode { get; set; }
        
    }
}