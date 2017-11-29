using Project_Management_Tool.Context;
using Project_Management_Tool.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_Management_Tool.Controllers
{
    public class ManageProjectController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: ManageProject
        public ActionResult AddProject()
        {

            var user = Session["user"] as User;

            if(user != null && user.UserDesignationId == 2)
            {
                IList<SelectListItem> Status = new List<SelectListItem>
                {
                    new SelectListItem{Text = "Not Started", Value = "Not Started"},
                    new SelectListItem{Text = "Started", Value = "Started"},
                    new SelectListItem{Text = "Completed", Value = "Completed"},
                    new SelectListItem{Text = "Cancelled", Value = "Cancelled"}
                };
                ViewBag.Status = new SelectList(Status, "Text", "Value");
                ViewBag.Message = null;
                return View();
            }

            return RedirectToAction("Login","Account");
        }

        [HttpPost]
        public ActionResult AddProject(Project project, HttpPostedFileBase file)
        {

            IList<SelectListItem> Status = new List<SelectListItem>
            {
                new SelectListItem{Text = "Not Started", Value = "Not Started"},
                new SelectListItem{Text = "Started", Value = "Started"},
                new SelectListItem{Text = "Completed", Value = "Completed"},
                new SelectListItem{Text = "Cancelled", Value = "Cancelled"}
            };
            ViewBag.Status = new SelectList(Status, "Text", "Value");

            string message = "";

            db.Projects.Add(project);
            int saveConfirm = 0;

            if(file != null && file.ContentLength > 0)
            {
                try
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var fileType = Path.GetExtension(fileName);
                    fileName = fileName + project.CodeName + fileType;
                    var path = Path.Combine(Server.MapPath("~/App_Data/ProjectFiles"), fileName);
                    file.SaveAs(path);
                    saveConfirm = 1;
                }
                catch (Exception ex)
                {
                    saveConfirm = 0;
                    message = "File Upload Failed";
                }
            }
            else
            {
                saveConfirm = 0;
                message = "You have not specified a file !!";
            }
            var user = Session["user"] as User;
            if(saveConfirm == 1)
            {
                var rowChanged = db.SaveChanges();
                if(rowChanged > 0)
                {
                    var projectTeam = new ProjectTeam()
                    {
                        ProjectId = project.Id,
                        UserId = user.Id
                    };
                    db.ProjectTeams.Add(projectTeam);
                    var changedRow = db.SaveChanges();
                    if (changedRow > 0)
                    {
                        message = "Project Created Successfull";
                    }
                    else
                    {
                        message = "Project Created Failed";
                    }
                }
                else
                {
                    message = "Project Created Failed";
                }
            }

            ViewBag.Message = message;

            return View();
        }

        public ActionResult AssignPerson()
        {
            var user = Session["user"] as User;
            if(user != null && user.UserDesignationId == 2)
            {
                ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
                ViewBag.UserId = new SelectList(db.Users.Where(c => c.UserDesignationId != 1), "Id", "Name");
                ViewBag.Message = null;
                ViewBag.ProjectTeam = db.ProjectTeams.ToList();
                return View();
            }

            return RedirectToAction("Login","Account");
        }

        [HttpPost]
        public ActionResult AssignPerson(ProjectTeam projectTeam)
        {
            var user = Session["user"] as User;
            if (user != null && user.UserDesignationId == 2)
            {
                ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
                ViewBag.UserId = new SelectList(db.Users.Where(c => c.UserDesignationId != 1), "Id", "Name");
                
                string message = "";
                var any = db.ProjectTeams.Any(c => c.ProjectId == projectTeam.ProjectId && c.UserId == projectTeam.UserId);

                if(!any)
                {
                    db.ProjectTeams.Add(projectTeam);
                    var rowAffected = db.SaveChanges();
                    if (rowAffected > 0)
                    {
                        message = "Assign Successfull ..";
                    }
                    else
                    {
                        message = "Assign Failed !!";
                    }
                }
                else
                {
                    message = "This Project Already assigned to this person";
                }


                ViewBag.Message = message;
                ViewBag.ProjectTeam = db.ProjectTeams.ToList();
                return View();
            }

            return RedirectToAction("Login", "Account");
        }



        public ActionResult AllProject()
        {
            var user = Session["user"] as User;

            if (user != null && user.UserDesignationId != 1)
            {
                
                var proj = db.ProjectTeams.GroupBy(b => new { b.Project.Id, b.Project.Name, b.Project.CodeName, b.Project.Status }).Select(g => new { g.Key.Id, g.Key.Name, g.Key.CodeName, g.Key.Status, Count = g.Count() }).ToList();

                var task = db.Tasks.GroupBy(b => new { b.Project.Id }).Select(g => new { g.Key.Id, Count = g.Count() }).ToList();

                var result = proj.Join(task,
                    a => a.Id,
                    b => b.Id,
                    (b, a) => new { Member = b, Task = a }).ToList();

                List<ProjectInvolved> list = new List<ProjectInvolved>();

                foreach(var item in result)
                {
                    ProjectInvolved projectInvolved = new ProjectInvolved()
                    {   
                        Id = item.Member.Id,
                        Name = item.Member.Name,
                        CodeName = item.Member.CodeName,
                        Status = item.Member.Status,
                        NOM = item.Member.Count,
                        NOT = item.Task.Count
                    };
                    list.Add(projectInvolved);
                }
                
                ViewBag.AllProject = list;
                return View();
            }
            return RedirectToAction("Login", "Account");

        }

        public ActionResult UpdateProjectDetails(int id)
        {
            var user = Session["user"] as User;

            if (user != null && user.UserDesignationId == 2)
            {
                IList<SelectListItem> Status = new List<SelectListItem>
                {
                    new SelectListItem{Text = "Not Started", Value = "Not Started"},
                    new SelectListItem{Text = "Started", Value = "Started"},
                    new SelectListItem{Text = "Completed", Value = "Completed"},
                    new SelectListItem{Text = "Cancelled", Value = "Cancelled"}
                };
                ViewBag.Status = new SelectList(Status, "Text", "Value");
                ViewBag.Message = null;
                ViewBag.Details = db.Projects.FirstOrDefault(c => c.Id == id);
                return View();
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult UpdateProjectDetails(Project project, int id, HttpPostedFileBase file)
        {
            IList<SelectListItem> Status = new List<SelectListItem>
                {
                    new SelectListItem{Text = "Not Started", Value = "Not Started"},
                    new SelectListItem{Text = "Started", Value = "Started"},
                    new SelectListItem{Text = "Completed", Value = "Completed"},
                    new SelectListItem{Text = "Cancelled", Value = "Cancelled"}
                };
            ViewBag.Status = new SelectList(Status, "Text", "Value");

            ViewBag.Details = db.Projects.FirstOrDefault(c => c.Id == id);


            string message = "";

            var update = db.Projects.Find(id);
            update.Name = project.Name;
            update.Description = project.Description;
            update.StartDate = project.StartDate;
            update.EndDate = project.EndDate;
            update.Duration = project.Duration;
            update.Status = project.Status;
            db.Entry(update).State = EntityState.Modified;
            int saveConfirm = 0;

            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var fileType = Path.GetExtension(fileName);
                    fileName = fileName + project.CodeName + fileType;
                    var path = Path.Combine(Server.MapPath("~/App_Data/ProjectFiles"), fileName);
                    file.SaveAs(path);
                    saveConfirm = 1;
                }
                catch (Exception ex)
                {
                    saveConfirm = 0;
                    message = "File Upload Failed";
                }
            }
            else
            {
                var rowChanged = db.SaveChanges();
                if (rowChanged > 0)
                {
                    message = "Project Update Successfull";
                }
                else
                {
                    message = "Project Update Failed";
                }
            }
            
            if (saveConfirm == 1)
            {
                var rowChanged = db.SaveChanges();
                if (rowChanged > 0)
                {
                    message = "Project Update Successfull";
                }
                else
                {
                    message = "Project Update Failed";
                }
            }

            ViewBag.Message = message;

            return View();
        }
    }
}