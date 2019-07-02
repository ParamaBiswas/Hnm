using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CenterPointCoreSecurity.Model
{
    public class LoginModel
    {
        [Required(ErrorMessage ="User Name is Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Password is required")]
        public string Password { get; set; }
        public int? EmailStatus { get; set; }
    }
}
