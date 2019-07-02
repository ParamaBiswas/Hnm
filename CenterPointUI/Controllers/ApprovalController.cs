using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CenterPointUI.Controllers
{
    public class ApprovalController: Controller
    {
        public IActionResult ApprovalSelection()
        {
            return View();
        }
        public IActionResult GetData(string pDynColumns, string pTitle, string pURL, string pColumns)
        {
            ViewBag.Title = pTitle;
            ViewBag.Columns = pColumns;
            ViewBag.ColumnNames = pDynColumns;
            ViewBag.URL = pURL;
            return PartialView("_ApprovalPopUp");

        }
        
         public IActionResult GetDataApproval(string pDynColumns, string pTitle, string pURL)
        {
            ViewBag.Title = pTitle;
          //  ViewBag.Columns = pColumns;
            ViewBag.ColumnNames = pDynColumns;
            ViewBag.URL = pURL;
            return PartialView("PopUpForApprovalLevel");

        }
        public IActionResult ApprovalDashboard()
        {
            return View();
        }
    }
}
