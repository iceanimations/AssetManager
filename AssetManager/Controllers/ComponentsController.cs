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
    public class ComponentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Components
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var asset = db.Assets.Find(id);
            if (asset == null)
            {
                HttpNotFound();
            }
            ViewBag.Project = asset.Category.Project;
            ViewBag.Asset = asset;
            return View(asset.Components.ToList());
        }

        // GET: Components/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Component component = db.Components.Find(id);
            if (component == null)
            {
                return HttpNotFound();
            }
            ViewBag.Project = component.Asset.Category.Project;
            ViewBag.Asset = component.Asset;
            return View(component);
        }

        // GET: Components/Create
        public ActionResult Create(int? id)
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
            ViewBag.Asset = asset;
            ViewBag.AssetId = new SelectList(db.Assets, "Id", "Name");
            return View();
        }

        // POST: Components/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,AssetId,FilePath,Locked,Description,DateTimeCreated,DateTimeUpdated")] Component component)
        {
            var asset = db.Assets.Find(component.AssetId);
            if (ModelState.IsValid)
            {
                component.DateTimeUpdated = component.DateTimeCreated = DateTime.Now;
                db.Components.Add(component);
                db.SaveChanges();
                return RedirectToAction("Index", new { id=asset.Id});
            }
            ViewBag.Project = asset.Category.Project;
            ViewBag.Asset = asset;
            ViewBag.AssetId = new SelectList(db.Assets, "Id", "Name", component.AssetId);
            return View(component);
        }

        // GET: Components/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Component component = db.Components.Find(id);
            if (component == null)
            {
                return HttpNotFound();
            }
            ViewBag.Project = component.Asset.Category.Project;
            ViewBag.Asset = component.Asset;
            ViewBag.AssetId = new SelectList(db.Assets, "Id", "Name", component.AssetId);
            return View(component);
        }

        // POST: Components/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,AssetId,FilePath,Locked,Description,DateTimeCreated,DateTimeUpdated")] Component component)
        {
            if (ModelState.IsValid)
            {
                component.DateTimeUpdated = DateTime.Now;
                db.Entry(component).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { db.Assets.Find(component.AssetId).Id });
            }
            ViewBag.Project = component.Asset.Category.Project;
            ViewBag.Asset = component.Asset;
            ViewBag.AssetId = new SelectList(db.Assets, "Id", "Name", component.AssetId);
            return View(component);
        }

        // GET: Components/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Component component = db.Components.Find(id);
            if (component == null)
            {
                return HttpNotFound();
            }
            ViewBag.Project = component.Asset.Category.Project;
            ViewBag.Asset = component.Asset;
            return View(component);
        }

        // POST: Components/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Component component = db.Components.Find(id);
            var asset = component.Asset;
            db.Components.Remove(component);
            db.SaveChanges();
            return RedirectToAction("Index", new { id=asset.Id });
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
