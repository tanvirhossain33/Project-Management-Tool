using Project_Management_Tool.Context;
using Project_Management_Tool.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
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

            if (user != null)
            {
                if (user.UserDesignationId == 1)
                    return RedirectToAction("index", "Home");
                else if (user.UserDesignationId == 2)
                    return RedirectToAction("AddProject", "ManageProject");
            }

            ViewBag.Message = null;
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            var anyUser = db.Users.Any(c => c.Email == user.UserName && c.Password == user.Password && c.Status == "Active");

            if (anyUser)
            {
                var userInfo = db.Users.FirstOrDefault(c => c.Email == user.UserName);

                Session["user"] = new User()
                {
                    Id = userInfo.Id,
                    Name = userInfo.Name,
                    Email = userInfo.Email,
                    Password = userInfo.Password,
                    Status = userInfo.Status,
                    UserDesignationId = userInfo.UserDesignationId
                };
                var currentUser = Session["user"] as User;
                if(currentUser.UserDesignationId == 1) return RedirectToAction("Index", "Home");
                else if (currentUser.UserDesignationId == 2) return RedirectToAction("AddProject", "ManageProject");
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

        public JsonResult IsEmailAvailable(string email)
        {
            return Json(!db.Users.Any(c => c.Email == email), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddUser()
        {
            IList<SelectListItem> Status = new List<SelectListItem>
            {
                new SelectListItem{Text = "Active", Value = "Active"},
                new SelectListItem{Text = "InActive", Value = "InActive"}


            };

            var user = Session["user"] as User;
            if (user != null)
            {
                if (user.UserDesignationId == 1)
                {


                    ViewBag.Status = new SelectList(Status, "Text", "Value");
                    ViewBag.UserDesignationId = new SelectList(db.UserDesignations/*.Where(c => c.Id != 1)*/, "Id", "Type");

                    ViewBag.AllUsers = db.Users.ToList();
                    ViewBag.Message = null;
                    return View();
                }
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult AddUser(User user)
        {
            IList<SelectListItem> Status = new List<SelectListItem>
            {
                new SelectListItem{Text = "Active", Value = "Active"},
                new SelectListItem{Text = "InActive", Value = "InActive"}


            };

            var allUser = Session["user"] as User;
            if (allUser != null)
            {
                if (allUser.UserDesignationId == 1)
                {
                    ViewBag.Status = new SelectList(Status, "Text", "Value");
                    ViewBag.UserDesignationId = new SelectList(db.UserDesignations/*.Where(c => c.Id != 1)*/, "Id", "Type");

                    string message = "";
                    db.Configuration.ValidateOnSaveEnabled = false;

                    db.Users.Add(user);
                    var rowChange = db.SaveChanges();


                    //try
                    //{
                    //    // Your code...
                    //    // Could also be before try if you know the exception occurs in SaveChanges

                    //    db.SaveChanges();
                    //}
                    //catch (DbEntityValidationException e)
                    //{
                    //    foreach (var eve in e.EntityValidationErrors)
                    //    {
                    //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    //        foreach (var ve in eve.ValidationErrors)
                    //        {
                    //            Console.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                    //                ve.PropertyName,
                    //                eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                    //                ve.ErrorMessage);
                    //        }
                    //    }
                    //    throw;
                    //}



                    if (rowChange > 0)
                    {
                        message = "User add successfull ..";
                    }
                    else
                    {
                        message = "User add failed !!";
                    }
                    ViewBag.AllUsers = db.Users.ToList();
                    ViewBag.Message = message;

                    return View();
                }
            }
            return RedirectToAction("Login", "Account");
        }


        public ActionResult UpdateUser(int id)
        {
            IList<SelectListItem> Status = new List<SelectListItem>
            {
                new SelectListItem{Text = "Active", Value = "Active"},
                new SelectListItem{Text = "InActive", Value = "InActive"}


            };

            var user = Session["user"] as User;
            if (user != null)
            {
                if (user.UserDesignationId == 1)
                {


                    ViewBag.Status = new SelectList(Status, "Text", "Value");
                    ViewBag.UserDesignationId = new SelectList(db.UserDesignations/*.Where(c => c.Id != 1)*/, "Id", "Type");

                    ViewBag.SingleUserInfo = db.Users.FirstOrDefault(c => c.Id == id);

                    ViewBag.AllUsers = db.Users.ToList();
                    ViewBag.Message = null;
                    return View();
                }
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult UpdateUser(User user, int id)
        {
            IList<SelectListItem> Status = new List<SelectListItem>
            {
                new SelectListItem{Text = "Active", Value = "Active"},
                new SelectListItem{Text = "InActive", Value = "InActive"}


            };

            var allUser = Session["user"] as User;
            if (allUser != null)
            {
                if (allUser.UserDesignationId == 1)
                {
                    ViewBag.Status = new SelectList(Status, "Text", "Value");
                    ViewBag.UserDesignationId = new SelectList(db.UserDesignations/*.Where(c => c.Id != 1)*/, "Id", "Type");

                    string message = "";
                    db.Configuration.ValidateOnSaveEnabled = false;


                    var update = db.Users.Find(id);
                    update.Name = user.Name;
                    //update.Email = user.Email;
                    update.Password = user.Password;
                    update.Status = user.Status;
                    update.UserDesignationId = user.UserDesignationId;
                    db.Entry(update).State = EntityState.Modified;
                    var rowAffected = db.SaveChanges();
                    if (rowAffected > 0)
                    {
                        message = "Update Successfull";
                    }
                    else
                    {
                        message = "Update Failed";
                    }

                    ViewBag.SingleUserInfo = db.Users.FirstOrDefault(c => c.Id == id);
                    ViewBag.AllUsers = db.Users.ToList();
                    ViewBag.Message = message;

                    return View();
                }
            }
            return RedirectToAction("Login", "Account");

        }
    }
}