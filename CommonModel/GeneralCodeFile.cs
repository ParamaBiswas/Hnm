using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;



namespace LS.General.ModelBiz
{
     public class GeneralCodeFile : BaseModel
     {
           public string TableName_TBL { get; set; }
           public string SortOrder { get; set; }
           public string FileName { get; set; }
           public string FileShortName { get; set; }
           public int FileCode_PK { get; set; }
           public string FileId { get; set; }
           public int IsActiveCodeFile { get; set; }
           public string ModuleCode { get; set; }
           public bool IsReadOnly { get; set; }
           public int LevelCode { get; set; }
           //public int FileTypeCode_FK { get; set; }
           public string ParentFileCode { get; set; }
           public int FileTypeCode { get; set; }
           public string LevelName { get; set; }

     }
}