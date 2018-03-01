using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VillagePaint.Models
{
    public class CustomerInfo
    {
        public long customerID { get; set; }
        public string CardNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string Cell { get; set; }
        public string AddressStreet { get; set; }
        public string AddressSuburb { get; set; }
        public string AddressCity { get; set; }
        public string ZipCode { get; set; }
    }
}