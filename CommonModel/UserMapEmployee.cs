using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;



namespace LS.General.ModelBiz
{
     public class UserMapEmployee : BaseModel
    {
        private string vMSG = String.Empty;

        #region Properties
        public string TableName_TBL { get; set; }
        public string UserMapCode_PK { get; set; }
        public string EmployeeCode_FK { get; set; }
        //public string EmployeeId_VW { get; set; }
        public string UserId_FK { get; set; }
        public string UserName_VW { get; set; }
        public string DeptCode_VW { get; set; }
        public string UserID_VW { get; set; }
        public int? IsActive { get; set; }
        public bool IsActive_VW { get; set; }
        #endregion


    }
}