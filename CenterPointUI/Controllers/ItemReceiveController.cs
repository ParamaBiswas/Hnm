using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CenterPointUI.Models;

namespace CenterPointUI.Controllers
{
    public class ItemReceiveController : Controller
    {
        public IActionResult ItemReceiveGRN()
        {
            return View();
        }
       
    }
}
