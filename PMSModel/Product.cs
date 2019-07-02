using CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSModel
{
    public class Product :BaseModel
    {
        public string TableName_TBL { get; set; }
        public string ProductID { get; set; }
        public int? RepairingStatus { get; set; }
        public int? chkSampleProduct { get; set; }
        public int? chkServiceProduct { get; set; }
        public int? chkMaintenanceServiceProduct { get; set; }
        public int? Status { get; set; }
        public double? PackCCSize { get; set; }
        public string ProductName { get; set; }
        public int? FileCodeForProductCategory { get; set; }
        public int? chkSerialNoManage { get; set; }
        public double? VAT { get; set; }
        public string Description { get; set; }
        public int? FileCodeForMou { get; set; }
        public int? CountryofManufactur { get; set; }
        public int? CountryofOrigin { get; set; }
        public string PartNumber { get; set; }
        public string ProductSeries { get; set; }
        public int? FileCodeForBrand { get; set; }
        public int? WorkingUnitCode { get; set; }
        public string ProductCode_PK { get; set; }
        public double? TPRate { get; set; }
        public string ProductcatagoryName_VW { get; set; }
        public string ProductbrandName_VW { get; set; }
        public string CountryoforiginName_VW { get; set; }
        public string CountryofManufactureName_VW { get; set; }
        public string MOU_VW { get; set; }
    }
}
