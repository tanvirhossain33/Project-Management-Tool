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

                    var name = fileName.Replace(fileType, project.CodeName);
                    fileName = name + fileType;
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
                var proj = db.ProjectTeams.GroupBy(b => new { b.Project.Id, b.Project.Name, b.Project.CodeName, b.Project.Status })
                    .Select(g => new { g.Key.Id, g.Key.Name, g.Key.CodeName, g.Key.Status, Count = g.Count() }).ToList();
                //var task = db.Tasks.GroupBy(b => new { b.Project.Id }).Select(g => new { g.Key.Id, Count = g.Count() }).ToList();
                var task = db.Projects.Where(c => c.Tasks.Count >= 0).ToList();




                List<ProjectInvolved> list = new List<ProjectInvolved>();
                int i = 0;
                foreach (var item in proj)
                {
                    var any = db.ProjectTeams.Any(c => c.ProjectId == item.Id && c.UserId == user.Id);

                    if (any)
                    {
                        ProjectInvolved projectInvolved = new ProjectInvolved()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            CodeName = item.CodeName,
                            Status = item.Status,
                            NOM = item.Count,
                            NOT = task[i].Tasks.Count()
                        };
                        list.Add(projectInvolved);
                    }
                    i++;
                    
                }
                ViewBag.AllProject = list;
                return View();
            }
            return RedirectToAction("Login", "Account");
        }
        private int Max(int val1, int val2)
        {
            if (val1 > val2) return val1;
            else return val2;
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
            var details = db.Projects.FirstOrDefault(c => c.Id == id);
            ViewBag.Details = details;
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
                    var name = fileName.Replace(fileType, details.CodeName);
                    fileName = name + fileType;
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

        public ActionResult ProjectDetails(int id)
        {
            var user = Session["user"] as User;
            var any = db.ProjectTeams.Any(c => c.ProjectId == id && c.UserId == user.Id);
            if (user != null && user.UserDesignationId != 1 && any == true)
            {
                var projectDetails = db.Projects.FirstOrDefault(c => c.Id == id);
                var assignedMember = db.ProjectTeams.Where(c => c.ProjectId == id).ToList();
                var dir = new System.IO.DirectoryInfo(Server.MapPath("~/App_Data/ProjectFiles"));
                System.IO.FileInfo[] fileNames = dir.GetFiles("*"+projectDetails.CodeName + ".*");
                List<string> list = new List<string>();
                if(fileNames.Length != 0)
                {
                    foreach (var item in fileNames)
                    {
                        string str = item.FullName.Replace(projectDetails.CodeName + item.Extension, item.Extension);
                        str = str.Replace(item.DirectoryName + "\\", "");
                        list.Add(str);
                    }
                }
                else
                {
                    list = null;
                }
                var tasks = db.Tasks.Where(c => c.ProjectId == id).ToList();

                

                ViewBag.ProjectDetails = projectDetails;
                ViewBag.AssignMember = assignedMember;
                ViewBag.FileNames = list;
                ViewBag.Tasks = tasks;
                //ViewBag.ResourcePerson = any;
                return View();
            }
            return RedirectToAction("Login","Account");
        }

        public ActionResult DownloadFile(string fileName, string code)
        {
            var session = Session["user"] as User;
            if (session != null && session.UserDesignationId != 1)
            {
                string[] s = fileName.Split('.');
                int len = s.Length;
                string extension = "." + s[len - 1];
                string name = fileName.Replace(extension, code + extension);
                var dir = new System.IO.DirectoryInfo(Server.MapPath("~/App_Data/ProjectFiles"));
                System.IO.FileInfo[] fileNames = dir.GetFiles(name);
                if (fileNames.Length > 0)
                {
                    var data = File("~/App_Data/ProjectFiles/" + name, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                    return data;
                }
            }
            return RedirectToAction("Login", "Account");
        }


        public ActionResult CreateTask(int projectId)
        {
            var user = Session["user"] as User;
            if (user != null && user.UserDesignationId != 1)
            {
                var any = db.ProjectTeams.Any(c => c.ProjectId == projectId && c.UserId == user.Id);
                if (any)
                {
                    IList<SelectListItem> Priority = new List<SelectListItem>
                    {
                        new SelectListItem{Text = "High", Value = "High"},
                        new SelectListItem{Text = "Medium", Value = "Medium"},
                        new SelectListItem{Text = "Low", Value = "Low"}

                    };
                    ViewBag.Priority = new SelectList(Priority, "Text", "Value");

                    ViewBag.Project = db.ProjectTeams.Where(c => c.UserId == user.Id).Distinct().ToList();

                    

                    ViewBag.Message = null;
                    return View();
                }
                
            }


            return RedirectToAction("Login","Account");
        }

        [HttpPost]
        public ActionResult CreateTask(Task task)
        {
            var user = Session["user"] as User;
            IList<SelectListItem> Priority = new List<SelectListItem>
                {
                    new SelectListItem{Text = "High", Value = "High"},
                    new SelectListItem{Text = "Medium", Value = "Medium"},
                    new SelectListItem{Text = "Low", Value = "Low"}

                };
            ViewBag.Priority = new SelectList(Priority, "Text", "Value");

            ViewBag.Project = db.ProjectTeams.Where(c => c.UserId == user.Id).Distinct().ToList();

            //ViewBag.ProjectId = new SelectList(db.ProjectTeams.Where(c => c.UserId == user.Id).Distinct(), "ProjectId", "Project.Name");

            string message = "";

            

            var tasks = new Task()
            {
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = task.Priority,
                AssignedByUser = user.Name,
                ProjectId = task.ProjectId,
                UserId = task.UserId
            };
            db.Tasks.Add(tasks);

            var rowAffected = db.SaveChanges();
            if(rowAffected > 0)
            {
                message = "Task add Successfull";
            }
            else
            {
                message = "Task add Failed";
            }

            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        public JsonResult GetTeamMemberByProject(int projectId)
        {
            var users = db.ProjectTeams.Where(c => c.ProjectId == projectId).Select(a => new
            {
                id = a.UserId,
                name = a.User.Name
            });
            
            return Json(users, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AddComment(int projectId)
        {
            var user = Session["user"] as User;
            if(user != null && user.UserDesignationId != 1)
            {
                var any = db.ProjectTeams.Any(c => c.ProjectId == projectId && c.UserId == user.Id);
                if (any)
                {
                    ViewBag.Project = db.ProjectTeams.Where(c => c.UserId == user.Id).Distinct().ToList();
                    ViewBag.Message = null;
                    return View();
                }
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult AddComment(Comment comments, int projectId)
        {
            var user = Session["user"] as User;
            var any = db.ProjectTeams.Any(c => c.ProjectId == projectId && c.UserId == user.Id);
            if (any)
            {
                ViewBag.Project = db.ProjectTeams.Where(c => c.UserId == user.Id).Distinct().ToList();

                string message = "";

                var com = new Comment()
                {
                    Value = comments.Value,
                    dateTime = DateTime.Now,
                    CommentBy = user.Name,
                    TaskId = comments.TaskId
                };


                db.Comments.Add(com);
                var rowAffected = db.SaveChanges();
                if(rowAffected > 0)
                {
                    message = "Comment add Successfull";
                }
                else
                {
                    message = "Comment add Failed";
                }


                ViewBag.Message = message;
                return View();
            }

            return RedirectToAction("Login", "Account");
        }


        [HttpPost]
        public JsonResult GetTaskByProject(int projectId)
        {
            var task = db.Tasks.Where(c => c.ProjectId == projectId).Select(a => new
            {
                id = a.Id,
                description = a.Description
            });

            return Json(task, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewComments(int id)
        {
            var user = Session["user"] as User;
            if(user != null && user.UserDesignationId != 1)
            {
                var comments = db.Comments.Where(c => c.TaskId == id).ToList();
                ViewBag.AllComment = comments;
                return View();
            }
            return RedirectToAction("Login", "Account");
        }
    }
}