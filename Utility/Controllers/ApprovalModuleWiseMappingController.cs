using System;
using System.Collections.Generic;
using ApprovalInterface;
using ApprovalModel;
using CommonModel;
using ConnectionGateway;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Utility.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalModuleWiseMappingController : ControllerBase
    {
        IAppModuleObjectMapping _appModuleObjectMapping;
        ISupplierDbContext _supplierDbContext;

        public ApprovalModuleWiseMappingController(ISupplierDbContext supplierDbContext, IAppModuleObjectMapping appModuleObjectMapping)
        {
            _supplierDbContext = supplierDbContext;
            _appModuleObjectMapping = appModuleObjectMapping;
        }
        [HttpGet]
        [Route("GetMoudule")]
        public IActionResult GetMoudule()
        {
            List<StaticItem> objOfModuleMappingList = new List<StaticItem>();
            objOfModuleMappingList = _appModuleObjectMapping.GetModuleName();
            return Ok(new
            {
                objOfModuleMappingList
            });
        }
        [HttpPost]
        [Route("SaveAppModuleObjectMapping")]
        public IActionResult SaveAppModuleObjectMapping(ListModel objList)
        {
            foreach (AppModuleObjectMapping objIApp in objList.objIAppModuleObjectMappingList)
            {
                objIApp.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
                objIApp.ActionType = "INSERT";

            }
            string Vmsg = _appModuleObjectMapping.SaveMappedBusinessObject(objList.objIAppModuleObjectMappingList);
            return Ok(new
            {
                message = Vmsg
            });
        }
        [HttpGet]
        [Route("GetAppModuleObjectMapping")]
        public IActionResult GetAppModuleObjectMapping()
        {
            List<AppModuleObjectMapping> objIBusinessObjectDetailsList = new List<AppModuleObjectMapping>();
            objIBusinessObjectDetailsList = _appModuleObjectMapping.GetMappingBusinessObjectDetails();
            return Ok(new
            {
                objIBusinessObjectDetailsList
            });
        }
    }
}