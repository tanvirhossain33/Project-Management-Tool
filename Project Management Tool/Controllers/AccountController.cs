using Project_Management_Tool.Context;
using Project_Management_Tool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Project_Management_Tool.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext db = new ApplicationContext();



        // GET: Account
        public ActionResult Login()
        {
            var user = Session["user"] as User;

            if(user != null)
            {
                return RedirectToAction("index", "Home");
            }

            ViewBag.Message = null;
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            var anyUser = db.Users.Any(c => c.Email == user.Email && c.Password == user.Password && c.Status == true);

            if (anyUser)
            {
                var userInfo = db.Users.FirstOrDefault(c => c.Email == user.Email);

                Session["user"] = new User()
                {
                    Id = userInfo.Id,
                    Name = userInfo.Name,
                    Email = userInfo.Email,
                    Password = userInfo.Password,
                    Status = userInfo.Status,
                    UserDesignationId = userInfo.UserDesignationId
                };

                return RedirectToAction("Index", "Home");
            }




            ViewBag.Message = "email and password is incorrect, Try again !!";
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}