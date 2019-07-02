using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSModel
{
    public class ItemReceiveDet : BaseModel
    {
        public string TableName_TBL { get; set; }
        public string ItemCode { get; set; }
        public string ReceiveCode_FK { get; set; }
        public int DepotCode { get; set; }
        public string SpecCode { get; set; }
        public int PageCode { get; set; }
        public decimal AllocatedQuantity { get; set; }
        public string ReciveRateCode { get; set; }
        public decimal ReceiveRate { get; set; }
        public int NoOfContainer { get; set; }
        public int ItemMoUCode { get; set; }
        public int ScannedQuantity { get; set; }
        public decimal ReceiveQuantity { get; set; }
        public string ReceiveDetCode_PK { get; set; }
        public int ReceiveQuantityType { get; set; }

        public string ProductID_VW { get; set; }
        public string ProductName_VW { get; set; }
    }
}
