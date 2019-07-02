using CommonModel;

namespace LS.LS.ModelBiz
{
    public class SupplierContact :BaseModel
       {
           public string TableName_TBL { get; set; }
           public string SupplierCode_FK { get; set; }
           public string ContactPersonCode_PK { get; set; }
           public string TelephoneNumber { get; set; }
           public string JobRole { get; set; }
           public string ContactName { get; set; }
           public string Email { get; set; }
           public string MobileNumber { get; set; }
           public string Designation { get; set; }
           
        }
}