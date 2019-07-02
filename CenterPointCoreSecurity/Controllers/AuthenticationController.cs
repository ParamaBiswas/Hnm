using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CenterPointCoreSecurity.Data;
using CenterPointCoreSecurity.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Security.Model;

namespace CenterPointCoreSecurity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private string server = "http://192.168.20.152/";
        private readonly MyConfiguration _myConfiguration;

        public AuthenticationController(MyConfiguration myConfiguration)
        {
            _myConfiguration = myConfiguration;
        }

        


        public IConfiguration Configuration { get; }

        [HttpPost]
        [Route("CreateUser")]
        public IActionResult CreateUser(UserInfo objUserInfo)
        {
            
            //string connection = _myConfiguration.DefaultConnection;
            //string connectionstring = Configuration.GetConnectionString("DefaultConnection").C;
            // _connectionString = Configuration.GetConnectionString("DefaultConnection");
            objUserInfo.UserID = Guid.NewGuid().ToString();
            objUserInfo.ActionDate = DateTime.Now.ToString();
            objUserInfo.ActionType = "INSERT";
            string Vmsg = UserInfoDC.SaveUser(objUserInfo);
            //string mailBody = null;

            if (Vmsg == "User Created Successfully")
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.IsBodyHtml = true;
                mail.From = new MailAddress("paramammu@gmail.com");
                mail.To.Add(objUserInfo.Email);
                mail.Subject = "Test Mail";
                //mailBody = "Dear Concern\nPlease Click on the below link to verify your Email <a href=\"http://192.168.20.152/CenterPointSupplier/LogIn/LoginPageUI?pEmailStatus=1\">Login</a>"+"\n<b>Regrads,</b>\nH&M";
                mail.Body = "<b>Dear Concern</b>" +
                            "<br/><br/>" +
                            "Please Click on the below link to verify your Email <a href=\"http://192.168.20.29/CoreSolution/LogIn/LoginPageUI?pEmailStatus=1\">Login</a>" +
                            "<br/><b>" +
                            "Regrads,</b>" +
                            "<br/>H&M";
                
                SmtpServer.Port = 587;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("paramammu@gmail.com", "lordofring");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                Vmsg = "Email Sent Successfully";
            }
            return Ok(new
            {
                message = Vmsg
            });
        }
        
        [HttpPost]
    [Route("Login")]
    public IActionResult Login(LoginModel objLoginModel)
    {
            //string connection = _myConfiguration.DefaultConnection;
            string vout = UserInfoDC.ValidateUser(objLoginModel.UserName, objLoginModel.Password, objLoginModel.EmailStatus);
            MyDependency myDependency = new MyDependency();
            List<RoleInfo> objRoleInfoList = myDependency.GetUserInRoles(objLoginModel.UserName);

            if (vout == "Login Successfully")
            {
                string usercode = UserInfoDC.GetUserId(objLoginModel.UserName);
                string suppliercode = UserInfoDC.GetSupplierCode(usercode);
                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authnetication"));
                var claims = new List < Claim >
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,objLoginModel.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                    };
                foreach (var userRole in objRoleInfoList)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole.RoleName));
                }


                    var token = new JwtSecurityToken(
                        issuer: "Parama",
                        audience: "Shanta",
                        expires: DateTime.Now.AddMinutes(5),
                        claims: claims,
                        signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                        );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    message = vout,
                    UserID= usercode,
                    Email= objLoginModel.UserName,
                    SupplierCode= suppliercode
                });
            }

            return Unauthorized();
    }
}
}