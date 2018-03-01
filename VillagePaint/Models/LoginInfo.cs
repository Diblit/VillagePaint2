using VillagePaint.DAL.Classes.Login;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VillagePaint.Models
{
    public class LoginInfo
    {
        public string returnUrl { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class UserLoginInfo
    {
        public bl_Login User { get; set; }
    }
}