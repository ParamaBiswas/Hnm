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
    public class SupplierInvoiceRepository : ISupplierInvoiceRepository
    {
        ISupplierDbContext _supplierDbContext;
        ICRUD _cRUD;
        IIDGenCriteriaInfo _iIDGenCriteriaInfo;
        public SupplierInvoiceRepository(ISupplierDbContext supplierDbContext, ICRUD cRUD, IIDGenCriteriaInfo iDGenCriteriaInfo)
        {
            _supplierDbContext = supplierDbContext;
            _cRUD = cRUD;
            _iIDGenCriteriaInfo = iDGenCriteriaInfo;
        }
        static readonly string PMS_SupplierInvoice_TBL = "PMS_SupplierInvoice";
        static readonly string PMS_SupplierInvoiceLine_TBL = "PMS_SupplierInvoiceLine";
        public string SaveSupplierInvoice(SupplierInvoice objSupplierInvoice)
        {
            int vResult = 0;
            string vOut = "Exception Occured !";
            string vUpdateQuery = String.Empty;
            ArrayList vQueryList = new ArrayList();
            objSupplierInvoice.TableName_TBL = PMS_SupplierInvoice_TBL;
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            using (SqlTransaction trans = connection.BeginTransaction())
            {
                if (string.IsNullOrEmpty(objSupplierInvoice.InvoiceCode_PK))
                {
                    objSupplierInvoice.InvoiceCode_PK = Guid.NewGuid().ToString();
                    objSupplierInvoice.InvoiceNo = _iIDGenCriteriaInfo.GenerateID(trans, objSupplierInvoice, EnumIdCategory.Invoice);
                }
                vQueryList.Add(GetQuery(objSupplierInvoice));
                foreach (SupplierInvoiceLine objSupplierInvoiceLine in objSupplierInvoice.SupplierInvoiceLine_VW)
                {
                    if (string.IsNullOrEmpty(objSupplierInvoiceLine.InvoiceLineCode_PK))
                    {
                        objSupplierInvoiceLine.InvoiceLineCode_PK = Guid.NewGuid().ToString();
                        objSupplierInvoiceLine.TableName_TBL = PMS_SupplierInvoiceLine_TBL;
                        objSupplierInvoiceLine.InvoiceCode = objSupplierInvoice.InvoiceCode_PK;

                         vUpdateQuery = "UPDATE Inv_ItemReceive SET IsInvoiced = 1 " +
                    "WHERE ReceiveCode = '" + objSupplierInvoiceLine.ReceiveCode + "' and [ActionType] <> 'Delete'";

                    }
                    vQueryList.Add(vUpdateQuery);
                    vQueryList.Add(GetQuery(objSupplierInvoiceLine));

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
                        //command.CommandText = vUpdateQuery;
                        //vResult1 = command.ExecuteNonQuery();

                    }
                    if (vResult > 0)
                    {
                        trans.Commit();
                        vOut = objSupplierInvoice.InvoiceNo;
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
        public List<SupplierInvoice> GetSupplierInvoices(string pDateFrom, string pDateTo,string suppliercode)
        {
            List<SupplierInvoice> supplierInvoices = new List<SupplierInvoice>();
            SupplierInvoice objSupplierInvoice;

            string vComTxt = @"SELECT  [InvoiceCode]
                                          ,[InvoiceNo]
                                          ,[InvoiceDate]
                                          ,[InvoiceTotal]
                                      FROM [PMS_SupplierInvoice] q
                                      WHERE [SupplierCode]= '" + suppliercode + "'";

            if (!string.IsNullOrEmpty(pDateFrom) && !string.IsNullOrEmpty(pDateTo))
            {
                vComTxt = vComTxt + @" and [InvoiceDate] BETWEEN '" + pDateFrom + @"' AND '" + pDateTo + "'";
            }
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            dr = objDbCommand.ExecuteReader();
            while (dr.Read())
            {
                objSupplierInvoice = new SupplierInvoice();
                objSupplierInvoice.InvoiceCode_PK = dr["InvoiceCode"].ToString();
                objSupplierInvoice.InvoiceNo= dr["InvoiceNo"].ToString();
                objSupplierInvoice.InvoiceDate= dr.GetDateTime(dr.GetOrdinal("InvoiceDate")).ToString("dd-MM-yyyy");
                objSupplierInvoice.InvoiceTotal= Convert.ToDecimal(dr["InvoiceTotal"].ToString());

                supplierInvoices.Add(objSupplierInvoice);
            }
            return supplierInvoices;
        }
    }
}
