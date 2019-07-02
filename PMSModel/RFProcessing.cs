using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSModel
{
    public class RFProcessing:BaseModel
    {
        public string TableName_TBL { get; set; }
        public string PublishDate { get; set; }
        public string RFProcessCode_PK { get; set; }
        public string RequisitionCode_FK { get; set; }
        public int IsApproved { get; set; }
        public string ClosingDate { get; set; }
        public string OpeningDate { get; set; }
        public int Status { get; set; }
        public string Status_VW { get; set; }
        public string InvitingAuthority { get; set; }
        public string EmployeeName_VW { get; set; }
        public string EmployeeID_VW { get; set; }
        public string Address_VW { get; set; }
        public string Designation_VW { get; set; }
        public string Contact_VW { get; set; }
        public string Remarks { get; set; }
        public int ApprovalAction { get; set; }
        public string RFProcessId { get; set; }
        public int RequisitionType_VW { get; set; }
        public string RequisitionTypeName_VW { get; set; }
        public string RequisitionTitle_VW { get; set; }
        public int? SLNO_VW { get; set; }

    }
}
