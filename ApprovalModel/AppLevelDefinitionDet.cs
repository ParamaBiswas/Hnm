using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApprovalModel
{
    public class AppLevelDefinitionDet
    {
        public string TableName_TBL { get; set; }
        public decimal? ValueFrom1 { get; set; }
        public decimal? ValueTo1 { get; set; }
        public int NoOfAppLevel { get; set; }
        public decimal? ValueFrom2 { get; set; }
        public decimal? ValueTo2 { get; set; }
        public string AppLvDefinitionDetCode_PK { get; set; }
        public decimal? ValueFrom3 { get; set; }
        public string AppLvDefinitionCode_FK { get; set; }
        public decimal? ValueTo3 { get; set; }
        public string ObjAppInfoMapLogicCode_FK { get; set; }

        public List<StaticItem> Value1List_VW { get; set; }
        public List<StaticItem> Value2List_VW { get; set; }
        public List<StaticItem> Value3List_VW { get; set; }

        public string OptionText1_VW { get; set; }
        public string OptionText2_VW { get; set; }
        public string OptionText3_VW { get; set; }
        public int SlNo { get; set; }
        public int ActiveYN { get; set; }
    }
}
