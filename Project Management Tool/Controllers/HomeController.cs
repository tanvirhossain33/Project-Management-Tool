using Project_Management_Tool.Context;
using Project_Management_Tool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Project_Management_Tool.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db = new ApplicationContext();
        public ActionResult Index()
        {
            var user = Session["user"] as User;
            if(user != null)
            {
                if (user.UserDesignationId == 1)
                {
                    ViewBag.TotalUser = db.Users.Count();
                    ViewBag.TotalProject = db.Projects.Count();
                    ViewBag.TotalTask = db.Tasks.Count();
                    ViewBag.TotalCommnet = db.Comments.Count();
                    return View();
                }
            }
            
            return RedirectToAction("Login","account");
        }

        public ActionResult Chart(int? page)
        {
            var model = new ChartModel()
            {
                Chart = GetChart()

            };
            return View(model);
        }


    private Chart GetChart()
    {
            var projects = db.Projects.ToList();
            List<string> list = new List<string>();
            foreach(var item in projects)
            {
                list.Add(item.Name);
            }

            var projectTeam = db.ProjectTeams.GroupBy(b => b.ProjectId).Select(g => new { ProjectId = g.Key, Count = g.Count() });
            List<string> projectCount = new List<string>();
            foreach(var item in projectTeam)
            {
                projectCount.Add(item.Count.ToString());
            }

            return new Chart(600, 400, ChartTheme.Blue)
            .AddTitle("Number Of Project Users")
            .AddLegend()
            .AddSeries(
                name: "Project",
                chartType: "Pie",
                xValue: list /*new[] { "Digg", "DZone", "DotNetKicks", "StumbleUpon" }*/,
                yValues: projectCount /*new[] { "150000", "180000", "120000", "250000" }*/);
    }

}
}