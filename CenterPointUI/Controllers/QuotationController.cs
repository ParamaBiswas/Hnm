using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CenterPointUI.Controllers
{
    public class QuotationController : Controller
    {
        public IActionResult QuotationComparison()
        {
            return View();
        }
        public IActionResult QuotationRequisition(string RFProcessCode)
        {
            ViewBag.RFProcessCode = RFProcessCode;
            return View();
        }
    }
}