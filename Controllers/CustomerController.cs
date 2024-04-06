using HotelManagementProject.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;

namespace HotelManagementProject.Controllers
{
    public class CustomerController : Controller
    {
        HotelMSEntities db = new HotelMSEntities();
        // GET: /Customer/Index
        public ActionResult Index()
        {
            // Retrieve available rooms
            var availableRooms = db.Rooms.Where(r => r.Status == "Available").ToList();
            return View(availableRooms);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: /Customer/Search
        public ActionResult Search(string roomType, decimal? minPrice, decimal? maxPrice , string roomStatus,int?i)
        {
            // Search for rooms based on criteria
            var rooms = db.Rooms.AsQueryable();

            if(roomStatus =="Available" || roomStatus == "Booked")
            {
                rooms = rooms.Where(r => r.Status == roomStatus);
            }

            if (!string.IsNullOrEmpty(roomType))
            {
                rooms = rooms.Where(r => r.RoomType == roomType);
            }
            if (minPrice.HasValue)
            {
                rooms = rooms.Where(r => r.Price >= minPrice);
            }
            if (maxPrice.HasValue)
            {
                rooms = rooms.Where(r => r.Price <= maxPrice);
            }
            ViewBag.RoomTypes=new SelectList(db.Rooms.Select(r => r.RoomType).Distinct());
            ViewBag.RoomStatus= new SelectList(db.Rooms.Select(r => r.Status).Distinct());

            return View(rooms.ToList().ToPagedList(i ?? 1, 6));
        }

        // GET: /Customer/MyBookings
        public ActionResult MyBookings()
        {
            // Retrieve bookings for the current customer
            int? customerId = Session["CustomerId"] as int?;
            if (customerId.HasValue)
            {
                var bookings = db.Bookings.Where(b => b.CustomerId == customerId.Value).ToList();
                return View(bookings);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: /Customer/CreateBooking
        public ActionResult CreateBooking(int? id)
        {
            // Populate dropdown lists for customers and rooms
            int? customerId = Session["CustomerId"] as int?;
            if (customerId.HasValue)
            {
                ViewBag.CustomerId=customerId.Value;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            var customers = db.Customers.ToList();
            var rooms = db.Rooms.Where(r => r.Status == "Available").ToList(); // Only show available rooms
            ViewBag.RoomId = id;
            ViewBag.Customers = new SelectList(customers, "CustomerId", "Name");
            ViewBag.Rooms = new SelectList(rooms, "RoomId", "RoomNumber");
            return View();
        }

        // POST: /Customer/CreateBooking
        [HttpPost]
        public ActionResult CreateBooking(Booking booking)
        { 

            int? customerId = Session["CustomerId"] as int?;
            if(customerId.HasValue)
            {
                 TimeSpan d = booking.CheckOutDate - booking.CheckInDate;
                decimal numberOfDays = (decimal)d.TotalDays;
                booking.CustomerId=customerId.Value;
                var room = db.Rooms.FirstOrDefault(r => r.RoomId == booking.RoomId);
                decimal RoomPrice = room.Price;
                decimal TotalAmount = RoomPrice * numberOfDays;
                Session["amount"] = TotalAmount;
                if (room!=null)
                {
                    room.Status = "Booked";
                }
                booking.TotalAmount = TotalAmount;
                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("MyBookings");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: /Customer/ViewBooking/1
        public ActionResult ViewBooking(int id)
        {
            var booking = db.Bookings.Find(id);

            if (booking == null || booking.CustomerId != (Session["CustomerId"] as int?).Value)
            {
                return HttpNotFound();
            }
            return View(booking);
        }
        public ActionResult Profile(string id)
        {
            
            if (ModelState.IsValid)
            {
                int userId = int.Parse(id);
                var custId = db.Customers.FirstOrDefault(c => c.UserId == userId);
                var customer = db.Customers.Find(custId.CustomerId);
                Session["CustomerId"] = custId.CustomerId;
                ViewBag.UserId = id;
                return View(customer);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        public ActionResult Profile(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                
                return RedirectToAction("Index", "Home");
            }
            return View(customer);
        }


        public ActionResult Cancel()
        {
            return View();
        }
    }
}