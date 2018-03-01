using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VillagePaint.Models
{
    public class AdminInfo
    {
        public long userID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Cell { get; set; }
    }
}