using HotelManagementProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManagementProject.Controllers
{
    public class BookingController : Controller
    {
        HotelMSEntities db = new HotelMSEntities();
        // GET: /Booking/Index
        public ActionResult Index()
        {
            var bookings = db.Bookings.ToList();
            return View(bookings);
        }

        // GET: /Booking/Details/1
        public ActionResult Details(int id)
        {
            var booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }


        // GET: /Booking/Edit/1
        public ActionResult Edit(int id)
        {
            var booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            var customers = db.Customers.ToList();
            var rooms = db.Rooms.ToList();
            ViewBag.Customers = new SelectList(customers, "CustomerId", "Name", booking.CustomerId);
            ViewBag.Rooms = new SelectList(rooms, "RoomId", "RoomNumber", booking.RoomId);
            return View(booking);
        }

        // POST: /Booking/Edit/1
        [HttpPost]
        public ActionResult Edit(Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var customers = db.Customers.ToList();
            var rooms = db.Rooms.ToList();
            ViewBag.Customers = new SelectList(customers, "CustomerId", "Name", booking.CustomerId);
            ViewBag.Rooms = new SelectList(rooms, "RoomId", "RoomNumber", booking.RoomId);
            return View(booking);
        }

        // GET: /Booking/Delete/1
        public ActionResult Delete(int id)
        {
            var booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: /Booking/Delete/1
        [HttpPost, ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            var booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}