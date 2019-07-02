using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSModel
{
    public class ItemReceive:BaseModel
    {
        public string TableName_TBL { get; set; }
        public string GRN { get; set; }
        public string Remarks { get; set; }
        public int ReceiveType { get; set; }
        public int ItemStatusCode { get; set; }
        public int ReceiveFrom { get; set; }
        public string ReceiveDate { get; set; }
        public string ReceiveByCode { get; set; }
        public string RefCode { get; set; }
        public int DepotCode { get; set; }
        public string POCode { get; set; }
        public int IsReceived { get; set; }
        public int ApprovalAction { get; set; }
        public int IsApproved { get; set; }
        public string ChallanCode { get; set; }
        public string IssueCode { get; set; }
        public string ReceiveCode_PK { get; set; }

        public List<ItemReceiveDet> ItemReceiveDet_VW { get; set; }
        public decimal TotalQuantity_VW { get; set; }
        public string ProductID_VW { get; set; }
        public string ProductName_VW { get; set; }
        public decimal ReceiveQuantity_VW { get; set; }
        public decimal InvoiceAmount_VW { get; set; }
        public decimal TotalVAT_VW { get; set; }
        public decimal Rate_VW { get; set; }
        public decimal VATRate_VW { get; set; }
        public decimal SubTotalAmount_VW { get; set; }
    }
}
