using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CommonInterface;
using CommonModel;
using ConnectionGateway;
using LS.General.ModelBiz;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupplierInterface;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;

namespace Utility.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        IGeneralCodeFile _ICommon;
        ISupplierDbContext _supplierDbContext;

        IGeneralCodeFileType _ICommonGeneralCodeFile;
        IGeneralCodeFileLevel _ICommonGeneralCodeFileLevel;
        IUserMapEmployee _IUserMapEmployee;

        public CommonController(ISupplierDbContext supplierDbContext, IGeneralCodeFile common, IGeneralCodeFileType commonCodeFileType, IGeneralCodeFileLevel commongeneralCodeFileLevel, IUserMapEmployee commonUserMapEmployee)
        {
            _supplierDbContext = supplierDbContext;
            _ICommon = common;
            _ICommonGeneralCodeFile = commonCodeFileType;
            _ICommonGeneralCodeFileLevel = commongeneralCodeFileLevel;
            _IUserMapEmployee = commonUserMapEmployee;
        }
        
        [HttpGet]
        [Route("GetCodeFile")]
        public IActionResult GetGeneralCodeFileByFileTypeNFileLevel(Int32 FileTypeCode, Int32 LevelCode, Int32 COMPANY_CODE)
        {
            List<GeneralCodeFile> listGeneralCodeFile = new List<GeneralCodeFile>();
            listGeneralCodeFile = _ICommon.GetGeneralCodeFileByFileTypeNFileLevel(FileTypeCode, LevelCode, COMPANY_CODE);
            return Ok(new
            {
                listGeneralCodeFile
            });
        }
        [HttpGet]
        [Route("GetCodeFileByParent")]
        public IActionResult GetGeneralCodeFileByFileTypeNParentFile(Int32 pFileTypeCode, Int32 pParentFileCode, Int32 COMPANY_CODE)
        {
            List<GeneralCodeFile> listGeneralCodeFile = new List<GeneralCodeFile>();
            listGeneralCodeFile = _ICommon.GetGeneralCodeFileByFileTypeNParentFile(pFileTypeCode, pParentFileCode, COMPANY_CODE);
            return Ok(new
            {
                listGeneralCodeFile
            });
        }

        //Habib [20-03-2019]
        [HttpGet]
        [Route("GetGeneralCodeFileTypeAll")]
        public IActionResult GetGeneralCodeFileTypeAll(string pModuleCode, Int32 COMPANY_CODE)
        {
            List<GeneralCodeFileType> listGeneralCodeFileType = new List<GeneralCodeFileType>();
            listGeneralCodeFileType = _ICommonGeneralCodeFile.GetGeneralCodeFileTypeAll(pModuleCode, COMPANY_CODE);
            return Ok(new
            {
                listGeneralCodeFileType
            });
        }

        //Habib [21-03-2019]
        [HttpGet]
        [Route("GetGeneralCodeFileTypeByKey")]
        public IActionResult GetGeneralCodeFileTypeByKey(Int32 FileTypeCode, Int32 COMPANY_CODE)
        {
            GeneralCodeFileType objGeneralCodeFileType = new GeneralCodeFileType();
            objGeneralCodeFileType = _ICommonGeneralCodeFile.GetGeneralCodeFileTypeByKey(FileTypeCode, COMPANY_CODE);
            return Ok(new
            {
                objGeneralCodeFileType
            });
        }

        //Habib [20-03-2019]
        [HttpPost]
        [Route("CreateGeneralCodeFileType")]
        public IActionResult SaveGeneralCodeFileType(GeneralCodeFileType objGeneralCodeFileType)
        {
            objGeneralCodeFileType.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
            string Vmsg = _ICommonGeneralCodeFile.SaveGeneralCodeFileType(objGeneralCodeFileType);
            return Ok(new
            {
                message = Vmsg
            });
        }

        //Habib [21-03-2019]
        [HttpGet]
        [Route("GetModuleList")]
        public IActionResult GetModuleList(string pModuleCode, Int32 COMPANY_CODE)
        {
            List<StaticItem> listGeneralModuleList = new List<StaticItem>();
            listGeneralModuleList = _ICommonGeneralCodeFile.GetModuleList();
            return Ok(new
            {
                listGeneralModuleList
            });
        }

        //Habib [21-03-2019]
        [HttpGet]
        [Route("GetGeneralCodeFileByFileType")]
        public IActionResult GetGeneralCodeFileByFileType(Int32 FileTypeCode, string pCompanyCode)
        {
            List<GeneralCodeFile> listGeneralCodeFileByFileType = new List<GeneralCodeFile>();
            listGeneralCodeFileByFileType = _ICommon.GetGeneralCodeFileByFileType(FileTypeCode, pCompanyCode);
            return Ok(new
            {
                listGeneralCodeFileByFileType
            });
        }

        //Habib [24-03-2019]
        [HttpGet]
        [Route("GetGeneralCodeFileLevelByFileType")]
        public IActionResult GetGeneralCodeFileLevelByFileType(Int32 fileTypeCode_PK, Int32 companyCode_PK)
        {
            List<GeneralCodeFileLevel> listGeneralCodeFileLevelByFileType = new List<GeneralCodeFileLevel>();
            listGeneralCodeFileLevelByFileType = _ICommonGeneralCodeFileLevel.GetGeneralCodeFileLevelByFileType(fileTypeCode_PK, companyCode_PK);

            return Ok(new
            {
                listGeneralCodeFileLevelByFileType
            });
        }

        //Habib [25-03-2019]
        [HttpPost]
        [Route("SaveGeneralCodeFileLevel")]
        public IActionResult SaveGeneralCodeFileLevel(GeneralCodeFileLevel objGeneralCodeFileLevel)
        {
            objGeneralCodeFileLevel.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
            if(objGeneralCodeFileLevel.LevelCode_PK == 0)
            {
                objGeneralCodeFileLevel.IsNew = true;
            }
            else
            {
                objGeneralCodeFileLevel.IsNew = false;
            }
            string Vmsg = _ICommonGeneralCodeFileLevel.SaveGeneralCodeFileLevel(objGeneralCodeFileLevel);
            return Ok(new
            {
                message = Vmsg
            });
        }

        //Habib [27-03-2019]
        [HttpPost]
        [Route("SaveGeneralCodeFile")]
        public IActionResult SaveGeneralCodeFile(GeneralCodeFile objGeneralCodeFile)
        {
            objGeneralCodeFile.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
            if (objGeneralCodeFile.FileCode_PK == 0)
            {
                objGeneralCodeFile.IsNew = true;
                objGeneralCodeFile.ActionType = "INSERT";
            }
            else
            {
                objGeneralCodeFile.IsNew = false;
                objGeneralCodeFile.ActionType = "UPDATE";
            }
            string Vmsg = _ICommon.SaveGeneralCodeFile(objGeneralCodeFile);
            return Ok(new
            {
                message = Vmsg
            });
        }

        //Habib [02-04-2019]
        [HttpGet]
        [Route("GetUserList")]
        public IActionResult GetUserList(string pUserName, Int32 pStringMatchOptionValue)
        {
            List<UserMapEmployee> listUser = new List<UserMapEmployee>();
            listUser = _IUserMapEmployee.GetUserList(pUserName, pStringMatchOptionValue);

            return Ok(new
            {
                listUser
            });
        }

        //Habib [03-04-2019]
        [HttpPost]
        [Route("CreateUserMapEmployee")]
        public IActionResult InsertUserMapEmployee(UserMapEmployee objUserMapEmployee)
        {
            objUserMapEmployee.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");

            if (String.IsNullOrEmpty(objUserMapEmployee.UserMapCode_PK))
            {
                objUserMapEmployee.IsNew = true;
            }
            else
            {
                objUserMapEmployee.IsNew = false;
            }

            string Vmsg = _IUserMapEmployee.InsertUserMapEmployee(objUserMapEmployee);
            return Ok(new
            {
                message = Vmsg
            });
        }

        //Habib [21-03-2019]
        [HttpGet]
        [Route("GetUserMapEmployeeByUserCode")]
        public IActionResult GetUserMapEmployeeByUserCode(string pUserCode, bool isActive)
        {
            UserMapEmployee objUserMapEmployee = new UserMapEmployee();
            objUserMapEmployee = _IUserMapEmployee.GetUserMapEmployeeByUserCode(pUserCode, isActive);
            return Ok(new
            {
                objUserMapEmployee
            });
        }
        //[HttpPost]
        //[Route("SendEmail")]
        //public IActionResult SendEmail()
        //{
        //    MailMessage mail = new MailMessage();
        //    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

        //    mail.From = new MailAddress("paramammu@gmail.com");
        //    mail.To.Add("parama.biswas@leads-bd.com");
        //    mail.Subject = "Test Mail";
        //    string body = "<div>Dear Concern,<br><br> Congratulation!<br><br> This is to inform you that your profile with id @PARAMETER@ have been approved.<br><br><br>Regards,<br><br>H&M</div>";
        //    var replacement = body.Replace("@PARAMETER@", "peaks");
        //    mail.Body = replacement;
        //    mail.IsBodyHtml = true;


        //    SmtpServer.Port = 587;
        //    SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    SmtpServer.UseDefaultCredentials = false;
        //    SmtpServer.Credentials = new System.Net.NetworkCredential("paramammu@gmail.com", "lordofring");
        //    SmtpServer.EnableSsl = true;

        //    SmtpServer.Send(mail);
        //    string Vmsg = "Mail Send Successfully";
        //    return Ok(new
        //    {
        //        message = Vmsg
        //    });
        //}
    }
}