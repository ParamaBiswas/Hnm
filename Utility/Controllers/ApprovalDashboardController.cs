using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApprovalInterface;
using ApprovalModel;
using CommonInterface;
using CommonModel;
using ConnectionGateway;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMSInterface;
using SupplierInterface;

namespace Utility.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalDashboardController : ControllerBase
    {
        IGeneralWaitingForApproval _generalWaitingForApproval;
        ISupplierDbContext _supplierDbContext;
        
        public ApprovalDashboardController(ISupplierDbContext supplierDbContext, IGeneralWaitingForApproval generalWaitingForApproval)
        {
            _supplierDbContext = supplierDbContext;
            _generalWaitingForApproval = generalWaitingForApproval;
        }
        [HttpGet]
        [Route("GetDashBoardData")]
        public IActionResult GetDashBoardData(string pUserId)
        {
            List<GeneralWaitingForApproval> vList = new List<GeneralWaitingForApproval>();
            vList = _generalWaitingForApproval.GetDashBoardData(pUserId);
            return Ok(new
            {
                vList
            });
        }
        [HttpGet]
        [Route("GetObjectListWithDataForApproval")]
        public IActionResult GetObjectListWithDataForApproval(string pModuleObjMapCode, int pApprovalStatus, string pFromDate, string pToDate, string pUserCode)
        {
            AppObjectInfoMap objAppObjectInfoMap = new AppObjectInfoMap();
            objAppObjectInfoMap = _generalWaitingForApproval.GetAppObjectInfoMapByMapCode(pModuleObjMapCode);
            List<AppObjectInfoMap> vList = new List<AppObjectInfoMap>();
            vList = _generalWaitingForApproval.GetObjectListWithDataForApproval(pModuleObjMapCode, objAppObjectInfoMap, pApprovalStatus, pFromDate, pToDate, pUserCode);
            return Ok(new
            {
                vList
            });
        }
        [HttpPost]
        [Route("SaveApproval")]
        public IActionResult SaveApprovalData(ListModel objList)
        {
            //foreach (GeneralWaitingForApproval objIApp in objList.objIGeneralWaitingForApprovalList)
            //{
            //    objIApp.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
            //    objIApp.ActionType = "INSERT";

            //}
            string Vmsg = _generalWaitingForApproval.SaveApprovalData(objList.objIGeneralWaitingForApprovalList);
            return Ok(new
            {
                message = Vmsg
            });
        }
    }
}