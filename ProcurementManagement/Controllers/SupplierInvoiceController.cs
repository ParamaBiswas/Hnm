using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonModel;
using ConnectionGateway;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMSInterface;
using PMSModel;

namespace ProcurementManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierInvoiceController : ControllerBase
    {
        ISupplierInvoiceRepository _supplierInvoiceRepository;
        ISupplierDbContext _supplierDbContext;
        public SupplierInvoiceController(ISupplierDbContext supplierDbContext, ISupplierInvoiceRepository supplierInvoiceRepository)
        {
            _supplierDbContext = supplierDbContext;
            _supplierInvoiceRepository = supplierInvoiceRepository;
        }
        [HttpPost]
        [Route("CreateInvoice")]
        public IActionResult SaveSupplierInvoice(SupplierInvoice objSupplierInvoice)
        {

            if (!String.IsNullOrEmpty(objSupplierInvoice.InvoiceCode_PK))
            {
                objSupplierInvoice.IsNew = false;
            }
            else
            {
                objSupplierInvoice.IsNew = true;
            }
            objSupplierInvoice.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
            objSupplierInvoice.InvoiceDate= CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
            foreach (SupplierInvoiceLine objSupplierInvoiceLine in objSupplierInvoice.SupplierInvoiceLine_VW)
            {
                objSupplierInvoiceLine.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
                objSupplierInvoiceLine.ActionType = objSupplierInvoice.ActionType;
                objSupplierInvoiceLine.UserCode = objSupplierInvoice.UserCode;
                objSupplierInvoiceLine.CompanyCode_FK = objSupplierInvoice.CompanyCode_FK;

            }

            string Vmsg = _supplierInvoiceRepository.SaveSupplierInvoice(objSupplierInvoice);
            return Ok(new
            {
                message = Vmsg
            });
        }

    }
}