using System;
using CommonModel;
using ConnectionGateway;
using LS.LS.ModelBiz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupplierInterface;

namespace Supplier.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        ISupplierRepository _supplierRepository;
        ISupplierDbContext _supplierDbContext;

        public SupplierController(ISupplierDbContext supplierDbContext, ISupplierRepository supplierRepository)
        {
            _supplierDbContext = supplierDbContext;
            _supplierRepository = supplierRepository;
        }
        [HttpPost]
        [Route("CreateSupplier")]
        public IActionResult SaveSupplier(SupplierInfo objsupplierInfo)
        {
            objsupplierInfo.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
            //objsupplierInfo.EnlistmentDate = CommonValidation.FormatDate(objsupplierInfo.EnlistmentDate, "yyyy-mm-dd", "dd-mm-yyyy");
            foreach (SupplierAttachment supplierAttachment in objsupplierInfo.SupplierAttachmentList_VW)
            {
                //supplierAttachment.IssueDate = CommonValidation.FormatDate(supplierAttachment.IssueDate, "yyyy-mm-dd", "dd-mm-yyyy");
                //supplierAttachment.ExpiryDate = CommonValidation.FormatDate(supplierAttachment.ExpiryDate, "yyyy-mm-dd", "dd-mm-yyyy");
                supplierAttachment.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
                supplierAttachment.ActionType = objsupplierInfo.ActionType;
                supplierAttachment.UserCode = objsupplierInfo.UserCode;
            }
            foreach (SupplierContact supplierContact in objsupplierInfo.SupplierContactList_VW)
            {
                supplierContact.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
                supplierContact.ActionType = objsupplierInfo.ActionType;
                supplierContact.UserCode = objsupplierInfo.UserCode;
            }

            foreach (SupplierBusiness supplierBusiness in objsupplierInfo.SupplierBusinessList_VW)
            {
                supplierBusiness.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
                supplierBusiness.ActionType = objsupplierInfo.ActionType;
                supplierBusiness.UserCode = objsupplierInfo.UserCode;
            }

            string Vmsg = _supplierRepository.SaveSupplier(objsupplierInfo);
            return Ok(new
            {
                message = Vmsg
            });

        }

        [HttpGet]
        [Route("GetSupplier")]
        public IActionResult GetSupplier(string suppliercode)
        {
            SupplierInfo supplierInfo = new SupplierInfo();
            supplierInfo = _supplierRepository.GetSupplierInfo(suppliercode);
            return Ok(new
            {
                supplierInfo
            });
        }
    }
}