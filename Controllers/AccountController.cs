using HotelManagementProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManagementProject.Controllers
{
    public class AccountController : Controller
    {
        HotelMSEntities db = new HotelMSEntities();
        // GET: Account/
        public ActionResult Login()
        {
            return View();
        }
        //POST
        [HttpPost]
        public ActionResult Login(string username,string password)
         {
            var user=db.UserAccounts.FirstOrDefault(u=>u.Username==username && u.Password==password);
            if(user != null)
            {
                if(user.IsAdmin)
                {
                    Session["Username"] = user.Username.ToString();
                    Session["id"] = user.UserId;
                    Session["UserId"] = user.UserId;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    string userId = Convert.ToString(user.UserId);
                    Session["Username"] = user.Username.ToString();
                    Session["id"] = user.UserId;
                    Session["UserId"] = userId;
                    return RedirectToAction("Profile", "Customer", new {id = userId});
                }
            }
            else
            {
                ViewBag.Error = "Invalid username and Password";
                return View();
            }
        }
        //GET
        public ActionResult SignUp()
        {
            return View();
        }

        //POST
        [HttpPost]
        public ActionResult SignUp(string username, string password, string Repassword, string email)
        {

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(Repassword) || string.IsNullOrEmpty(email))
            {
                ViewBag.Error2 = "All fields are required. Please fill in all the details.";
                return View();
            }
            if (ModelState.IsValid)
            {
                if (db.UserAccounts.Any(u => u.Username == username))
                {
                    ViewBag.Error = "Username is already Taken. Please choose different name.";
                    return View();
                }

                if (password != Repassword)
                {
                    ViewBag.Error1 = "Password Do Not Match";
                    return View();
                }
                var newUser = new UserAccount
                {
                    Username = username,
                    Password = password,
                    Email = email,
                    rePassword = Repassword,
                    IsAdmin = false 
                };


                db.UserAccounts.Add(newUser);

                db.SaveChanges();
            }
                var user = db.UserAccounts.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    var NewCustomer = new Customer
                    {
                        UserId = user.UserId
                    };
                    db.Customers.Add(NewCustomer);
                    db.SaveChanges();
                }

                return RedirectToAction("Login");
            }
        
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}