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
            var project = asset.Category.Project;
            var assets = new List<Asset>();
            foreach (var cat in project.Categories)
            {
                assets.AddRange(cat.Assets);
            }
            ViewBag.Project = project;
            ViewBag.Asset = asset;
            ViewBag.AssetId = new SelectList(assets, "Id", "Name");
            var model = new ComponentViewModel();
            model.Locked = true;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind(Include = "Id,Name,AssetId,UploadedFile,Locked,Description,UserIds,DateTimeCreated,DateTimeUpdated")] 
        public ActionResult Create(ComponentViewModel viewModelComponent)
        {
            //var file = Request.Files[0];
            var asset = db.Assets.Find(viewModelComponent.AssetId);
            if (ModelState.IsValid)
            {
                var component = new Component
                {
                    Name = viewModelComponent.Name,
                    AssetId = viewModelComponent.AssetId,
                    Locked = viewModelComponent.Locked,
                    Description = viewModelComponent.Description,
                    DateTimeCreated = DateTime.Now,
                    DateTimeUpdated = DateTime.Now
                };
                db.Components.Add(component);
                db.SaveChanges();
                if (viewModelComponent.UserIds != null)
                {
                    foreach (var uid in viewModelComponent.UserIds)
                    {
                        var cr = new ComponentRule
                        {
                            ComponentId=component.Id,
                            UserId=uid
                        };
                        db.ComponentRules.Add(cr);
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("Index", new { id=asset.Id});
            }
            ViewBag.Project = asset.Category.Project;
            ViewBag.Asset = asset;
            ViewBag.AssetId = new SelectList(db.Assets, "Id", "Name", viewModelComponent.AssetId);
            return View(viewModelComponent);
        }

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
            var model = new ComponentViewModel
            {
                Id = component.Id,
                Name = component.Name,
                AssetId = component.AssetId,
                FilePath = component.FilePath,
                Locked = component.Locked,
                Description = component.Description,
                DateTimeCreated = component.DateTimeCreated
            };
            var UserIds = new List<int>();
            foreach (var cr in component.ComponentRules)
            {
                UserIds.Add(cr.UserId);
            }
            model.UserIds = UserIds.ToArray();
            var project = component.Asset.Category.Project;
            var assets = new List<Asset>();
            foreach (var cat in project.Categories)
            {
                assets.AddRange(cat.Assets);
            }
            ViewBag.Project = project;
            ViewBag.Asset = component.Asset;
            ViewBag.AssetId = new SelectList(assets, "Id", "Name", component.AssetId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,AssetId,UploadedFile,Locked,Description,UserIds,DateTimeCreated,DateTimeUpdated")] ComponentViewModel viewModelComponent)
        {
            if (ModelState.IsValid)
            {
                var component = db.Components.Find(viewModelComponent.Id);
                component.Name = viewModelComponent.Name;
                component.AssetId = viewModelComponent.AssetId;
                component.FilePath = viewModelComponent.FilePath;
                component.Locked = viewModelComponent.Locked;
                component.Description = viewModelComponent.Description;
                component.DateTimeCreated = viewModelComponent.DateTimeCreated;
                component.DateTimeUpdated = DateTime.Now;
                db.Entry(component).State = EntityState.Modified;
                db.SaveChanges();
                foreach (var cr in db.ComponentRules.ToList())
                    if (cr.ComponentId == component.Id)
                        db.ComponentRules.Remove(cr);
                if (viewModelComponent.UserIds != null)
                {
                    foreach (var uid in viewModelComponent.UserIds)
                        db.ComponentRules.Add(new ComponentRule
                        {
                            UserId = uid,
                            ComponentId = component.Id
                        });
                    db.SaveChanges();
                }
                return RedirectToAction("Index", new { db.Assets.Find(component.AssetId).Id });
            }
            ViewBag.Asset = db.Components.Find(viewModelComponent.Id).Asset;
            ViewBag.Project = ViewBag.Asset.Category.Project;
            ViewBag.AssetId = new SelectList(db.Assets, "Id", "Name", viewModelComponent.AssetId);
            return View(viewModelComponent);
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
