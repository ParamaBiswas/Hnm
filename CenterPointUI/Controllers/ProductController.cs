using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CenterPointUI.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult ProductUI()
        {
            return View();
        }

        public IActionResult GetData(string pDynColumns, string pTitle, string pURL, string pColumns, string pPkCode)
        {
            ViewBag.Title = pTitle;
            ViewBag.Columns = pColumns;
            ViewBag.ColumnNames = pDynColumns;
            ViewBag.URL = pURL;
            ViewBag.Pk = pPkCode;


            return PartialView("_CommonPopUp");

        }
    }
}