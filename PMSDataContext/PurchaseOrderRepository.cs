using CommonInterface;
using CommonModel;
using ConnectionGateway;
using LSCrud;
using PMSInterface;
using PMSModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace PMSDataContext
{
    public class PurchaseOrderRepository: IPurchaseOrderRepository
    {
        ISupplierDbContext _supplierDbContext;
        ICRUD _cRUD;
        IIDGenCriteriaInfo _iIDGenCriteriaInfo;
        public PurchaseOrderRepository(ISupplierDbContext supplierDbContext, ICRUD cRUD, IIDGenCriteriaInfo iDGenCriteriaInfo)
        {
            _supplierDbContext = supplierDbContext;
            _cRUD = cRUD;
            _iIDGenCriteriaInfo = iDGenCriteriaInfo;
        }
        static readonly string PurchaseOrder_TBL = "PMS_PurchaseOrder";
        public string SavePurchaseOrder(PurchaseOrder objPurchaseOrder)
        {
            int vResult = 0;
            string vOut = "Exception Occured !";
            ArrayList vQueryList = new ArrayList();
            objPurchaseOrder.TableName_TBL = PurchaseOrder_TBL;
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            using (SqlTransaction trans = connection.BeginTransaction())
            {
                if (string.IsNullOrEmpty(objPurchaseOrder.PurchaseOrderCode_PK))
                {
                    objPurchaseOrder.PurchaseOrderCode_PK = Guid.NewGuid().ToString();
                    objPurchaseOrder.PurchaseOrderNo = _iIDGenCriteriaInfo.GenerateID(trans, objPurchaseOrder, EnumIdCategory.PurchaseOrderNo);
                }
                vQueryList.Add(GetQuery(objPurchaseOrder));

                try
                {
                    using (SqlCommand command = _supplierDbContext.GetCommand())
                    {
                        foreach (string obj_temp in vQueryList)
                        {
                            command.CommandText = obj_temp;
                            vResult = command.ExecuteNonQuery();
                        }

                    }
                    if (vResult > 0)
                    {
                        trans.Commit();
                        vOut = "Purchase Order Saved Successfully";
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
        public string GetQuery(object objObject)
        {
            BaseModel obj_Temp = (BaseModel)objObject;
            string vQuery = "";
            if (obj_Temp.IsNew)
            {

                vQuery = _cRUD.CREATEQuery(objObject);
            }
            else if (obj_Temp.IsDeleted == true)
            {
                Hashtable pMarkDELColNameValues = new Hashtable();
                pMarkDELColNameValues.Clear();
                obj_Temp.ActionType = "DELETE";
                pMarkDELColNameValues.Add("ActionType", obj_Temp.ActionType);
                pMarkDELColNameValues.Add("ActionDate", obj_Temp.ActionDate);
                pMarkDELColNameValues.Add("UserCode", obj_Temp.UserCode);
                pMarkDELColNameValues.Add("CompanyCode", obj_Temp.CompanyCode_FK);
                vQuery = _cRUD.DELETEQuery(objObject, true, pMarkDELColNameValues);
            }
            else
            {
                vQuery = _cRUD.UPDATEQuery(objObject);
            }
            return vQuery;
        }
        public List<PurchaseOrder> GetPurchaseOrders(string pDateFrom, string pDateTo)
        {
            List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();
            string vComTxt = @"SELECT p.[PurchaseOrderCode]
                                      ,[PurchaseOrderNo]
                                      ,[OrderDate]
                                      ,p.[QuotationCode]
                                      ,p.[RequisitionCode]
                                      ,p.[SupplierCode]
                                      ,s.SupplierID
                                      ,s.SupplierName
                                      ,r.RequisitionID
                                      ,r.RequisitionTitle
                                  FROM [PMS_PurchaseOrder] p
                                  join [dbo].[PMS_PurchaseRequisition] r
                                  on r.RequisitionCode=p.RequisitionCode
                                  join [dbo].[LSP_PMS_Quotation] q
                                  on q.QuotationCode=p.QuotationCode
                                  join [dbo].[LSP_PMS_SupplierInfo] s
                                  on s.SupplierCode=p.SupplierCode";
            if(!string.IsNullOrEmpty(pDateFrom) && !string.IsNullOrEmpty(pDateTo))
            {
                vComTxt = vComTxt + @" where [OrderDate] BETWEEN '" + pDateFrom + @"' AND '" + pDateTo +"'";
            }
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            dr = objDbCommand.ExecuteReader();
            while (dr.Read())
            {
                PurchaseOrder purchase = new PurchaseOrder();
                purchase.PurchaseOrderCode_PK= dr["PurchaseOrderCode"].ToString();
                purchase.PurchaseOrderNo= dr["PurchaseOrderNo"].ToString();
                purchase.OrderDate= dr.GetDateTime(dr.GetOrdinal("OrderDate")).ToString("dd-MM-yyyy");
                purchase.QuotationCode= dr["QuotationCode"].ToString();
                purchase.RequisitionCode= dr["RequisitionCode"].ToString();
                purchase.SupplierCode= dr["SupplierCode"].ToString();
                purchase.SupplierID_VW= dr["SupplierID"].ToString();
                purchase.SupplierName_VW= dr["SupplierName"].ToString();
                purchase.RequisitionID_VW= dr["RequisitionID"].ToString();
                purchase.RequisitionTitle_VW= dr["RequisitionTitle"].ToString();
                purchaseOrders.Add(purchase);

            }
            dr.Close();
            return purchaseOrders;

        }
        public PurchaseOrder GetPurchaseOrderByCode(string PurchaseOrderCode)
        {
            PurchaseOrder objPurchaseOrder = new PurchaseOrder();
            string vComTxt = @"SELECT  PurchaseOrderCode
                                      ,PurchaseOrderNo
                                      ,OrderDate
                                      ,q.[SupplierCode]
                                      ,s.SupplierName
                                      ,s.SupplierID
                                      ,s.SupplierAddress
                                      ,q.QuotationCode
                                  FROM PMS_PurchaseOrder q
                                  join LSP_PMS_SupplierInfo s on s.SupplierCode=q.SupplierCode
                                  WHERE PurchaseOrderCode= '" + PurchaseOrderCode + "'";
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);

            dr = objDbCommand.ExecuteReader();
            if (dr.Read())
            {
                objPurchaseOrder = new PurchaseOrder();
                objPurchaseOrder.PurchaseOrderCode_PK= dr["PurchaseOrderCode"].ToString();
                objPurchaseOrder.PurchaseOrderNo= dr["PurchaseOrderNo"].ToString();
                objPurchaseOrder.OrderDate= dr.GetDateTime(dr.GetOrdinal("OrderDate")).ToString("dd-MM-yyyy");
                objPurchaseOrder.SupplierCode= dr["SupplierCode"].ToString();
                objPurchaseOrder.SupplierID_VW= dr["SupplierID"].ToString();
                objPurchaseOrder.SupplierName_VW= dr["SupplierName"].ToString();
                objPurchaseOrder.SupplierAddress_VW= dr["SupplierAddress"].ToString();
                objPurchaseOrder.QuotationCode= dr["QuotationCode"].ToString();

            }
            dr.Close();
            objPurchaseOrder.QuotationItemList_VW = GetQuotationItem(objPurchaseOrder.QuotationCode, connection);

            connection.Close();
            return objPurchaseOrder;

        }
        public List<QuotationItem> GetQuotationItem(string QuotationCode, SqlConnection connection)
        {
            List<QuotationItem> objQuotationItemList_VW = new List<QuotationItem>();

            string vComTxt1 = @"SELECT  [QuotationDetCode]
                                      ,[QuotationCode]
                                      ,[RequisitionDetCode]
                                      ,[RequisitionCode]
                                      ,a.[ProductCode]
                                      ,B.productId
	                                  ,B.productName
                                      ,[ProductType]
                                      ,dbo.fxn_FileName(ProductType) productcategory
                                      ,[Quantity]
                                      ,[QunatityMOUCode]
                                      ,dbo.fxn_FileName(QunatityMOUCode)  as  Mou
                                      ,[VATRate]
                                      ,[Rate]
                                      ,[TotalVAT]
                                  FROM [dbo].[LSP_PMS_QuotationItem]  A join pm_product B on A.productcode = B.productCode
                                        WHERE A.[QuotationCode]= '" + QuotationCode + "'";

            SqlCommand objDbCommand1 = new SqlCommand(vComTxt1, connection);
            SqlDataReader dr;
            dr = objDbCommand1.ExecuteReader();
            while (dr.Read())
            {
                QuotationItem obj = new QuotationItem();
                obj.QuotationDetCode_PK = dr["QuotationDetCode"].ToString();
                obj.QuotationCode = dr["QuotationCode"].ToString();
                obj.RequisitionDetCode = dr["RequisitionDetCode"].ToString();
                obj.RequisitionCode = dr["RequisitionCode"].ToString();
                obj.ProductCode = dr["ProductCode"].ToString();
                obj.ProductID_VW = dr["productId"].ToString();
                obj.ProductName_VW = dr["productName"].ToString();
                obj.QunatityMOUCode = Convert.ToInt16(dr["QunatityMOUCode"].ToString());
                obj.QunatityMOU_VW = dr["Mou"].ToString();
                obj.Quantity = Convert.ToDecimal(dr["Quantity"].ToString());
                obj.ProductType = Convert.ToInt16(dr["ProductType"].ToString());
                obj.ProductCatagory_NM = dr["productcategory"].ToString();
                obj.VATRate = Convert.ToDecimal(dr["VATRate"].ToString());
                obj.Rate = Convert.ToDecimal(dr["Rate"].ToString());
                obj.TotalVAT = Convert.ToDecimal(dr["TotalVAT"].ToString());
                objQuotationItemList_VW.Add(obj);

            }
            dr.Close();
            return objQuotationItemList_VW;

        }

    }
}
