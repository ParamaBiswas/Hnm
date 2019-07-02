using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonModel;
using ConnectionGateway;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMSInterface;
using PMSModel;

namespace ProcurementManagement.Controllers
{
    [Authorize]
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class TermAndConditionController : ControllerBase
    {
        ITermsConditionRepository _termsConditionRepository;
        ISupplierDbContext _supplierDbContext;
        public TermAndConditionController(ISupplierDbContext supplierDbContext, ITermsConditionRepository termsConditionRepository)
        {
            _supplierDbContext = supplierDbContext;
            _termsConditionRepository = termsConditionRepository;
        }
        [HttpPost]
        [Route("SaveTermsAndCondition")]
        public IActionResult SaveTermsAndCondition(ListModel objList)
        {
            List<TermsAndCondition> objTermsAndCondition = new List<TermsAndCondition>();
            foreach (TermsAndCondition obj in objList.objTermsAndCondition)
            {
                obj.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
                if(obj.ConditionCode_PK==null)
                {
                    obj.IsNew = false;
                }
                else
                {
                    obj.IsNew = true;
                }
                objTermsAndCondition.Add(obj);
            }
            
            string Vmsg = _termsConditionRepository.SaveTermsAndCondition(objTermsAndCondition);
            return Ok(new
            {
                message = Vmsg
            });
        }
        [HttpGet]
        [Route("GetTermsAndCondition")]
        public IActionResult GetTermsAndCondition()
        {
            List<TermsAndCondition> termsAndConditions = new List<TermsAndCondition>();
            termsAndConditions = _termsConditionRepository.GetTermsAndCondition();
            return Ok(new
            {
                termsAndConditions
            });
        }
    }
}