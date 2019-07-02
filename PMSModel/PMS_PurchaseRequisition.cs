using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSModel
{
    public class PurchaseRequisition:BaseModel
    {
        public string TableName_TBL { get; set; }
        public string RequisitionID { get; set; }
        public int RequisitionType { get; set; }
        public string RequisitionType_VW { get; set; }
        public string RequisitionTitle { get; set; }
        public int RequisitionFor { get; set; }
        public string RequisitionFor_VW { get; set; }
        public int RequisitionForType { get; set; }
        public string RequisitionForTypeName_VW { get; set; }
        public string Remarks { get; set; }
        public string RequisitionDate { get; set; }
        public int ReqProductCategory { get; set; }
        public string ReqProductCategory_VW { get; set; }
        public string Address { get; set; }
        public int IsApproved { get; set; }
        public string PenaltyClause { get; set; }
        public string Requester { get; set; }
        public string Requester_VW { get; set; }
        public string SubmissionDeadline { get; set; }
        public string WarrantyToDate { get; set; }
        public string WarrantyFromDate { get; set; }
        public string MaturityTo { get; set; }
        public string MaturityFrom { get; set; }
        public string RequisitionCode_PK { get; set; }
        public int ApprovalAction { get; set; }
        public int PriceLock { get; set; }
        public string PriceLockName_VW { get; set; }
        public string EmployeeId_VW { get; set; }

        public List<PurchaseRequisitionItem> PurchaseRequisitionItemList_VW { get; set; }
        public List<PurchaseRequisitionCondition> PurchaseRequisitionTermsList_VW { get; set; }
    }
}
