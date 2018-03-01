using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VillagePaint.Models
{
    public class ResetPasswordInfo
    {
        public long userID { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}