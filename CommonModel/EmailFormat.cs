using System;
using System.Collections.Generic;
using System.Text;

namespace CommonModel
{
    public class EmailFormat:BaseModel
    {
        public string TableName_TBL { get; set; }
        public string EmailSubject { get; set; }
        public string EmailCode_PK { get; set; }
        public int EmailID { get; set; }
        public string EmailBody { get; set; }
    }
}
