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
    public class AssetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Assets
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            var cats = project.Categories;
            
            ViewBag.Project = project;
            
            if (cats.Count == 0)
            {
                return View(new List<Asset>());
            }
            var assets = new List<Asset>();
            foreach (var cat in cats)
            {
                assets.AddRange(cat.Assets);
            }
            return View(assets);
        }

        // GET: Assets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset = db.Assets.Find(id);
            if (asset == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = asset.Category.Project.Id;
            return View(asset);
        }

        // GET: Assets/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.Project = project;
            ViewBag.CategoryId = new SelectList(project.Categories, "Id", "Name");
            return View();
        }

        // POST: Assets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CategoryId,Thumbnail")] Asset asset)
        {
            if (ModelState.IsValid)
            {
                asset.DateTimeCreated = DateTime.Now;
                db.Assets.Add(asset);
                db.SaveChanges();
                var project = db.Categories.Find(asset.CategoryId).Project;
                return RedirectToAction("Index", new { id=project.Id });
            }
            var cat = db.Categories.Find(asset.CategoryId);
            ViewBag.Project = db.Projects.Find(cat.ProjectId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", asset.CategoryId);
            return View(asset);
        }

        // GET: Assets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset = db.Assets.Find(id);
            if (asset == null)
            {
                return HttpNotFound();
            }
            var project = asset.Category.Project;
            ViewBag.Project = project;
            ViewBag.CategoryId = new SelectList(project.Categories, "Id", "Name", asset.CategoryId);
            return View(asset);
        }

        // POST: Assets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CategoryId,Thumbnail,DateTimeCreated")] Asset asset)
        {
            if (ModelState.IsValid)
            {
                db.Entry(asset).State = EntityState.Modified;
                db.SaveChanges();
                var project = db.Categories.Find(asset.CategoryId).Project;
                return RedirectToAction("Index", new { id=project.Id });
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", asset.CategoryId);
            ViewBag.Project = asset.Category.Project;
            return View(asset);
        }

        // GET: Assets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset = db.Assets.Find(id);
            if (asset == null)
            {
                return HttpNotFound();
            }
            ViewBag.Project = asset.Category.Project;
            return View(asset);
        }

        // POST: Assets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Asset asset = db.Assets.Find(id);
            var project = asset.Category.Project;
            db.Assets.Remove(asset);
            db.SaveChanges();
            return RedirectToAction("Index", new { id=project.Id });
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
