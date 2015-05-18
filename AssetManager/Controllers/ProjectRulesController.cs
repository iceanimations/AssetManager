using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AssetManager.Models;

namespace AssetManager.Controllers
{
    public class ProjectRulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProjectRules
        public ActionResult Index()
        {
            return View(db.ProjectRules.ToList());
        }

        // GET: ProjectRules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectRule projectRule = db.ProjectRules.Find(id);
            if (projectRule == null)
            {
                return HttpNotFound();
            }
            return View(projectRule);
        }

        // GET: ProjectRules/Create
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: ProjectRules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId,UserId")] ProjectRule projectRule)
        {
            if (ModelState.IsValid)
            {
                db.ProjectRules.Add(projectRule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", projectRule.ProjectId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", projectRule.UserId);
            return View(projectRule);
        }

        // GET: ProjectRules/Edit/5
        public ActionResult Edit(int? projectId, int? userId)
        {
            if (projectId == null || userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectRule projectRule = db.ProjectRules.Find(projectId, userId);
            if (projectRule == null)
            {
                return HttpNotFound();
            }
            ViewBag.Project = db.Projects.Find(projectRule.ProjectId);
            ViewBag.User = db.Users.Find(projectRule.UserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", projectRule.ProjectId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", projectRule.UserId);
            return View(projectRule);
        }

        // POST: ProjectRules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectId,UserId")] ProjectRule projectRule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectRule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Project = db.Projects.Find(projectRule.ProjectId);
            ViewBag.User = db.Users.Find(projectRule.UserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", projectRule.ProjectId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", projectRule.UserId);
            return View(projectRule);
        }

        // GET: ProjectRules/Delete/5
        public ActionResult Delete(int? projectId, int? userId)
        {
            if (projectId == null || userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectRule projectRule = db.ProjectRules.Find(projectId, userId);
            if (projectRule == null)
            {
                return HttpNotFound();
            }
            ViewBag.Project = db.Projects.Find(projectRule.ProjectId);
            ViewBag.User = db.Users.Find(projectRule.UserId);
            return View(projectRule);
        }

        // POST: ProjectRules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int projectId, int userId)
        {
            ProjectRule projectRule = db.ProjectRules.Find(projectId, userId);
            db.ProjectRules.Remove(projectRule);
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
