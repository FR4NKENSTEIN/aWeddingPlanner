using System;
using System.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using WeddingPlanner.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WeddingPlanner.Controllers
{
    public class UserController: Controller
    {
        public int? LiveUser
        {
            get {return HttpContext.Session.GetInt32("UserID");}
            set {HttpContext.Session.SetInt32("UserID", (int)value);}
        }
        private DatabaseContext database;
        // CONSTRUCTOR
        public UserController(DatabaseContext context)
        {
            database = context;
            Console.WriteLine("###### CONTEXT ACQUIRED");
        }
        // RENDER FORMS
        [Route("")]
        public IActionResult UserIndex()
        {
            HttpContext.Session.Clear();
            Console.WriteLine("###### @ UserIndex");
            return View();
        }
        // PROCESS REGISTRATION
        [HttpPost("registering")]
        public IActionResult Register(UindexView patron)
        {
            if (ModelState.IsValid)
            {
                // check the email isn't already in the data base
                if (database.Users.Any(u => u.Email == patron.UserNew.Email))
                {
                    ModelState.AddModelError("UserNew.Email", "Email already in use... sorry");
                    Console.WriteLine("###### BAD EMAIL");
                    return View("UserIndex", patron);
                }
                // hash the password
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                string hashedPW = hasher
                    .HashPassword(patron.UserNew, patron.UserNew.Password);
                patron.UserNew.Password = hashedPW;
                Console.WriteLine("###### PW HAS BEEN HASHED");
                // Add and save to the database
                database.Add(patron.UserNew);
                database.SaveChanges();
                LiveUser = patron.UserNew.UserId;
                Console.WriteLine("###### VALID");
                return RedirectToAction("Success");
            }
            Console.WriteLine("###### INVALID");
            return View("UserIndex", patron);
        } 

        // PROCESS LOGIN
        [HttpPost("entering")]
        public IActionResult Login(UindexView patron)
        {
            if (ModelState.IsValid)
            {
                // I believe this query would prove inefficient for
                // large sets of data as I am looking at EVERY email.
                User UserInDb = database.Users
                    .FirstOrDefault(u => u.Email == patron.UserExist.Email);
                if (UserInDb == null)
                {
                    ModelState.AddModelError("UserExist.Email", "That Email does not exist");
                    Console.WriteLine("###### BAD EMAIL");
                    return View("UserIndex", patron);
                }
                PasswordHasher<LogUser> hasher = new PasswordHasher<LogUser>();
                var result = hasher.VerifyHashedPassword(
                    patron.UserExist, UserInDb.Password, patron.UserExist.Password
                );
                if (result == 0)
                {
                    ModelState.AddModelError("UserExist.Password", "That password does not exist");
                    Console.WriteLine("###### BAD PASSWORD");
                    return View("UserIndex", patron);
                }
                LiveUser = UserInDb.UserId;
                return RedirectToAction("Success");
            }
            return View("UserIndex");
        } 

        // SUCCESS ACTION
        [Route("success")]
        public IActionResult Success()
        {
            if (LiveUser != null)
            {
                Console.WriteLine("###### SUCCESSFUL");
                return RedirectToAction("Dashboard", "Wedding");
            }
            Console.WriteLine("###### NO NO NO NO NO, WE DON'T DO THAT");
            return RedirectToAction("UserIndex", LiveUser);
        }
        // LOGOUT
        [Route("exiting")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Console.WriteLine("###### BUH BUY");
            return RedirectToAction("UserIndex");
        }
    }
}