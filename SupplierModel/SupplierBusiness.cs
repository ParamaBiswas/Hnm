using CommonModel;

namespace LS.LS.ModelBiz
{
    public class SupplierBusiness :BaseModel
       {
          public string TableName_TBL { get; set; }
          public string SupplierCode_FK { get; set; }
          public int BusinessTypeCode_PK { get; set; }
          public string filename_VW { get; set; }


        }
}