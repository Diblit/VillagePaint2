using VillagePaint.DAL;
using VillagePaint.DAL.Classes.Admin;
using VillagePaint.DAL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using VillagePaint.Utility;

namespace VillagePaint.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CustomerList()
        {
            var paging = new PagingInfo
            {
                skip = 0,
                take = 10
            };

            var data = bl_Customer.CustomerList(ref paging);

            ViewBag.data = new JavaScriptSerializer().Serialize(new { List = data, Count = paging.result_count });

            return View();
        }
        //GET: Admin/AdminDash
        public ActionResult AdminDash()
        {
            var data = bl_AdminDash.AdminDashList();
            ViewBag.data = new JavaScriptSerializer().Serialize(new { List = data });

            return View();
        }

        public ActionResult AdminList()
        {
            var paging = new PagingInfo
            {
                skip = 0,
                take = 10
            };

            var data = bl_Admin.AdminList(ref paging);
            var userID = this.loggedInUserID();

            ViewBag.data = new JavaScriptSerializer().Serialize(new { List = data, Count = paging.result_count});
            ViewBag.loggedInUserID = new JavaScriptSerializer().Serialize(new { LoggedInUserID = userID});
            return View();
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
