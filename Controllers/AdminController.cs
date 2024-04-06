using HotelManagementProject.Filters;
using HotelManagementProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;

namespace HotelManagementProject.Controllers
{
    public class AdminController : Controller
    {
        HotelMSEntities db = new HotelMSEntities();
        // GET: Admin/Index
        public ActionResult Index()
        {
            return View();
        }
        [CustomAuthorize]
        public ActionResult ManageRoom(int? i)
        {
            var rooms = db.Rooms.ToList().ToPagedList(i ?? 1, 6);
            return View(rooms);
        }
        //GET:/Admin/AddRoom
        [CustomAuthorize]
        public ActionResult AddRoom()
        {
            var roomType = new List<string> { "Suite", "Deluxe", "Single", "Double" };
            ViewBag.RoomType = new SelectList(roomType);

            var status = new List<string> { "Available", "Booked" };
            ViewBag.status = new SelectList(status);
            return View();
        }
        //POST:/Admin/AddRoom
        [HttpPost]
        public ActionResult AddRoom(Room room)
        {

            if (ModelState.IsValid)
            {
                string filename = Path.GetFileNameWithoutExtension(room.ImageFile.FileName);

                string extension = Path.GetExtension(room.ImageFile.FileName);

                filename = filename + extension;

                room.RoomImage = "~/RoomImages/" + filename;

                filename = Path.Combine(Server.MapPath("~/RoomImages"), filename);

                room.ImageFile.SaveAs(filename);
                db.Rooms.Add(room);
                db.SaveChanges();
                return RedirectToAction("ManageRoom");
            }
            return View(room);
        }
        //GET:/Admin/EditRoom/1
        [CustomAuthorize]
        public ActionResult EditRoom(int id)
        {
            var room = db.Rooms.Find(id);
            var roomType = new List<SelectListItem> { new  SelectListItem{ Text= "Suite" },
            new  SelectListItem{ Text = "Deluxe" } ,
                new  SelectListItem{ Text = "Single" } ,
                new  SelectListItem{ Text= "Double" }
                };

            ViewBag.RoomType = roomType;
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }
        //POST:/Admin/EditRoom/1
        [HttpPost]
        public ActionResult EditRoom(Room room)
        {


            var status = new List<string> { "Available", "Booked" };
            ViewBag.status = new SelectList(status);
            if (ModelState.IsValid)
            {
                db.Entry(room).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ManageRoom");
            }
            return View(room);
        }
        //GET:/Admin/DeleteRoom/1
        [CustomAuthorize]
        public ActionResult DeleteRoom(int id)
        {
            var room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        //POST:/Admin/DeleteRoom/1
        [HttpPost, ActionName("DeleteRoom")]
        public ActionResult ConfirmDeleteRoom(int id)
        {
            var room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            db.Rooms.Remove(room);
            db.SaveChanges();
            return RedirectToAction("ManageRoom");
        }
        //Booking Management
        [CustomAuthorize]
        public ActionResult ManageBookings(int? i)
        {
            var bookings = db.Bookings.ToList().ToPagedList(i ?? 1, 8);
            return View(bookings);
        }
        //Customer Management
        [CustomAuthorize]
        public ActionResult ManageCustomers(int? i)
        {
            var customers = db.Customers.ToList().ToPagedList(i ?? 1, 8);
            return View(customers);
        }

        [CustomAuthorize]
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer cust = db.Customers.Find(id);
            if (cust == null)
            {
                return HttpNotFound();
            }
            return View(cust);

        }

        [CustomAuthorize]
        public ActionResult DeleteCustomer(int id)
        {
            var room = db.Customers.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }
        //Post
        [HttpPost, ActionName("DeleteCustomer")]
        public ActionResult DeleteCustomers(int id)
        {
            var customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            // Find associated bookings
            var bookings = db.Bookings.Where(b => b.CustomerId == id).ToList();
            foreach (var booking in bookings)
            {
                // Delete associated bookings
                db.Bookings.Remove(booking);
                var pay = db.Payments.Where(x => x.BookingId == booking.BookingId);
                foreach (var p in pay)
                {

                    db.Payments.Remove(p);
                }
            }
            //Remove Payment details 


            // Update room status
            foreach (var booking in bookings)
            {
                var room = db.Rooms.Find(booking.RoomId);
                if (room != null)
                {
                    room.Status = "Available";
                }
            }

            // Delete the user account
            var userAccount = db.UserAccounts.FirstOrDefault(u => u.UserId == customer.UserId);
            if (userAccount != null)
            {
                db.UserAccounts.Remove(userAccount);
            }

            // Delete the customer
            db.Customers.Remove(customer);
            db.SaveChanges();

            return RedirectToAction("ManageCustomers");
        }

        public ActionResult DeleteBooking(int id)
        {
            var book = db.Bookings.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            var pay = db.Payments.Where(x => x.BookingId == book.BookingId);
            foreach (var p in pay)
            {

                db.Payments.Remove(p);
            }


            var room = db.Rooms.Find(book.RoomId);
            if (room != null)
            {
                room.Status = "Available";
            }
            db.Bookings.Remove(book);

            db.SaveChanges();
            return RedirectToAction("ManageBookings");
        }
    }
}