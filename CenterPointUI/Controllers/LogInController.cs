using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;

namespace CenterPointUI.Controllers
{
    public class LogInController : Controller
    {
        private readonly IConfiguration configuration;
        public LogInController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public IActionResult LogInPageUI(string pEmailStatus="")
        {

            //  ViewData["total"]=count;
            // ViewData["total"] = 0;

            //CookieOptions option = new CookieOptions();
            //Response.Cookies.Append(
            //  "11",
            //  ".......token.....",
            //  new CookieOptions()
            //  {
            //      Expires = DateTime.Now.AddMinutes(10),
            //      IsEssential = true
            //  });
            //string cookieValueFromContext = HttpContext.Request.Cookies["1150"];

            // Response.Cookies.Append("1209", "value");
            if (pEmailStatus == "")
                pEmailStatus = "0";
            ViewBag.emailStatus = pEmailStatus;
            return View();
        }
        //public IActionResult Registering(string pName, string pPassword,string pUserName)
        //{
        //    string connectionString = configuration.GetConnectionString("DefaultConnectionString");
        //    SqlConnection connection = new SqlConnection(connectionString);
        //    connection.Open();
        //    SqlCommand command = new SqlCommand("Select count(*) from dbo.UserCore where UserName=@name", connection);
        //    command.Parameters.AddWithValue("@name", pUserName);
        //    var count = (int)command.ExecuteScalar();
        //    //bool vMSG ;
        //    if (count != 0)
        //        return View("LogInPageUI");
        //    //return Json(new
        //    //{
        //    //    vMSG = false
        //    //});

        //    command = new SqlCommand("INSERT INTO UserCore (UserName,passwordCore,Name) VALUES(@Uname,@pass,@name) ", connection);
        //    command.Parameters.AddWithValue("@name", pName);
        //    command.Parameters.AddWithValue("@Uname", pUserName);
        //    command.Parameters.AddWithValue("@pass", pPassword);
        //    command.ExecuteNonQuery();
        //    //connection.Close();
        //    //return Json(new
        //    //{

        //    //    vMSG = true
        //    //});
        //    return View("LogInPageUI");
        //}
        //public IActionResult RegisterUI()
        //{
        //    return View();
        //}
        //public IActionResult LogInTo(string pName,string pPassword)
        //{
        //    string connectionString = configuration.GetConnectionString("DefaultConnectionString");
        //    SqlConnection connection = new SqlConnection(connectionString);
        //    connection.Open();
        //    SqlCommand command = new SqlCommand("Select passwordCore from dbo.UserCore where UserName=@name", connection);
        //    command.Parameters.AddWithValue("@name", pName);
        //    string n = (string)command.ExecuteScalar();
        //    if (String.IsNullOrEmpty(n)||!String.Equals(n, pPassword))
        //        return View("LogInPageUI");
        //    string a = pName, b = pPassword;
        //    ViewData["name"] = pName;
        //    ViewData["pass"] = pPassword;
            
        //    return View("SuccessFulLogIn");
        //}
    }
}
