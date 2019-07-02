using System;
using System.Collections.Generic;
using CommonModel;
using ConnectionGateway;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMSInterface;
using PMSModel;

namespace ProcurementManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseRequsitionController : ControllerBase
    {
        IPurchaseRequsitionRepository _purchaseRequsitionRepository;
        ISupplierDbContext _supplierDbContext;
        public PurchaseRequsitionController(ISupplierDbContext supplierDbContext, IPurchaseRequsitionRepository purchaseRequsitionRepository)
        {
            _supplierDbContext = supplierDbContext;
            _purchaseRequsitionRepository = purchaseRequsitionRepository;
        }
        [HttpPost]
        [Route("CreatePurchaseRequsition")]
        public IActionResult SavePurchaseRequsition(PurchaseRequisition objPMS_PurchaseRequisition)
        {
            string Vmsg = string.Empty;
            objPMS_PurchaseRequisition.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
            objPMS_PurchaseRequisition.RequisitionDate = CommonValidation.FormatDate(objPMS_PurchaseRequisition.RequisitionDate, "yyyy-mm-dd", "dd-mm-yyyy");
            if (objPMS_PurchaseRequisition.PurchaseRequisitionItemList_VW.Count > 0)
            {
                foreach (PurchaseRequisitionItem objPurchaseRequisitionItem in objPMS_PurchaseRequisition.PurchaseRequisitionItemList_VW)
                {
                    objPurchaseRequisitionItem.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
                    objPurchaseRequisitionItem.ActionType = objPMS_PurchaseRequisition.ActionType;
                    objPurchaseRequisitionItem.UserCode = objPMS_PurchaseRequisition.UserCode;
                    objPurchaseRequisitionItem.CompanyCode_FK = objPMS_PurchaseRequisition.CompanyCode_FK;
                    if (objPurchaseRequisitionItem.PurchaseReqItemSpecificationList_VW.Count > 0)
                    {
                        foreach (PurchaseReqItemSpecification purchaseReqItemSpecification in objPurchaseRequisitionItem.PurchaseReqItemSpecificationList_VW)
                        {
                            purchaseReqItemSpecification.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
                            purchaseReqItemSpecification.ActionType = objPMS_PurchaseRequisition.ActionType;
                            purchaseReqItemSpecification.UserCode = objPMS_PurchaseRequisition.UserCode;
                            purchaseReqItemSpecification.CompanyCode_FK = objPMS_PurchaseRequisition.CompanyCode_FK;
                        }
                    }
                    else
                    {
                        Vmsg = "Please enter product specfication!";
                    }
                }
            }
            else
            {
                Vmsg = "Please enter product!";
            }
            if (objPMS_PurchaseRequisition.PurchaseRequisitionTermsList_VW.Count > 0)
            {
                foreach (PurchaseRequisitionCondition purchaseRequisitionCondition in objPMS_PurchaseRequisition.PurchaseRequisitionTermsList_VW)
                {
                    purchaseRequisitionCondition.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
                    purchaseRequisitionCondition.ActionType = objPMS_PurchaseRequisition.ActionType;
                    purchaseRequisitionCondition.UserCode = objPMS_PurchaseRequisition.UserCode;
                    purchaseRequisitionCondition.CompanyCode_FK = objPMS_PurchaseRequisition.CompanyCode_FK;
                }
            }
            else
            {
                Vmsg = "Please enter Terms and Condition!";
            }

            if (Vmsg == "")
            {

                Vmsg = _purchaseRequsitionRepository.SavePurchaseRequsition(objPMS_PurchaseRequisition);
            }
            
            return Ok(new
            {
                message = Vmsg
            });
        }
        [HttpGet]
        [Route("GetRequisitionList")]
        public IActionResult GetRequisitionList(string requisitionDate)
        {
            string reqDate=CommonValidation.FormatDate(requisitionDate, "yyyy-mm-dd", "dd-mm-yyyy");
            List<PurchaseRequisition> purchaseRequisitions = new List<PurchaseRequisition>();
            purchaseRequisitions = _purchaseRequsitionRepository.GetRequisition(reqDate);
            return Ok(new
            {
                purchaseRequisitions
            });
        }
        [HttpGet]
        [Route("GetRequisitionByCode")]
        public IActionResult GetRequisitionByCode(string requisitionCode)
        {
            PurchaseRequisition purchaseRequisitions = new PurchaseRequisition();
            purchaseRequisitions = _purchaseRequsitionRepository.GetRequisitionByCode(requisitionCode);
            return Ok(new
            {
                purchaseRequisitions
            });
        }
    }
}