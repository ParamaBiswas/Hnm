using CommonInterface;
using CommonModel;
using ConnectionGateway;
using LSCrud;
using PMSInterface;
using PMSModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace PMSDataContext
{
    public class QuotationRepository : IQuotationRepository
    {
        ISupplierDbContext _supplierDbContext;
        ICRUD _cRUD;
        IIDGenCriteriaInfo _iIDGenCriteriaInfo;
        public QuotationRepository(ISupplierDbContext supplierDbContext, ICRUD cRUD, IIDGenCriteriaInfo iDGenCriteriaInfo)
        {
            _supplierDbContext = supplierDbContext;
            _cRUD = cRUD;
            _iIDGenCriteriaInfo = iDGenCriteriaInfo;
        }
        static readonly string LSP_PMS_Quotation_TBL = "LSP_PMS_Quotation";
        static readonly string LSP_PMS_QuotationItem_TBL = "LSP_PMS_QuotationItem";
        static readonly string LSP_PMS_QuotationItemSpecification_TBL = "LSP_PMS_QuotationItemSpecification";
        static readonly string LSP_PMS_QuotationCondition_TBL = "LSP_PMS_QuotationCondition";

        public string SaveQuotation(Quotation objQuotation)
        {
            int vResult = 0;
            string vOut = "Exception Occured !";
            ArrayList vQueryList = new ArrayList();
            objQuotation.TableName_TBL = LSP_PMS_Quotation_TBL;
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            using (SqlTransaction trans = connection.BeginTransaction())
            {
                if (string.IsNullOrEmpty(objQuotation.QuotationCode_PK))
                {
                    objQuotation.QuotationCode_PK = Guid.NewGuid().ToString();
                    objQuotation.QuotationNo = _iIDGenCriteriaInfo.GenerateID(trans, objQuotation, EnumIdCategory.QuotationNo);
                }
                vQueryList.Add(GetQuery(objQuotation));
                foreach (QuotationItem objQuotationItem in objQuotation.QuotationItemList_VW)
                {
                    if (string.IsNullOrEmpty(objQuotationItem.QuotationDetCode_PK))
                    {
                        objQuotationItem.QuotationDetCode_PK = Guid.NewGuid().ToString();
                        objQuotationItem.TableName_TBL = LSP_PMS_QuotationItem_TBL;
                        objQuotationItem.QuotationCode = objQuotation.QuotationCode_PK;

                    }
                    vQueryList.Add(GetQuery(objQuotationItem));
                    foreach (QuotationItemSpecification objQuotationItemSpecification in objQuotationItem.QuotationItemSpecificationList_VW)
                    {
                        if (string.IsNullOrEmpty(objQuotationItemSpecification.QuotationItemSpecificationCode_PK))
                        {
                            objQuotationItemSpecification.QuotationItemSpecificationCode_PK = Guid.NewGuid().ToString();
                            objQuotationItemSpecification.TableName_TBL = LSP_PMS_QuotationItemSpecification_TBL;
                            objQuotationItemSpecification.QuotationDetCode_FK = objQuotationItem.QuotationDetCode_PK;

                        }
                        vQueryList.Add(GetQuery(objQuotationItemSpecification));
                    }
                }
                foreach (QuotationCondition objQuotationCondition in objQuotation.QuotationConditionList_VW)
                {
                    if (!string.IsNullOrEmpty(objQuotationCondition.ConditionCode_PK))
                    {
                        objQuotationCondition.TableName_TBL = LSP_PMS_QuotationCondition_TBL;
                        objQuotationCondition.QuotationCode = objQuotation.QuotationCode_PK;
                    }
                    vQueryList.Add(GetQuery(objQuotationCondition));
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
                        vOut = "Quotation Saved Successfully";
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
        public List<Quotation> GetQuotation()
        {
            List<Quotation> quotations = new List<Quotation>();
            Quotation objQuotation;

            string vComTxt = @"SELECT  [QuotationCode]
                                      ,[QuotationNo]
                                      ,[RFProcessCode]
                                      ,[QuotationDate]
                                      ,q.[SupplierCode]
                                      ,s.SupplierName
                                      ,s.SupplierID
                                      ,[Remarks]
                                      ,q.[IsApproved]
                                      ,q.[ApprovalAction]
                                      ,[TotatlQuotationScore]
                                      ,[IsFinalSelection]
                                  FROM [dbo].[LSP_PMS_Quotation] q
                                  join LSP_PMS_SupplierInfo s on s.SupplierCode=q.SupplierCode";


            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            dr = objDbCommand.ExecuteReader();
            while (dr.Read())
            {
                objQuotation = new Quotation();
                objQuotation.QuotationCode_PK = dr["QuotationCode"].ToString();
                objQuotation.QuotationNo = dr["QuotationNo"].ToString();
                objQuotation.RFProcessCode = dr["RFProcessCode"].ToString();
                objQuotation.QuotationDate = dr.GetDateTime(dr.GetOrdinal("QuotationDate")).ToString("dd-MM-yyyy");
                objQuotation.SupplierCode = dr["SupplierCode"].ToString();
                objQuotation.SupplierID_VW = dr["SupplierID"].ToString();
                objQuotation.SupplierName_VW = dr["SupplierName"].ToString();
                objQuotation.Remarks = dr["Remarks"].ToString();
                objQuotation.IsApproved = Convert.ToInt16(dr["IsApproved"].ToString());
                objQuotation.ApprovalAction = Convert.ToInt16(dr["ApprovalAction"].ToString());
                objQuotation.TotatlQuotationScore = Convert.ToDecimal(dr["TotatlQuotationScore"].ToString());
                objQuotation.IsFinalSelection = Convert.ToInt16(dr["IsFinalSelection"].ToString());

                quotations.Add(objQuotation);
            }
            dr.Close();
            return quotations;
        }
        public Quotation GetQuotationByCode(string QuotationCode)
        {
            Quotation objQuotation = new Quotation();


            string vComTxt = @"SELECT  [QuotationCode]
                                      ,[QuotationNo]
                                      ,[RFProcessCode]
                                      ,[QuotationDate]
                                      ,q.[SupplierCode]
                                      ,s.SupplierName
                                      ,s.SupplierID
                                      ,[Remarks]
                                      ,q.[IsApproved]
                                      ,q.[ApprovalAction]
                                      ,[TotatlQuotationScore]
                                      ,[IsFinalSelection]
                                  FROM [dbo].[LSP_PMS_Quotation] q
                                  join LSP_PMS_SupplierInfo s on s.SupplierCode=q.SupplierCode
                                  WHERE [QuotationCode]= '" + QuotationCode + "'";

            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);

            dr = objDbCommand.ExecuteReader();
            if (dr.Read())
            {
                objQuotation = new Quotation();
                objQuotation.QuotationCode_PK = dr["QuotationCode"].ToString();
                objQuotation.QuotationNo = dr["QuotationNo"].ToString();
                objQuotation.RFProcessCode = dr["RFProcessCode"].ToString();
                objQuotation.QuotationDate = dr.GetDateTime(dr.GetOrdinal("QuotationDate")).ToString("dd-MM-yyyy");
                objQuotation.SupplierCode = dr["SupplierCode"].ToString();
                objQuotation.SupplierID_VW = dr["SupplierID"].ToString();
                objQuotation.SupplierName_VW = dr["SupplierName"].ToString();
                objQuotation.Remarks = dr["Remarks"].ToString();
                objQuotation.IsApproved = Convert.ToInt16(dr["IsApproved"].ToString());
                objQuotation.ApprovalAction = Convert.ToInt16(dr["ApprovalAction"].ToString());
                objQuotation.TotatlQuotationScore = Convert.ToDecimal(dr["TotatlQuotationScore"].ToString());
                objQuotation.IsFinalSelection = Convert.ToInt16(dr["IsFinalSelection"].ToString());
            }

            dr.Close();
            objQuotation.QuotationItemList_VW = GetQuotationItem(QuotationCode, connection);
            objQuotation.QuotationConditionList_VW = GetQuotationCondition(QuotationCode, connection);

            return objQuotation;
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
                obj.QuotationItemSpecificationList_VW = GetItemSpecifications(QuotationCode, connection, dr);
                objQuotationItemList_VW.Add(obj);

            }
            dr.Close();
            return objQuotationItemList_VW;

        }
        public List<QuotationItemSpecification> GetItemSpecifications(string QuotationCode, SqlConnection connection, SqlDataReader dr)
        {
            List<QuotationItemSpecification> quotationItemSpecifications = new List<QuotationItemSpecification>();
            string vComTxt2 = @"SELECT  q.[QuotationDetCode]
                                          ,q.[ProductCode]
                                          ,q.[SpecificationCode]
                                          ,[QuotationItemSpecificationCode]
                                          ,[IsSatisfied]
	                                      ,s.ProductSpecification
                                      FROM [dbo].[LSP_PMS_QuotationItemSpecification] q
                                      join PMS_PurchaseReqItemSpecification s on s.SpecificationCode=q.SpecificationCode
                                      join LSP_PMS_QuotationItem i on i.QuotationDetCode=q.QuotationDetCode
                                WHERE i.[QuotationCode]= '" + QuotationCode + "'";
            SqlCommand objDbCommand2 = new SqlCommand(vComTxt2, connection);

            dr = objDbCommand2.ExecuteReader();
            while (dr.Read())
            {
                QuotationItemSpecification obj = new QuotationItemSpecification();
                obj.QuotationDetCode_FK = dr["SpecificationCode"].ToString();
                obj.ProductCode = dr["SpecificationCode"].ToString();
                obj.SpecificationCode = dr["SpecificationCode"].ToString();
                obj.QuotationItemSpecificationCode_PK = dr["SpecificationCode"].ToString();
                obj.ProductSpecification_VW = dr["ProductSpecification"].ToString();
                obj.IsSatisfied = Convert.ToInt16(dr["IsSatisfied"].ToString());
                quotationItemSpecifications.Add(obj);

            }
            return quotationItemSpecifications;

        }
        public List<QuotationCondition> GetQuotationCondition(string QuotationCode, SqlConnection connection)
        {
            List<QuotationCondition> objQuotationConditionList_VW = new List<QuotationCondition>();
            string vComTxt2 = @"SELECT [QuotationCode]
                                      ,q.[ConditionCode]
                                      ,[ConSLNo]
                                      ,[Remarks]
                                      ,[IsSatisfied]
                                      ,t.[ConditionValue]
                                      ,[AchivedValue]
	                                  ,t.Condition
                                  FROM [dbo].[LSP_PMS_QuotationCondition] q
                                  join PMS_TermsAndCondition t on t.ConditionCode=q.ConditionCode
                                 WHERE [QuotationCode]= '" + QuotationCode + "'";
            SqlCommand objDbCommand3 = new SqlCommand(vComTxt2, connection);
            SqlDataReader dr;
            dr = objDbCommand3.ExecuteReader();
            while (dr.Read())
            {
                QuotationCondition obj = new QuotationCondition();
                obj.QuotationCode = dr["QuotationCode"].ToString();
                obj.ConditionCode_PK = dr["ConditionCode"].ToString();
                obj.Condition_VW = dr["Condition"].ToString();
                obj.ConSLNo = Convert.ToInt32(dr["ConSLNo"].ToString());
                obj.Remarks = dr["Remarks"].ToString();
                obj.IsSatisfied = Convert.ToInt16(dr["IsSatisfied"].ToString());
                obj.ConditionValue = Convert.ToDecimal(dr["ConditionValue"].ToString());
                obj.AchivedValue = Convert.ToDecimal(dr["AchivedValue"].ToString());
                objQuotationConditionList_VW.Add(obj);

            }
            dr.Close();
            return objQuotationConditionList_VW;

        }
        public List<Quotation> GetQuotationByRFP(string pREFProcessCode)
        {
            List<Quotation> quotations = new List<Quotation>();
            Quotation objQuotation;

            string vComTxt = @"SELECT  [QuotationCode]
                                          ,[QuotationNo]
                                          ,q.[RFProcessCode]
                                          ,p.[RequisitionCode]
                                          ,[QuotationDate]
                                          ,q.[SupplierCode]
                                          ,s.SupplierName
                                          ,s.SupplierID
                                          ,q.[Remarks]
                                          ,q.[IsApproved]
                                          ,q.[ApprovalAction]
                                          ,[TotatlQuotationScore]
                                          ,[IsFinalSelection]
                                      FROM [dbo].[LSP_PMS_Quotation] q
                                      join [dbo].[PMS_RFProcessing] p
                                      on q.RFProcessCode=p.RFProcessCode
                                      join LSP_PMS_SupplierInfo s on s.SupplierCode=q.SupplierCode
                                      WHERE q.[RFProcessCode]= '" + pREFProcessCode + "'";


            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            dr = objDbCommand.ExecuteReader();
            while (dr.Read())
            {
                objQuotation = new Quotation();
                objQuotation.QuotationCode_PK = dr["QuotationCode"].ToString();
                objQuotation.QuotationNo = dr["QuotationNo"].ToString();
                objQuotation.RFProcessCode = dr["RFProcessCode"].ToString();
                objQuotation.RequisitionCode_VW = dr["RequisitionCode"].ToString();
                objQuotation.QuotationDate = dr.GetDateTime(dr.GetOrdinal("QuotationDate")).ToString("dd-MM-yyyy");
                objQuotation.SupplierCode = dr["SupplierCode"].ToString();
                objQuotation.SupplierID_VW = dr["SupplierID"].ToString();
                objQuotation.SupplierName_VW = dr["SupplierName"].ToString();
                objQuotation.Remarks = dr["Remarks"].ToString();
                objQuotation.IsApproved = Convert.ToInt16(dr["IsApproved"].ToString());
                objQuotation.ApprovalAction = Convert.ToInt16(dr["ApprovalAction"].ToString());
                objQuotation.TotatlQuotationScore = Convert.ToDecimal(dr["TotatlQuotationScore"].ToString());
                objQuotation.IsFinalSelection = Convert.ToInt16(dr["IsFinalSelection"].ToString());

                quotations.Add(objQuotation);
            }
            dr.Close();
            return quotations;
        }
        public int CompareQuotation(string RFProcessCode)
        {
            int vSuccess = 0;
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            using (SqlTransaction trans = connection.BeginTransaction())
            {

                try
                {
                    SqlCommand command = new SqlCommand("uspLSP_CalculationScoreOfQuotation", connection, trans);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@RFProcessCode", RFProcessCode);
                    command.Parameters.AddWithValue("@CompanyCode", 1);
                    command.Parameters.Add("@IsSuccess", SqlDbType.Int).Direction = ParameterDirection.Output;
                    command.ExecuteNonQuery();

                        vSuccess = Convert.ToInt32(command.Parameters["@IsSuccess"].Value.ToString());
                        if (vSuccess == 1)
                        {
                            trans.Commit();
                        }
                        else
                            trans.Rollback();
                    
                }
                catch (DbException ex)
                {

                    trans.Rollback();

                    throw new ApplicationException(ex.Message);

                }
                finally
                {

                    connection.Close();

                }
                return vSuccess;
            }

        }
    }
}
