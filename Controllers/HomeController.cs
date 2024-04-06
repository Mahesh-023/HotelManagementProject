using HotelManagementProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManagementProject.Controllers
{
    public class HomeController : Controller
    {
        HotelMSEntities db = new HotelMSEntities();
        public ActionResult Index()
        {
            var row = db.Rooms.ToList();
            return View(row);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}