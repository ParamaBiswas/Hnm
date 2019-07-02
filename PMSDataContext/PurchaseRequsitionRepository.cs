using ApprovalDataContext;
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
    public class PurchaseRequsitionRepository : IPurchaseRequsitionRepository
    {
        ISupplierDbContext _supplierDbContext;
        ICRUD _cRUD;
        IIDGenCriteriaInfo _iIDGenCriteriaInfo;
        public PurchaseRequsitionRepository(ISupplierDbContext supplierDbContext, ICRUD cRUD, IIDGenCriteriaInfo iDGenCriteriaInfo)
        {
            _supplierDbContext = supplierDbContext;
            _cRUD = cRUD;
            _iIDGenCriteriaInfo = iDGenCriteriaInfo;
        }
        static readonly string PMS_PurchaseRequisition_TBL = "PMS_PurchaseRequisition";
        static readonly string PMS_PurchaseRequisitionItem_TBL = "PMS_PurchaseRequisitionItem";
        static readonly string PMS_PurchaseReqItemSpecification_TBL = "PMS_PurchaseReqItemSpecification";
        static readonly string PMS_RequisitionTermsCondition_TBL = "PMS_PurchaseRequisitionCondition";



        public string SavePurchaseRequsition(PurchaseRequisition objPMS_PurchaseRequisition)
        {
            int vResult = 0;
            int vApprovalResult = 0;
            string vOut = "Exception Occured !";
            ArrayList vQueryList = new ArrayList();
            objPMS_PurchaseRequisition.TableName_TBL = PMS_PurchaseRequisition_TBL;
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            using (SqlTransaction trans = connection.BeginTransaction())
            {
                if (string.IsNullOrEmpty(objPMS_PurchaseRequisition.RequisitionCode_PK))
                {
                    objPMS_PurchaseRequisition.RequisitionCode_PK = Guid.NewGuid().ToString();
                    objPMS_PurchaseRequisition.RequisitionID = _iIDGenCriteriaInfo.GenerateID(trans, objPMS_PurchaseRequisition, EnumIdCategory.PurchaseRequsition);
                }
                vQueryList.Add(GetQuery(objPMS_PurchaseRequisition));
                foreach (PurchaseRequisitionItem objPurchaseRequisitionItem in objPMS_PurchaseRequisition.PurchaseRequisitionItemList_VW)
                {
                    if (string.IsNullOrEmpty(objPurchaseRequisitionItem.RequisitionDetCode_PK))
                    {
                        objPurchaseRequisitionItem.RequisitionDetCode_PK = Guid.NewGuid().ToString();
                        objPurchaseRequisitionItem.TableName_TBL = PMS_PurchaseRequisitionItem_TBL;
                        objPurchaseRequisitionItem.RequisitionCode = objPMS_PurchaseRequisition.RequisitionCode_PK;

                    }
                    vQueryList.Add(GetQuery(objPurchaseRequisitionItem));
                    foreach(PurchaseReqItemSpecification objPurchaseReqItemSpecification in objPurchaseRequisitionItem.PurchaseReqItemSpecificationList_VW)
                    {

                        if (string.IsNullOrEmpty(objPurchaseReqItemSpecification.SpecificationCode_PK))
                        {
                            objPurchaseReqItemSpecification.SpecificationCode_PK = Guid.NewGuid().ToString();
                            objPurchaseReqItemSpecification.TableName_TBL = PMS_PurchaseReqItemSpecification_TBL;
                            objPurchaseReqItemSpecification.RequisitionDetCode = objPurchaseRequisitionItem.RequisitionDetCode_PK;

                        }
                        vQueryList.Add(GetQuery(objPurchaseReqItemSpecification));
                    }
                }
                foreach (PurchaseRequisitionCondition objPurchaseRequisitionTerms in objPMS_PurchaseRequisition.PurchaseRequisitionTermsList_VW)
                {
                    if (!string.IsNullOrEmpty(objPurchaseRequisitionTerms.ConditionCode_PK))
                    {
                        //objPurchaseRequisitionTerms.ConditionCode_PK = Guid.NewGuid().ToString();
                        objPurchaseRequisitionTerms.TableName_TBL = PMS_RequisitionTermsCondition_TBL;
                        objPurchaseRequisitionTerms.RequisitionCode = objPMS_PurchaseRequisition.RequisitionCode_PK;
                    }
                    vQueryList.Add(GetQuery(objPurchaseRequisitionTerms));
                }
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
                        vApprovalResult = AppObjectInfoMapDC.GenerateApprovalFromOtherObject(29, objPMS_PurchaseRequisition.RequisitionCode_PK, objPMS_PurchaseRequisition.UserCode, objPMS_PurchaseRequisition.CompanyCode_FK, connection, trans);
                        if (vApprovalResult > 0)
                        {
                            trans.Commit();
                            vOut = "Purchase Requsition Saved Successfully";
                        }
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

        public List<PurchaseRequisition> GetRequisition(string requisitionDate)
        {
            List<PurchaseRequisition> purchaseRequisitions = new List<PurchaseRequisition>();
            PurchaseRequisition objpurchaserequsition;

            string vComTxt = @"SELECT [RequisitionCode]
                                      ,[RequisitionID]
                                      ,[RequisitionDate]
                                      ,[RequisitionTitle]
                                      ,[RequisitionFor]
                                      ,dbo.fxn_FileName(RequisitionFor) department
                                      ,[RequisitionForType]
                                      ,[RequisitionType]
                                      ,dbo.fxn_FileName(RequisitionType) RequisitionTypeName
                                      ,[ReqProductCategory]
                                      ,dbo.fxn_FileName(ReqProductCategory)servicetype
                                      ,[Requester]
                                      ,e.employeeid
                                      ,e.name
                                      ,[IsApproved]
                                      ,[ApprovalAction]
                                      ,[Remarks]
                                      ,[PriceLock]
                                      ,(case when [PriceLock]=1 then 'Yes' else 'No' end) PriceLockname
                                  FROM [PMS_PurchaseRequisition] r
                                  join HR_Employee e on r.[Requester]=e.EmployeeCode";

            if (!String.IsNullOrEmpty(requisitionDate))
                vComTxt += " WHERE RequisitionDate='" + requisitionDate + "' ";
            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            dr = objDbCommand.ExecuteReader();
            while (dr.Read())
            {
                objpurchaserequsition = new PurchaseRequisition();
                objpurchaserequsition.RequisitionCode_PK= dr["RequisitionCode"].ToString();
                objpurchaserequsition.RequisitionID = dr["RequisitionID"].ToString();
                objpurchaserequsition.RequisitionDate = dr.GetDateTime(dr.GetOrdinal("RequisitionDate")).ToString("dd-MM-yyyy");
                objpurchaserequsition.RequisitionTitle = dr["RequisitionTitle"].ToString();
                objpurchaserequsition.RequisitionFor = Convert.ToInt16(dr["RequisitionFor"].ToString());
                objpurchaserequsition.RequisitionFor_VW = dr["department"].ToString();
                objpurchaserequsition.RequisitionForType = Convert.ToInt16(dr["RequisitionForType"].ToString());
                objpurchaserequsition.RequisitionForTypeName_VW = dr["RequisitionTypeName"].ToString();
                objpurchaserequsition.ReqProductCategory = Convert.ToInt16(dr["ReqProductCategory"].ToString());
                objpurchaserequsition.ReqProductCategory_VW = dr["servicetype"].ToString();
                objpurchaserequsition.Requester = dr["Requester"].ToString();
                objpurchaserequsition.EmployeeId_VW = dr["employeeid"].ToString();
                objpurchaserequsition.Requester_VW = dr["name"].ToString();
                objpurchaserequsition.IsApproved = Convert.ToInt32(dr["IsApproved"].ToString());
                objpurchaserequsition.ApprovalAction = Convert.ToInt32(dr["ApprovalAction"].ToString());
                objpurchaserequsition.Remarks = dr["Remarks"].ToString();
                objpurchaserequsition.PriceLock = Convert.ToInt32(dr["PriceLock"].ToString());
                objpurchaserequsition.PriceLockName_VW = dr["PriceLockname"].ToString();
                purchaseRequisitions.Add(objpurchaserequsition);
            }
            dr.Close();
            return purchaseRequisitions;
        }
        public PurchaseRequisition GetRequisitionByCode(string requisitionCode)
        {
            PurchaseRequisition objpurchaserequsition =new PurchaseRequisition();

            
            string vComTxt = @"SELECT [RequisitionCode]
                                      ,[RequisitionID]
                                      ,[RequisitionDate]
                                      ,[RequisitionTitle]
                                      ,[RequisitionFor]
                                      ,dbo.fxn_FileName(RequisitionFor) department
                                      ,[RequisitionForType]
                                      ,[RequisitionType]
                                      ,dbo.fxn_FileName(RequisitionType) RequisitionTypeName
                                      ,[ReqProductCategory]
                                      ,dbo.fxn_FileName(ReqProductCategory)servicetype
                                      ,[Requester]
                                      ,e.employeeid
                                      ,[IsApproved]
                                      ,[ApprovalAction]
                                      ,[Remarks]
                                      ,[PriceLock]
                                      ,(case when [PriceLock]=1 then 'Yes' else 'No' end) PriceLockname
                                  FROM [PMS_PurchaseRequisition] r
                                  join HR_Employee e on r.[Requester]=e.EmployeeCode
                                  WHERE [RequisitionCode]= '" + requisitionCode + "'";
            



            SqlConnection connection = _supplierDbContext.GetConn();
            connection.Open();
            SqlDataReader dr;
            SqlCommand objDbCommand = new SqlCommand(vComTxt, connection);
            
            dr = objDbCommand.ExecuteReader();
            if (dr.Read())
            {
                objpurchaserequsition = new PurchaseRequisition();
                objpurchaserequsition.RequisitionCode_PK = dr["RequisitionCode"].ToString();
                objpurchaserequsition.RequisitionID = dr["RequisitionID"].ToString();
                objpurchaserequsition.RequisitionDate = dr.GetDateTime(dr.GetOrdinal("RequisitionDate")).ToString("dd/MM/yyyy");
                objpurchaserequsition.RequisitionTitle = dr["RequisitionTitle"].ToString();
                objpurchaserequsition.RequisitionFor = Convert.ToInt16(dr["RequisitionFor"].ToString());
                objpurchaserequsition.RequisitionFor_VW = dr["department"].ToString();
                objpurchaserequsition.RequisitionForType = Convert.ToInt16(dr["RequisitionForType"].ToString());
                objpurchaserequsition.RequisitionForTypeName_VW = dr["RequisitionTypeName"].ToString();
                objpurchaserequsition.ReqProductCategory = Convert.ToInt16(dr["ReqProductCategory"].ToString());
                objpurchaserequsition.ReqProductCategory_VW = dr["servicetype"].ToString();
                objpurchaserequsition.Requester = dr["Requester"].ToString();
                objpurchaserequsition.EmployeeId_VW = dr["employeeid"].ToString();
                objpurchaserequsition.IsApproved = Convert.ToInt32(dr["IsApproved"].ToString());
                objpurchaserequsition.ApprovalAction = Convert.ToInt32(dr["ApprovalAction"].ToString());
                objpurchaserequsition.Remarks = dr["Remarks"].ToString();
                objpurchaserequsition.PriceLock = Convert.ToInt32(dr["PriceLock"].ToString());
                objpurchaserequsition.PriceLockName_VW = dr["PriceLockname"].ToString();
                
            }

            dr.Close();
            objpurchaserequsition.PurchaseRequisitionItemList_VW = GetRequisitionItem(requisitionCode, connection);
            objpurchaserequsition.PurchaseRequisitionTermsList_VW = GetRequsitionCondition(requisitionCode, connection);
            
            return objpurchaserequsition;
        }

        public List<PurchaseRequisitionItem> GetRequisitionItem(string requisitionCode,SqlConnection connection)
        {
            List<PurchaseRequisitionItem> objPurchaseRequisitionItemList_VW = new List<PurchaseRequisitionItem>();
            
            string vComTxt1 = @"SELECT  [RequisitionDetCode]
                                        ,A.[ProductCode]
	                                    ,B.productId
	                                    ,B.productName
                                        ,[Quantity]
                                        ,QunatityMOUCode
                                        ,dbo.fxn_FileName(QunatityMOUCode)  as  Mou
                                        ,isnull([ProductType],0)ProductType
                                        ,dbo.fxn_FileName(ProductType) productcategory
                                        FROM [PMS_PurchaseRequisitionItem]  A join pm_product B on A.productcode = B.productCode
                                        --and A.CompanyCode = B.CompanyCode
                                        WHERE A.[RequisitionCode]= '" + requisitionCode + "'";
            
            SqlCommand objDbCommand1 = new SqlCommand(vComTxt1, connection);
            SqlDataReader dr;
            dr = objDbCommand1.ExecuteReader();
            while (dr.Read())
            {
                PurchaseRequisitionItem obj = new PurchaseRequisitionItem();
                obj.RequisitionDetCode_PK = dr["RequisitionDetCode"].ToString();
                obj.ProductCode = dr["ProductCode"].ToString();
                obj.ProductID_VW = dr["productId"].ToString();
                obj.ProductName_VW = dr["productName"].ToString();
                obj.QunatityMOUCode = Convert.ToInt16(dr["QunatityMOUCode"].ToString());
                obj.QunatityMOU_VW = dr["Mou"].ToString();
                obj.Quantity = Convert.ToDecimal(dr["Quantity"].ToString());
                obj.ProductType = Convert.ToInt16(dr["ProductType"].ToString());
                obj.ProductCatagory_VW = dr["productcategory"].ToString();
                obj.PurchaseReqItemSpecificationList_VW = GetItemSpecifications(requisitionCode, connection,dr);
                objPurchaseRequisitionItemList_VW.Add(obj);

            }
            dr.Close();
            return objPurchaseRequisitionItemList_VW;

        }
        public List<PurchaseReqItemSpecification> GetItemSpecifications(string requisitionCode, SqlConnection connection, SqlDataReader dr)
        {
            List<PurchaseReqItemSpecification> purchaseReqItemSpecifications = new List<PurchaseReqItemSpecification>();
            string vComTxt2 = @"select B.SpecificationCode,B.ProductSpecification
                                from  PMS_PurchaseRequisitionItem A join PMS_PurchaseReqItemSpecification B
                                on A.RequisitionDetCode = B.RequisitionDetCode and A.CompanyCode=B.CompanyCode
                                WHERE A.[RequisitionCode]= '" + requisitionCode + "'";
            SqlCommand objDbCommand2 = new SqlCommand(vComTxt2, connection);
            
            dr = objDbCommand2.ExecuteReader();
            while (dr.Read())
            {
                PurchaseReqItemSpecification obj = new PurchaseReqItemSpecification();
                obj.SpecificationCode_PK = dr["SpecificationCode"].ToString();
                obj.ProductSpecification = dr["ProductSpecification"].ToString();

                purchaseReqItemSpecifications.Add(obj);

            }
            return purchaseReqItemSpecifications;

        }
        public List<PurchaseRequisitionCondition> GetRequsitionCondition(string requisitionCode, SqlConnection connection)
        {
            List<PurchaseRequisitionCondition> objPurchaseRequisitionTermsList_VW = new List<PurchaseRequisitionCondition>();
            string vComTxt2 = @"SELECT c.[ConditionCode]
                                      ,t.[Condition]
                                      ,[ConSLNo]
                                      ,[Remarks]
                                      ,[RequisitionCode]
                                  FROM [PMS_PurchaseRequisitionCondition] c
                                  join PMS_TermsAndCondition t on c.ConditionCode=t.ConditionCode 
                                 WHERE [RequisitionCode]= '" + requisitionCode + "'";
            SqlCommand objDbCommand3 = new SqlCommand(vComTxt2, connection);
            SqlDataReader dr;
            dr = objDbCommand3.ExecuteReader();
            while (dr.Read())
            {
                PurchaseRequisitionCondition obj = new PurchaseRequisitionCondition();
                obj.ConditionCode_PK = dr["ConditionCode"].ToString();
                obj.Condition_VW = dr["Condition"].ToString();
                obj.ConSLNo = Convert.ToInt32(dr["ConSLNo"].ToString());
                obj.Remarks = dr["Remarks"].ToString();
                objPurchaseRequisitionTermsList_VW.Add(obj);

            }
            dr.Close();
            return objPurchaseRequisitionTermsList_VW;

        }
        public string ApproveActionUpdate(string prequisitionCode, int pApprovalAction, bool pIsApproved, SqlConnection objDbConnection, SqlTransaction objDbTransaction)
        {
            string vOut = string.Empty;
            int vResult = 0;
            string vUpdateQuery = string.Empty;
            string vComTxt = string.Empty;
            string vUpdateSOStatus = string.Empty;



            try
            {
                PurchaseRequisition objPurchaseRequisition = new PurchaseRequisition();
                objPurchaseRequisition = GetRequisitionByCode(prequisitionCode);

                if (pIsApproved == true)
                {
                    vUpdateQuery = "UPDATE " + PMS_PurchaseRequisition_TBL +
                                      " SET ApprovalAction = " + pApprovalAction +
                                      ",  IsApproved = 1 " +
                                     " WHERE RequisitionCode = '" + prequisitionCode + "' and [ActionType] <> 'Delete'";

                }

                else
                {

                    vUpdateQuery = "UPDATE " + PMS_PurchaseRequisition_TBL + " SET ApprovalAction = " + pApprovalAction + " "
                                          + " WHERE RequisitionCode = '" + prequisitionCode + "' and [ActionType] <> 'Delete' ";

                }
                using (SqlCommand command = new SqlCommand("", objDbConnection, objDbTransaction))
                {
                    command.CommandText = vUpdateQuery;
                    vResult = command.ExecuteNonQuery();
                }

                if (vResult > 0)
                {
                    //objDbTransaction.Commit();

                    vOut = EnumMessageId.LS101.ToString();
                }
                else
                {
                    //objDbTransaction.Rollback();
                    vOut = EnumMessageId.LS200.ToString();

                }
            }
            catch (DbException ex)
            {
                //objDbTransaction.Rollback();
                vOut = EnumMessageId.LS251.ToString() + ": \n " + ex.Message;
            }
            finally
            {
                //objDbConnection.Close();
            }


            return vOut;

        }
    }
}
