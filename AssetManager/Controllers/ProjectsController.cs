using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using AssetManager.Models;
using AssetManager.ViewModels;
using AssetManager.Utils;

namespace AssetManager.Controllers
{
    [Authorize(Users=@"ICEANIMATIONS\qurban.ali,ICEANIMATIONS\sarmad.mushtaq")]
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            ViewBag.ProjectTypeId = new SelectList(db.ProjectTypes, "Id", "Type");
            return View(new ProjectViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Thumbnail,Description,ProjectTypeId,UserIds,DateTimeCreated")] ProjectViewModel viewModelProject)
        {
            if (ModelState.IsValid)
            {
                // Handle file system
                try
                {
                    var basePath = db.ProjectTypes.Find(viewModelProject.ProjectTypeId).LocationUNC;
                    Directory.CreateDirectory(Path.Combine(basePath, viewModelProject.Name));
                }
                catch (Exception ex)
                {
                    return Content("Error: Could not create specified directory at " + db.ProjectTypes.Find(viewModelProject.ProjectTypeId).LocationDisplay +
                        " \nMake sure that the network is working properly.\n\n" + ex.ToString());
                }
                // create project in database
                var project = new Project
                {
                    Name = viewModelProject.Name,
                    Description = viewModelProject.Description,
                    DateTimeCreated = DateTime.Now,
                    ProjectTypeId = viewModelProject.ProjectTypeId
                };
                if (viewModelProject.Thumbnail != null)
                    project.Thumbnail = Util.GetThumbnail(this, viewModelProject.Thumbnail, viewModelProject.Name, "Projects");
                else
                    project.Thumbnail = "~/Content/Images/no-thumbnail.png";
                db.Projects.Add(project);
                db.SaveChanges();
                // create project rules
                var users = viewModelProject.UserIds;
                if (users != null)
                {
                    foreach (var id in users)
                    {
                        var projectRule = new ProjectRule {ProjectId=project.Id, UserId=id};
                        db.ProjectRules.Add(projectRule);
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.ProjectTypeId = new SelectList(db.ProjectTypes, "Id", "Type", viewModelProject.ProjectTypeId);
            return View(viewModelProject);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            var viewModelProject = new ProjectViewModel
            {
                Id=project.Id,
                Name=project.Name,
                Description=project.Description,
                ProjectTypeId=project.ProjectTypeId,
                DateTimeCreated=project.DateTimeCreated
            };
            List<int> userIds = new List<int>();
            foreach (var pr in project.ProjectRules)
            {
                userIds.Add(pr.UserId);
            }
            viewModelProject.UserIds = userIds.ToArray();
            ViewBag.ProjectTypeId = new SelectList(db.ProjectTypes, "Id", "Type", project.ProjectTypeId);
            return View(viewModelProject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Thumbnail,Description,ProjectTypeId,UserIds,DateTimeCreated")] ProjectViewModel viewModelProject)
        {
            if (ModelState.IsValid)
            {
                var project = db.Projects.Find(viewModelProject.Id);
                project.Name = viewModelProject.Name;
                project.Description = viewModelProject.Description;
                project.ProjectTypeId = viewModelProject.ProjectTypeId;
                project.DateTimeCreated = viewModelProject.DateTimeCreated;
                if (viewModelProject.Thumbnail != null)
                    project.Thumbnail = Util.GetThumbnail(this, viewModelProject.Thumbnail, viewModelProject.Name, "Projects");
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                var pr = db.ProjectRules.ToList();
                foreach (var p in pr)
                {
                    if (p.ProjectId == project.Id)
                        db.ProjectRules.Remove(p);
                }
                db.SaveChanges();
                if (viewModelProject.UserIds != null)
                {
                    for (var i = 0; i < viewModelProject.UserIds.Length; i++ )
                    {
                        db.ProjectRules.Add(new ProjectRule { ProjectId = project.Id, UserId = viewModelProject.UserIds[i] });
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.ProjectTypeId = new SelectList(db.ProjectTypes, "Id", "Type", viewModelProject.ProjectTypeId);
            return View(viewModelProject);
        }

        public ActionResult EditRules(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            var viewModelProject = new ProjectViewModel
            {
                Id = project.Id,
                Name = project.Name
            };
            List<int> userIds = new List<int>();
            foreach (var pr in project.ProjectRules)
            {
                userIds.Add(pr.UserId);
            }
            viewModelProject.UserIds = userIds.ToArray();
            return View(viewModelProject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRules([Bind(Include = "Id,UserIds")] ProjectViewModel viewModelProject)
        {
            var pr = db.ProjectRules.ToList();
            foreach (var p in pr)
            {
                if (p.ProjectId == viewModelProject.Id)
                    db.ProjectRules.Remove(p);
            }
            db.SaveChanges();
            if (viewModelProject.UserIds != null)
            {
                for (var i = 0; i < viewModelProject.UserIds.Length; i++)
                {
                    db.ProjectRules.Add(new ProjectRule { ProjectId = viewModelProject.Id, UserId = viewModelProject.UserIds[i] });
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
