using System;
using System.Collections.Generic;
using CommonModel;
using ConnectionGateway;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PMSInterface;
using PMSModel;

namespace ProcurementManagement.Controllers
{
    [Authorize]
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RFPProcessingController : ControllerBase
    {
        IRFPProcessingRepository _rfpprocessingrepository;
        ISupplierDbContext _supplierDbContext;
        IPurchaseRequsitionRepository _purchaseRequsitionRepository;
        public RFPProcessingController(ISupplierDbContext supplierDbContext, IRFPProcessingRepository  processingRepository, IPurchaseRequsitionRepository purchaseRequsitionRepository)
        {
            _supplierDbContext = supplierDbContext;
            _rfpprocessingrepository = processingRepository;
            _purchaseRequsitionRepository = purchaseRequsitionRepository;
        }
        [HttpPost]
        [Route("CreateRFPProcessing")]
        public IActionResult SaveRFPProcessing(RFProcessing objRFProcessing)
        {
            objRFProcessing.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
            string Vmsg = _rfpprocessingrepository.SaveRFPProcessing(objRFProcessing);
            return Ok(new
            {
                message = Vmsg
            });

        }
        [HttpGet]
        [Route("GetRFPProcessingList")]
        public IActionResult GetRFPProcessingList()
        {
            List<RFProcessing> rFProcessings = new List<RFProcessing>();
            rFProcessings = _rfpprocessingrepository.GetRFProcessing();
            return Ok(new
            {
                vList = rFProcessings
            });
        }
        [HttpGet]
        [Route("GetRFProcessingByCode")]
        public IActionResult GetRFProcessingByCode(string rFProcessCode)
        {
            RFProcessing objRFPProcessing = new RFProcessing();
            objRFPProcessing = _rfpprocessingrepository.GetRFProcessingByCode(rFProcessCode);
            PurchaseRequisition purchaseRequisitions = new PurchaseRequisition();
            purchaseRequisitions = _purchaseRequsitionRepository.GetRequisitionByCode(objRFPProcessing.RequisitionCode_FK);

            return Ok(new
            {
                objRFPProcessing,
                purchaseRequisitions
            });
        }

    }
}