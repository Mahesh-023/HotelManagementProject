using HotelManagementProject.Filters;
using HotelManagementProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManagementProject.Controllers
{
    public class PaymentController : Controller
    {
        HotelMSEntities db = new HotelMSEntities();
        // GET: Payment
        [CustomAuthorize]
        public ActionResult Index(int? id)
        {
            var payment = db.Payments.Where(b => b.BookingId == id).ToList();
            if (payment != null)
            {
                return View(payment);
            }
            return HttpNotFound();
        }

     
        public ActionResult MakePayment(int id, decimal amt)
        {
            var booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            var payment = new Payment
            {
                BookingId = id,
                AmountPaid = amt,
                PaymentDate = DateTime.Now
            };
            ViewBag.bookingId = id;
            ViewBag.Amt = amt;
            ViewBag.date = DateTime.Now;
            return View(payment);
        }
        [HttpPost]
        public ActionResult MakePayment(Payment payment)
        {
            if (ModelState.IsValid)
            {
                if (payment.PaymentMode == "PayOnVisit")
                {
                    payment.Paymentstatus = "Incomplete";
                }
                else
                {
                    payment.Paymentstatus = "Completed";
                }
                db.Payments.Add(payment);
                db.SaveChanges();
                return RedirectToAction("PaymentConfirmation");
            }
            return View(payment);
        }
        public ActionResult PaymentConfirmation()
        {
            return View();
        }
    }
}