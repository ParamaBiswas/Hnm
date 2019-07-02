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
    public class PurchaseOrderController : ControllerBase
    {
        IPurchaseOrderRepository _purchaseOrderrepository;
        ISupplierDbContext _supplierDbContext;
        public PurchaseOrderController(ISupplierDbContext supplierDbContext, IRFPProcessingRepository processingRepository, IPurchaseOrderRepository purchaseOrderRepository)
        {
            _supplierDbContext = supplierDbContext;
            _purchaseOrderrepository = purchaseOrderRepository;
        }
        [HttpPost]
        [Route("CreatePurchaseOrder")]
        public IActionResult SavePurchaseOrder(PurchaseOrder objPurchaseOrder)
        {
            if (!String.IsNullOrEmpty(objPurchaseOrder.PurchaseOrderCode_PK))
            {
                objPurchaseOrder.IsNew = false;
            }
            else
            {
                objPurchaseOrder.IsNew = true;
            }
            objPurchaseOrder.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
            objPurchaseOrder.OrderDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
            objPurchaseOrder.IsAcceptedBySupplier = 0;
            objPurchaseOrder.ApprovalAction = 0;
            objPurchaseOrder.IsApproved = 0;
            string Vmsg = _purchaseOrderrepository.SavePurchaseOrder(objPurchaseOrder);
            return Ok(new
            {
                message = Vmsg
            });

        }
        [HttpGet]
        [Route("GetPurchaseOrders")]
        public IActionResult GetPurchaseOrders(string pDateFrom, string pDateTo)
        {
            List<PurchaseOrder> vList = new List<PurchaseOrder>();
            vList = _purchaseOrderrepository.GetPurchaseOrders(pDateFrom, pDateTo);
            return Ok(new
            {
                vList
            });
        }
        [HttpGet]
        [Route("GetPurchaseOrderByCode")]
        public IActionResult GetPurchaseOrderByCode(string PurchaseOrderCode)
        {
            PurchaseOrder vList = new PurchaseOrder();
            vList = _purchaseOrderrepository.GetPurchaseOrderByCode(PurchaseOrderCode);
            return Ok(new
            {
                vList
            });
        }

    }
}