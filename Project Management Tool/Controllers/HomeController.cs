using Project_Management_Tool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_Management_Tool.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var user = Session["user"] as User;

            if(user != null)
            {
                if (user.UserDesignationId == 1)
                {
                    return View();
                }
            }
            
            return RedirectToAction("Login","account");
        }

    }
}