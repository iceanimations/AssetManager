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
using System.IO;
using AssetManager.Utils;
using AssetManager.Authorization;

namespace AssetManager.Controllers
{
    [AuthorizeComponent]
    public class ComponentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
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

        [AllowAnonymous]
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
            ViewBag.FileName = Path.GetFileName(component.FilePath);
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
        [ActionName("Create")]
        public ActionResult CreatePost(ComponentViewModel viewModelComponent)
        {
            var asset = db.Assets.Find(viewModelComponent.AssetId);
            if (ModelState.IsValid)
            {
                // create components in file system
                string path = "";
                try
                {
                    path = Util.GetAssetPath(asset);
                    path = Path.Combine(path, viewModelComponent.Name);
                    Directory.CreateDirectory(path);
                }
                catch(Exception ex)
                {
                    return Content(ex.ToString());
                }
                var component = new Component
                {
                    Name = viewModelComponent.Name,
                    AssetId = viewModelComponent.AssetId,
                    Locked = viewModelComponent.Locked,
                    Description = viewModelComponent.Description,
                    DateTimeCreated = DateTime.Now,
                    DateTimeUpdated = DateTime.Now
                };
                // save the uploaded maya file
                if (viewModelComponent.UploadedFile != null)
                {
                    try
                    {
                        string ext = Path.GetExtension(viewModelComponent.UploadedFile.FileName);
                        path = Path.Combine(path, string.Join("_", new string[] { asset.Name, viewModelComponent.Name })) + ext;
                        viewModelComponent.UploadedFile.SaveAs(path);
                        component.FilePath = path;
                    }
                    catch(Exception ex)
                    {
                        return Content(ex.ToString());
                    }
                }
                db.Components.Add(component);
                db.SaveChanges();
                // create rules
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
                if (viewModelComponent.UploadedFile != null)
                {
                    try
                    {
                        string ext = Path.GetExtension(viewModelComponent.UploadedFile.FileName);
                        string path = Util.GetComponentPath(component);
                        path = Path.Combine(path, string.Join("_", new string[] { component.Asset.Name, component.Name })) + ext;
                        viewModelComponent.UploadedFile.SaveAs(path);
                        component.FilePath = path;
                    }
                    catch(Exception ex)
                    {
                        return Content(ex.ToString());
                    }
                }
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

        public ActionResult EditRules(int? id)
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
                Name = component.Name
            };
            var UserIds = new List<int>();
            foreach (var cr in db.ComponentRules.ToList())
            {
                if (cr.ComponentId == component.Id)
                {
                    UserIds.Add(cr.UserId);
                }
            }
            ViewBag.Project = component.Asset.Category.Project;
            ViewBag.Asset = component.Asset;
            model.UserIds = UserIds.ToArray();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRules([Bind(Include = "Id,UserIds")] ComponentViewModel viewModelComponent)
        {
            foreach (var ar in db.ComponentRules.ToList())
            {
                if (ar.ComponentId == viewModelComponent.Id)
                {
                    db.ComponentRules.Remove(ar);
                }
            }
            db.SaveChanges();
            if (viewModelComponent.UserIds != null)
            {
                foreach (var uid in viewModelComponent.UserIds)
                {
                    db.ComponentRules.Add(new ComponentRule
                    {
                        UserId = uid,
                        ComponentId = viewModelComponent.Id
                    });
                }
                db.SaveChanges();
            }
            var asset = db.Components.Find(viewModelComponent.Id).Asset;
            return RedirectToAction("Index", new { id = asset.Id });
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
