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
    public class ItemReceiveRepository: IItemReceiveRepository
    {
        ISupplierDbContext _supplierDbContext;
        ICRUD _cRUD;
        IIDGenCriteriaInfo _iIDGenCriteriaInfo;
        public ItemReceiveRepository(ISupplierDbContext supplierDbContext, ICRUD cRUD, IIDGenCriteriaInfo iDGenCriteriaInfo)
        {
            _supplierDbContext = supplierDbContext;
            _cRUD = cRUD;
            _iIDGenCriteriaInfo = iDGenCriteriaInfo;
        }
        static readonly string Inv_ItemReceive_TBL = "Inv_ItemReceive";
        static readonly string Inv_ItemReceiveDet_TBL = "Inv_ItemReceiveDet";
        public string SaveItemReceive(ItemReceive objItemReceive)
        {
            int vResult = 0;
            string vOut = "Exception Occured !";
            ArrayList vQueryList = new ArrayList();
            objItemReceive.TableName_TBL = Inv_ItemReceive_TBL;
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            using (SqlTransaction trans = connection.BeginTransaction())
            {
                if (string.IsNullOrEmpty(objItemReceive.ReceiveCode_PK))
                {
                    objItemReceive.ReceiveCode_PK = Guid.NewGuid().ToString();
                    objItemReceive.GRN = _iIDGenCriteriaInfo.GenerateID(trans, objItemReceive, EnumIdCategory.GRN);
                }
                vQueryList.Add(GetQuery(objItemReceive));
                foreach (ItemReceiveDet objItemReceiveDet in objItemReceive.ItemReceiveDet_VW)
                {
                    if (string.IsNullOrEmpty(objItemReceiveDet.ReceiveDetCode_PK))
                    {
                        objItemReceiveDet.ReceiveDetCode_PK = Guid.NewGuid().ToString();
                        objItemReceiveDet.TableName_TBL = Inv_ItemReceiveDet_TBL;
                        objItemReceiveDet.ReceiveCode_FK = objItemReceive.ReceiveCode_PK;

                    }
                    vQueryList.Add(GetQuery(objItemReceiveDet));
                    
                }
                
                try
                {
                    using (SqlCommand command = new SqlCommand("", connection, trans))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        foreach (string obj_temp in vQueryList)
                        {
                            command.CommandText = obj_temp;
                            vResult = command.ExecuteNonQuery();
                        }

                    }
                    if (vResult > 0)
                    {
                        trans.Commit();
                        vOut = objItemReceive.GRN;
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
        public List<ItemReceive> GetItemReceiveByPO(string pPOCode)
        {
            List<ItemReceive> itemReceives = new List<ItemReceive>();
            string vComTxt = @"select ReceiveCode,GRN,ReceiveDate,IsApproved,ApprovalAction,POCode,sum(Quantity)Quantity,
                                sum(ReceiveQuantity)ReceiveQuantity,sum(invoice_amt)invoice_amt,sum(VAT_Amount)VAT_Amount
                                ,rate,VATRate,sum(invoice_amt)+sum(VAT_Amount) subtotalamount
                                from(
                                SELECT r.[ReceiveCode]
                                        ,[GRN]
                                        ,[ReceiveDate]
                                        ,r.[IsApproved]
                                        ,r.[ApprovalAction]
                                        ,[POCode]
	                                    ,i.Quantity
	                                    ,e.ReceiveQuantity
	                                    ,e.ItemCode
	                                    ,m.ProductID
	                                    ,m.ProductName
                                        ,e.ReceiveQuantity*i.rate invoice_amt
                                        ,(i.[VATRate]/100)*(e.ReceiveQuantity*i.rate) VAT_Amount
                                        ,i.rate 
                                        ,i.[VATRate]
                                    FROM [dbo].[Inv_ItemReceive] r
                                    join [dbo].[PMS_PurchaseOrder] p
                                    on r.POCode=p.PurchaseOrderCode
                                    join [dbo].[LSP_PMS_QuotationItem] i
                                    on i.QuotationCode=p.QuotationCode
                                    join [dbo].[Inv_ItemReceiveDet] e
                                    on e.ReceiveCode=r.ReceiveCode
                                    join [dbo].[PM_Product] m
                                    on m.ProductCode=e.ItemCode
                                    WHERE  isnull(IsInvoiced,0)<>1)a
                                  WHERE [POCode]= '" + pPOCode + "'" +
                                  "group by ReceiveCode,GRN,ReceiveDate,IsApproved,ApprovalAction,POCode,rate,VATRate";
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            dr = objDbCommand.ExecuteReader();
            while (dr.Read())
            {
                ItemReceive item = new ItemReceive();
                item.ReceiveCode_PK= dr["ReceiveCode"].ToString();
                item.GRN= dr["GRN"].ToString();
                item.ReceiveDate= dr.GetDateTime(dr.GetOrdinal("ReceiveDate")).ToString("dd-MM-yyyy");
                item.IsApproved= Convert.ToInt16(dr["IsApproved"].ToString());
                item.ApprovalAction= Convert.ToInt16(dr["ApprovalAction"].ToString());
                item.POCode= dr["POCode"].ToString();
                item.TotalQuantity_VW= Convert.ToDecimal(dr["Quantity"].ToString());
                item.ReceiveQuantity_VW = Convert.ToDecimal(dr["ReceiveQuantity"].ToString());
                item.InvoiceAmount_VW = Convert.ToDecimal(dr["invoice_amt"].ToString());
                item.TotalVAT_VW = Convert.ToDecimal(dr["VAT_Amount"].ToString());
                item.Rate_VW = Convert.ToDecimal(dr["rate"].ToString());
                item.VATRate_VW = Convert.ToDecimal(dr["VATRate"].ToString());
                item.SubTotalAmount_VW = Convert.ToDecimal(dr["subtotalamount"].ToString());

                itemReceives.Add(item);
            }
            dr.Close();
            connection.Close();
            return itemReceives;
        }
        
    }
}
