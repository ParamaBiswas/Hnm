using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonModel;
using ConnectionGateway;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMSInterface;
using PMSModel;

namespace ProcurementManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemReceiveController : ControllerBase
    {
        IItemReceiveRepository _itemReceiveRepository;
        ISupplierDbContext _supplierDbContext;
        public ItemReceiveController(ISupplierDbContext supplierDbContext, IItemReceiveRepository itemReceiveRepository)
        {
            _supplierDbContext = supplierDbContext;
            _itemReceiveRepository = itemReceiveRepository;
        }
        [HttpPost]
        [Route("CreateGRN")]
        public IActionResult SaveItemReceive(ItemReceive objItemReceive)
        {
            
            if (!String.IsNullOrEmpty(objItemReceive.ReceiveCode_PK))
            {
                objItemReceive.IsNew = false;
            }
            else
            {
                objItemReceive.IsNew = true;
            }
            objItemReceive.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
            objItemReceive.IsReceived = 1;
            foreach (ItemReceiveDet objItemReceiveDet in objItemReceive.ItemReceiveDet_VW)
            {
                objItemReceiveDet.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
                objItemReceiveDet.ActionType = objItemReceive.ActionType;
                objItemReceiveDet.UserCode = objItemReceive.UserCode;
                objItemReceiveDet.CompanyCode_FK = objItemReceive.CompanyCode_FK;
                
            }
            
            string Vmsg = _itemReceiveRepository.SaveItemReceive(objItemReceive);
            return Ok(new
            {
                message = Vmsg
            });
        }
        [HttpGet]
        [Route("GetItemReceiveByPO")]
        public IActionResult GetItemReceiveByPO(string pPOCode)
        {
            List<ItemReceive> vList = new List<ItemReceive>();
            vList = _itemReceiveRepository.GetItemReceiveByPO(pPOCode);
            return Ok(new
            {
                vList
            });
        }
    }
}