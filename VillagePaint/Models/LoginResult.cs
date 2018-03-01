using VillagePaint.DAL.Classes.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VillagePaint.Models
{
    public class LoginResult
    {
        public bool isSuccess { get; set; }
        public string errorText { get; set; }
        public string returnUrl { get; set; }
        public string userType { get; set; }
        //public bool isPasswordAgeExpired { get; set; }
        public bl_Login UserInfo { get; set; }
    }
}