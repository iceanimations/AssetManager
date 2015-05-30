using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AssetManager.Models;
using AssetManager.ViewModels;

namespace AssetManager.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

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
                // create the project
                var project = new Project();
                project.Name = viewModelProject.Name;
                project.Description = viewModelProject.Description;
                project.DateTimeCreated = DateTime.Now;
                project.ProjectTypeId = viewModelProject.ProjectTypeId;
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

        // GET: Projects/Edit/5
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
                var project = new Project
                {
                    Id=viewModelProject.Id,
                    Name = viewModelProject.Name,
                    Description = viewModelProject.Description,
                    ProjectTypeId = viewModelProject.ProjectTypeId,
                    DateTimeCreated = viewModelProject.DateTimeCreated
                };
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                var pr = db.ProjectRules.ToList();
                foreach (var p in pr)
                {
                    if (p.ProjectId == project.Id)
                        db.ProjectRules.Remove(p);
                }
                db.SaveChanges();
                for (var i = 0; i < viewModelProject.UserIds.Length; i++ )
                {
                    db.ProjectRules.Add(new ProjectRule { ProjectId = project.Id, UserId = viewModelProject.UserIds[i] });
                }
                db.SaveChanges();
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
            for (var i = 0; i < viewModelProject.UserIds.Length; i++)
            {
                db.ProjectRules.Add(new ProjectRule { ProjectId = viewModelProject.Id, UserId = viewModelProject.UserIds[i] });
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Projects/Delete/5
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

        // POST: Projects/Delete/5
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
