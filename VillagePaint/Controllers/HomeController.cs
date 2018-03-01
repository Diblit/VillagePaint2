using VillagePaint.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace VillagePaint.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string ReturnUrl)
        {
            ViewBag.returnUrl = ReturnUrl;

            if (this.User.Identity.IsAuthenticated)
            {
                return Redirect("~/Admin/AdminDash");
            }

            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            UserTicket.clearUserSession();

            // clear authentication cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            return Redirect("~/Home/Login");
        }
    }
}