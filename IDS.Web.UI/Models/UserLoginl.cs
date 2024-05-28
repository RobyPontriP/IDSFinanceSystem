using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IDS.Web.UI.Models
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Please enter your User ID", AllowEmptyStrings = false)]
        [Display(Name = "Enter User ID")]
        public string UserID { get; set; }

        [Required(ErrorMessage = "Please enter your password.")]
        [Display(Name = "Enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } 
    }
}