using CommonInterface;
using CommonModel;
using ConnectionGateway;
using LSCrud;
using PMSInterface;
using PMSModel;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace PMSDataContext
{
    public class ProductRepository: IProduct
    {
        ISupplierDbContext _supplierDbContext;
        ICRUD _cRUD;
        IIDGenCriteriaInfo _iIDGenCriteriaInfo;
        public ProductRepository(ISupplierDbContext supplierDbContext, ICRUD cRUD, IIDGenCriteriaInfo iDGenCriteriaInfo)
        {
            _supplierDbContext = supplierDbContext;
            _cRUD = cRUD;
            _iIDGenCriteriaInfo = iDGenCriteriaInfo;
        }
        static readonly string Product_TBL = "PM_Product";
        public string SaveProduct(Product objproduct)
        {
            int vResult = 0;
            string vOut = "Exception Occured !";
            objproduct.TableName_TBL = Product_TBL;
            string vQuery = string.Empty;
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            using (SqlTransaction trans = connection.BeginTransaction())
            {
                if (string.IsNullOrEmpty(objproduct.ProductCode_PK))
                {
                    objproduct.ProductCode_PK = Guid.NewGuid().ToString();
                    objproduct.ProductID= _iIDGenCriteriaInfo.GenerateID(trans, objproduct, EnumIdCategory.ProductID);

                }
                vQuery = _cRUD.CREATEQuery(objproduct);
                try
                {
                    using (SqlCommand command = _supplierDbContext.GetCommand())
                    {
                        command.CommandText = vQuery;
                        vResult = command.ExecuteNonQuery();
                    }
                    if (vResult > 0)
                    {
                        trans.Commit();
                        vOut = "Product Saved Successfully";
                    
}
                }

                catch (DbException ex)
                {
                    trans.Rollback();
                    throw ex;

                }
                finally
                {
                    connection.Close();
                }
            }
            return vOut;
            }
        public List<Product> GetProductsList()
        {
            List<Product> products = new List<Product>();
            Product objProduct;
            string vComTxt = @"SELECT  [ProductCode]
                                      ,[ProductID]
                                      ,[ProductName]
                                      ,[chkSampleProduct]
                                      ,[chkSerialNoManage]
                                      ,[chkServiceProduct]
                                      ,[chkMaintenanceServiceProduct]
                                      ,[RepairingStatus]
                                      ,[FileCodeForProductCategory]
                                      ,dbo.fxn_FileName([FileCodeForProductCategory]) productcatagory
                                      ,[FileCodeForBrand]
                                      ,dbo.fxn_FileName([FileCodeForBrand]) productbrand
                                      ,[ProductSeries]
                                      ,[PartNumber]
                                      ,[CountryofOrigin]
                                      ,dbo.fxn_FileName([CountryofOrigin]) countryoforigin_nm
                                      ,[CountryofManufactur]
                                      ,dbo.fxn_FileName(CountryofManufactur) CountryofManufactur_nm
                                      ,[FileCodeForMou]
                                      ,dbo.fxn_FileName(FileCodeForMou) mou
                                      ,[Description]
                                      ,[VAT]
                                      ,[TPRate]
                                      ,[Status]
                                      ,[PackCCSize]
                                      ,isnull([WorkingUnitCode],0)WorkingUnitCode
                                  FROM [PM_Product]";
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            dr = objDbCommand.ExecuteReader();
            while (dr.Read())
            {
                objProduct = new Product();
                objProduct.ProductCode_PK= dr["ProductCode"].ToString();
                objProduct.ProductID = dr["ProductID"].ToString();
                objProduct.ProductName = dr["ProductName"].ToString();
                objProduct.chkSampleProduct = Convert.ToInt16(dr["chkSampleProduct"].ToString());
                objProduct.chkSerialNoManage = Convert.ToInt16(dr["chkSerialNoManage"].ToString());
                objProduct.chkServiceProduct = Convert.ToInt16(dr["chkServiceProduct"].ToString());
                objProduct.chkMaintenanceServiceProduct = Convert.ToInt16(dr["chkMaintenanceServiceProduct"].ToString());
                objProduct.RepairingStatus = Convert.ToInt16(dr["RepairingStatus"].ToString());
                objProduct.FileCodeForProductCategory = Convert.ToInt16(dr["FileCodeForProductCategory"].ToString());
                objProduct.ProductcatagoryName_VW = dr["productcatagory"].ToString();
                objProduct.FileCodeForBrand = Convert.ToInt16(dr["FileCodeForBrand"].ToString());
                objProduct.ProductSeries = dr["ProductSeries"].ToString();
                objProduct.PartNumber = dr["PartNumber"].ToString();
                objProduct.CountryofOrigin = Convert.ToInt16(dr["CountryofOrigin"].ToString());
                objProduct.CountryoforiginName_VW = dr["countryoforigin_nm"].ToString();
                objProduct.CountryofManufactur = Convert.ToInt16(dr["CountryofManufactur"].ToString());
                objProduct.CountryofManufactureName_VW = dr["CountryofManufactur_nm"].ToString();
                objProduct.FileCodeForMou = Convert.ToInt16(dr["FileCodeForMou"].ToString());
                objProduct.MOU_VW = dr["mou"].ToString();
                objProduct.Description = dr["Description"].ToString();
                objProduct.VAT = Convert.ToDouble(dr["VAT"].ToString());
                objProduct.TPRate = Convert.ToDouble(dr["TPRate"].ToString());
                objProduct.Status = Convert.ToInt16(dr["Status"].ToString());
                objProduct.PackCCSize = Convert.ToDouble(dr["PackCCSize"].ToString());
                objProduct.WorkingUnitCode = Convert.ToInt16(dr["WorkingUnitCode"].ToString());


                products.Add(objProduct);
            }

            return products;
        }
        public Product GetProductByCode(string productcode)
        {
            Product objProduct = new Product();
            string vComTxt = @"SELECT  [ProductCode]
                                      ,[ProductID]
                                      ,[ProductName]
                                      ,[chkSampleProduct]
                                      ,[chkSerialNoManage]
                                      ,[chkServiceProduct]
                                      ,[chkMaintenanceServiceProduct]
                                      ,[RepairingStatus]
                                      ,[FileCodeForProductCategory]
                                      ,dbo.fxn_FileName([FileCodeForProductCategory]) productcatagory
                                      ,[FileCodeForBrand]
                                      ,dbo.fxn_FileName([FileCodeForBrand]) productbrand
                                      ,[ProductSeries]
                                      ,[PartNumber]
                                      ,[CountryofOrigin]
                                      ,dbo.fxn_FileName([CountryofOrigin]) countryoforigin_nm
                                      ,[CountryofManufactur]
                                      ,dbo.fxn_FileName(CountryofManufactur) CountryofManufactur_nm
                                      ,[FileCodeForMou]
                                      ,dbo.fxn_FileName(FileCodeForMou) mou
                                      ,[Description]
                                      ,[VAT]
                                      ,[TPRate]
                                      ,[Status]
                                      ,[PackCCSize]
                                      ,[WorkingUnitCode]
                                  FROM [PM_Product] WHERE [ProductCode]= '" + productcode + "'";
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            dr = objDbCommand.ExecuteReader();
            if (dr.Read())
            {
                objProduct.ProductCode_PK = dr["ProductCode"].ToString();
                objProduct.ProductID = dr["ProductID"].ToString();
                objProduct.ProductName = dr["ProductName"].ToString();
                objProduct.chkSampleProduct = Convert.ToInt16(dr["chkSampleProduct"].ToString());
                objProduct.chkSerialNoManage = Convert.ToInt16(dr["chkSerialNoManage"].ToString());
                objProduct.chkServiceProduct = Convert.ToInt16(dr["chkServiceProduct"].ToString());
                objProduct.chkMaintenanceServiceProduct = Convert.ToInt16(dr["chkMaintenanceServiceProduct"].ToString());
                objProduct.RepairingStatus = Convert.ToInt16(dr["RepairingStatus"].ToString());
                objProduct.FileCodeForProductCategory = Convert.ToInt16(dr["FileCodeForProductCategory"].ToString());
                objProduct.ProductcatagoryName_VW = dr["productcatagory"].ToString();
                objProduct.FileCodeForBrand = Convert.ToInt16(dr["FileCodeForBrand"].ToString());
                objProduct.ProductSeries = dr["ProductSeries"].ToString();
                objProduct.PartNumber = dr["PartNumber"].ToString();
                objProduct.CountryofOrigin = Convert.ToInt16(dr["CountryofOrigin"].ToString());
                objProduct.CountryoforiginName_VW = dr["countryoforigin_nm"].ToString();
                objProduct.CountryofManufactur = Convert.ToInt16(dr["CountryofManufactur"].ToString());
                objProduct.CountryofManufactureName_VW = dr["CountryofManufactur_nm"].ToString();
                objProduct.FileCodeForMou = Convert.ToInt16(dr["FileCodeForMou"].ToString());
                objProduct.MOU_VW = dr["mou"].ToString();
                objProduct.Description = dr["Description"].ToString();
                objProduct.VAT = Convert.ToDouble(dr["VAT"].ToString());
                objProduct.TPRate = Convert.ToDouble(dr["TPRate"].ToString());
                objProduct.Status = Convert.ToInt16(dr["Status"].ToString());
                objProduct.PackCCSize = Convert.ToDouble(dr["PackCCSize"].ToString());
                objProduct.WorkingUnitCode = Convert.ToInt16(dr["WorkingUnitCode"].ToString());
            }
            return objProduct;

            }
        public List<Product> GetProductsListByCategory(int productcategory)
        {
            List<Product> products = new List<Product>();
            Product objProduct;
            string vComTxt = @"SELECT  [ProductCode]
                                      ,[ProductID]
                                      ,[ProductName]
                                      ,[chkSampleProduct]
                                      ,[chkSerialNoManage]
                                      ,[chkServiceProduct]
                                      ,[chkMaintenanceServiceProduct]
                                      ,[RepairingStatus]
                                      ,[FileCodeForProductCategory]
                                      ,dbo.fxn_FileName([FileCodeForProductCategory]) productcatagory
                                      ,[FileCodeForBrand]
                                      ,dbo.fxn_FileName([FileCodeForBrand]) productbrand
                                      ,[ProductSeries]
                                      ,[PartNumber]
                                      ,[CountryofOrigin]
                                      ,dbo.fxn_FileName([CountryofOrigin]) countryoforigin_nm
                                      ,[CountryofManufactur]
                                      ,dbo.fxn_FileName(CountryofManufactur) CountryofManufactur_nm
                                      ,[FileCodeForMou]
                                      ,dbo.fxn_FileName(FileCodeForMou) mou
                                      ,[Description]
                                      ,[VAT]
                                      ,[TPRate]
                                      ,[Status]
                                      ,[PackCCSize]
                                      ,[WorkingUnitCode]
                                  FROM [PM_Product] WHERE [FileCodeForProductCategory]= '" + productcategory + "'";
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            dr = objDbCommand.ExecuteReader();
            while (dr.Read())
            {
                objProduct = new Product();
                objProduct.ProductCode_PK = dr["ProductCode"].ToString();
                objProduct.ProductID = dr["ProductID"].ToString();
                objProduct.ProductName = dr["ProductName"].ToString();
                objProduct.chkSampleProduct = Convert.ToInt16(dr["chkSampleProduct"].ToString());
                objProduct.chkSerialNoManage = Convert.ToInt16(dr["chkSerialNoManage"].ToString());
                objProduct.chkServiceProduct = Convert.ToInt16(dr["chkServiceProduct"].ToString());
                objProduct.chkMaintenanceServiceProduct = Convert.ToInt16(dr["chkMaintenanceServiceProduct"].ToString());
                objProduct.RepairingStatus = Convert.ToInt16(dr["RepairingStatus"].ToString());
                objProduct.FileCodeForProductCategory = Convert.ToInt16(dr["FileCodeForProductCategory"].ToString());
                objProduct.ProductcatagoryName_VW = dr["productcatagory"].ToString();
                objProduct.FileCodeForBrand = Convert.ToInt16(dr["FileCodeForBrand"].ToString());
                objProduct.ProductSeries = dr["ProductSeries"].ToString();
                objProduct.PartNumber = dr["PartNumber"].ToString();
                objProduct.CountryofOrigin = Convert.ToInt16(dr["CountryofOrigin"].ToString());
                objProduct.CountryoforiginName_VW = dr["countryoforigin_nm"].ToString();
                objProduct.CountryofManufactur = Convert.ToInt16(dr["CountryofManufactur"].ToString());
                objProduct.CountryofManufactureName_VW = dr["CountryofManufactur_nm"].ToString();
                objProduct.FileCodeForMou = Convert.ToInt16(dr["FileCodeForMou"].ToString());
                objProduct.MOU_VW = dr["mou"].ToString();
                objProduct.Description = dr["Description"].ToString();
                objProduct.VAT = Convert.ToDouble(dr["VAT"].ToString());
                objProduct.TPRate = Convert.ToDouble(dr["TPRate"].ToString());
                objProduct.Status = Convert.ToInt16(dr["Status"].ToString());
                objProduct.PackCCSize = Convert.ToDouble(dr["PackCCSize"].ToString());
                objProduct.WorkingUnitCode = Convert.ToInt16(dr["WorkingUnitCode"].ToString());


                products.Add(objProduct);
            }

            return products;
        }
        }
}
