using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CenterPCore.Controllers
{
    public class CommonController : Controller
    {
        public IActionResult GetGeneralCodeFileTypeAll()
        {
            return View();
        }

        public IActionResult CreateUserMapEmployee()
        {
            return View();
        }

    }
}