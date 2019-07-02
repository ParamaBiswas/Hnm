using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonInterface;
using CommonModel;
using ConnectionGateway;
using HRInterface;
using HRModel;
using LS.General.ModelBiz;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Utility.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        IEmployeeRepository _employeeRepository;
        ISupplierDbContext _supplierDbContext;

       
        public EmployeeController(ISupplierDbContext supplierDbContext, IEmployeeRepository employeeRepository)
        {
            _supplierDbContext = supplierDbContext;
            _employeeRepository = employeeRepository;
           
        }

        [HttpGet]
        [Route("GetEmployeeAll")]
        public IActionResult GetEmployeeAll()
        {

            List<Employee> vList = new List<Employee>();
            vList = _employeeRepository.GetEmployeeAll();
            return Ok(new
            {
                vList
            });
        }
        [HttpPost]
        [Route("CreateEmployee")]
        public IActionResult SaveEmployee(Employee objEmployee)
        {
            objEmployee.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");

            if (String.IsNullOrEmpty(objEmployee.EmployeeCode_PK))
            {
                objEmployee.IsNew = true;
            }
            else
            {
                objEmployee.IsNew = false;
            }

            string Vmsg = _employeeRepository.SaveEmployee(objEmployee);
            return Ok(new
            {
                message = Vmsg
            });

        }
        [HttpGet]
        [Route("GetEmployeeByID")]
        public IActionResult GetEmployeeByID(string pEmployeeId, int pCompanyCode)
        {
            Employee objEmployee = new Employee();
            objEmployee = _employeeRepository.GetEmployeeByID(pEmployeeId, pCompanyCode);
            

            return Ok(new
            {
                objEmployee
            });
        }
        
    }
}