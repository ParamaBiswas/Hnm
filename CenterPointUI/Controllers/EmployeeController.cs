using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CenterPCore.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult CreateEmployee()
        {
            return View();
        }


        public IActionResult GetData(string pDynColumns, string pTitle, string pURL, string pColumns, string pListName)
        {
            ViewBag.Title = pTitle;
            ViewBag.ListName = pListName;
            ViewBag.Columns = pColumns;
            ViewBag.ColumnNames = pDynColumns;
            ViewBag.URL = pURL;


            return PartialView("_CommonPopUp");

        }
    }
}