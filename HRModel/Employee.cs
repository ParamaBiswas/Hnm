using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRModel
{
    public class Employee:BaseModel
    {
        public string TableName_TBL { get; set; }
        public string EmployeeCode_PK { get; set; }
        public string EmployeeId { get; set; }
        public int? DepartmentCode_FK { get; set; }
        public string Department_VW { get; set; }
        public int? DesignationCode_FK { get; set; }
        public string Designation_VW { get; set; }
        public string Name { get; set; }
        public int Sex { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string ReportToCode { get; set; }
        public string ReportToName_VW { get; set; }
        public string PresentAddress { get; set; }
        public string DateOfBirth { get; set; }
        public int JobStatus { get; set; }
        public string ContactNumber { get; set; }
        public string JobStatusChangedDate { get; set; }
    }
}
