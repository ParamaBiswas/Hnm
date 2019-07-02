using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupplierPortal.Models;

namespace SupplierPortal.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            
            //string cookieValueFromContext = HttpContext.Request.Cookies["key2"];
           // string cookieValueFromContext = HttpContext.Request.Cookies["key2"];
            //  string MyCookieCollection = Request.Cookies;
            //Response.Cookies.Delete("11");
            //cookieValueFromContext = HttpContext.Request.Cookies["11"];
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult ReportView()
        {
            
            return View();
        }
        //public IActionResult GetReport()
        //{
        //    StiReport report = new StiReport();
        //    report.Load(StiNetCoreHelper.MapPath(this, "Reports/Report1.rdlc"));

        //    return StiNetCoreViewer.GetReportResult(this,report);
        //}
        //public IActionResult ViewerEvent()
        //{
        //    return StiNetCoreViewer.ViewerEventResult(this);
        //}
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
