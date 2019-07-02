using System.Collections.Generic;
using CommonInterface;
using CommonModel;
using ConnectionGateway;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProcurementManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StaticListController : ControllerBase
    {

        IStaticListRepository _staticListRepository;
        ISupplierDbContext _supplierDbContext;

        public StaticListController(ISupplierDbContext supplierDbContext, IStaticListRepository staticListRepository)
        {
            _supplierDbContext = supplierDbContext;
            _staticListRepository = staticListRepository;
        }


        [HttpGet]
        [Route("StaticList")]
        public IActionResult GetStaticList(int pDropDownCode)
        {
            List<StaticDropDownListItem> staticDropDownList = new List<StaticDropDownListItem>();
            staticDropDownList = _staticListRepository.GetStaticDropDownListByCode(pDropDownCode);
            return Ok(new
            {
                staticDropDownList
            });

        }
    }
}