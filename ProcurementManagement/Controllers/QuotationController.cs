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
    public class QuotationController : ControllerBase
    {
        IQuotationRepository _quotationRepository;
        ISupplierDbContext _supplierDbContext;
        public QuotationController(ISupplierDbContext supplierDbContext, IQuotationRepository quotationRepository)
        {
            _supplierDbContext = supplierDbContext;
            _quotationRepository = quotationRepository;
        }
        [HttpPost]
        [Route("CreateQuotation")]
        public IActionResult SaveQuotation(Quotation objQuotation)
        {
            if (!String.IsNullOrEmpty(objQuotation.QuotationCode_PK))
            {
                objQuotation.IsNew = false;
            }
            else
            {
                objQuotation.IsNew = true;
            }
            objQuotation.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
            foreach (QuotationItem objQuotationItem in objQuotation.QuotationItemList_VW)
            {
                objQuotationItem.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
                objQuotationItem.ActionType = objQuotation.ActionType;
                objQuotationItem.UserCode = objQuotation.UserCode;
                objQuotationItem.CompanyCode_FK = objQuotation.CompanyCode_FK;
                foreach (QuotationItemSpecification quotationItemSpecification in objQuotationItem.QuotationItemSpecificationList_VW)
                {
                    quotationItemSpecification.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
                    quotationItemSpecification.ActionType = objQuotation.ActionType;
                    quotationItemSpecification.UserCode = objQuotation.UserCode;
                    quotationItemSpecification.CompanyCode_FK = objQuotation.CompanyCode_FK;
                }
            }
            foreach (QuotationCondition quotationCondition in objQuotation.QuotationConditionList_VW)
            {
                quotationCondition.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
                quotationCondition.ActionType = objQuotation.ActionType;
                quotationCondition.UserCode = objQuotation.UserCode;
                quotationCondition.CompanyCode_FK = objQuotation.CompanyCode_FK;
            }


            string Vmsg = _quotationRepository.SaveQuotation(objQuotation);
            return Ok(new
            {
                message = Vmsg
            });
        }
        [HttpGet]
        [Route("GetQuotationList")]
        public IActionResult GetQuotationList()
        {
            List<Quotation> quotations = new List<Quotation>();
            quotations = _quotationRepository.GetQuotation();
            return Ok(new
            {
                quotations
            });
        }
        [HttpGet]
        [Route("GetQuotationByCode")]
        public IActionResult GetQuotationByCode(string QuotationCode)
        {
            Quotation quotations = new Quotation();
            quotations = _quotationRepository.GetQuotationByCode(QuotationCode);
            return Ok(new
            {
                quotations
            });
        }
        [HttpGet]
        [Route("GetQuotationByRFP")]
        public IActionResult GetQuotationByRFP(string pREFProcessCode)
        {
            List<Quotation> vList = new List<Quotation>();
            vList = _quotationRepository.GetQuotationByRFP(pREFProcessCode);
            return Ok(new
            {
                vList
            });
        }
        [HttpGet]
        [Route("CompareQuotation")]
        public IActionResult CompareQuotation(string pREFProcessCode)
        {
            int isSuccess = 0;
            if (!String.IsNullOrEmpty(pREFProcessCode))
                isSuccess = _quotationRepository.CompareQuotation(pREFProcessCode);
            return Ok(new
            {
                isSuccess
            });
        }
    }
}