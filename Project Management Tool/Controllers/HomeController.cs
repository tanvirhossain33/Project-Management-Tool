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

        public ActionResult BarChart(int? page)
        {
            var model = new ChartModel()
            {
                Chart = GetBarChart()

            };
            return View(model);
        }

        public ActionResult PieChartForTaskRatio(int? page)
        {
            var model = new ChartModel()
            {
                Chart = GetPieChartForTask()

            };
            return View(model);
        }

        public ActionResult BarChartForTaskRatio(int? page)
        {
            var model = new ChartModel()
            {
                Chart = GetBarChartForTask()

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

                
                List<string> projectCount = new List<string>();
                foreach(var item in projects)
                {
                    projectCount.Add(item.ProjectTeams.Count().ToString());
                }

                return new Chart(500, 400, ChartTheme.Blue)
                .AddTitle("Project Users Ratio")
                .AddLegend()
                .AddSeries(
                    name: "Projects",
                    chartType: "pie",
                    xValue: list ,
                    yValues: projectCount);
        }

        private Chart GetBarChart()
        {
            var projects = db.Projects.ToList();
            List<string> list = new List<string>();
            foreach (var item in projects)
            {
                list.Add(item.Name);
            }
            
            List<string> projectCount = new List<string>();
            foreach (var item in projects)
            {
                projectCount.Add(item.ProjectTeams.Count().ToString());
            }

            return new Chart(500, 400, ChartTheme.Blue)
            .AddTitle("Number of User per Project")
            .AddLegend()
            .AddSeries(
                name: "Projects",
                chartType: "column",
                xValue: list,
                yValues: projectCount );
        }

        private Chart GetPieChartForTask()
        {
            var projects = db.Projects.ToList();
            List<string> list = new List<string>();
            foreach (var item in projects)
            {
                list.Add(item.Name);
            }
            
            List<string> tasks = new List<string>();
            foreach (var item in projects)
            {
                tasks.Add(item.Tasks.Count().ToString());
            }

            return new Chart(500, 400, ChartTheme.Blue)
            .AddTitle("Project Tasks Ratio")
            .AddLegend()

            .AddSeries(
                name: "Project",
                chartType: "pie",
                xValue: list,
                yValues: tasks);
        }

        private Chart GetBarChartForTask()
        {
            var projects = db.Projects.ToList();
            List<string> list = new List<string>();
            foreach (var item in projects)
            {
                list.Add(item.Name);
            }

            List<string> tasks = new List<string>();
            foreach (var item in projects)
            {
                tasks.Add(item.Tasks.Count().ToString());
            }

            return new Chart(500, 400, ChartTheme.Blue)
            .AddTitle("Number of Task per Project")
            .AddLegend()

            .AddSeries(
                name: "Project",
                chartType: "column",
                xValue: list,
                yValues: tasks);
        }

    }
}