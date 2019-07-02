using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SupplierPortal.Controllers
{
    public class SupplierController: Controller
    {
        public IActionResult basicInformationUI(string pEmailStatus = "")
        {

            return View();
        }
        public IActionResult SupplierInfoUI(string pEmailStatus = "")
        {

            return View();
        }
        public IActionResult SupplierDocuments()
        {

            return View();
        }

        public IActionResult QuotationRequisition()
        {
            return View();
        }

        public IActionResult GetData(string pDynColumns, string pTitle,string pURL, string pColumns)
        {
            ViewBag.Title = pTitle;
            ViewBag.Columns = pColumns;
            ViewBag.ColumnNames = pDynColumns;
            ViewBag.URL = pURL;


            return PartialView("_CommonPopUp");
        
        }

        public IActionResult SupplierPopup()
        {

            return View();
        }
        public IActionResult RFPPortalUI()
        {

            return View();
        }
    }
}
