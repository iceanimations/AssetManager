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
using AssetManager.Utils;

namespace AssetManager.Controllers
{
    public class AssetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Assets
        public ActionResult Index(int? id, string SearchString)
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
            if (SearchString != null)
            {
                var newAssets = new List<Asset>();
                foreach (var asset in assets)
                {
                    if (asset.Name.ToLower().Contains(SearchString.ToLower()))
                    {
                        newAssets.Add(asset);
                    }
                }
                assets = newAssets;
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
            return View(new AssetViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Bind(Include = "Id,Name,CategoryId,Thumbnail,UserIds")] 
        public ActionResult Create(AssetViewModel viewModelAsset)
        {
            ViewBag.Success = false; // to show the fadeIn msg after successful submission
            if (ModelState.IsValid)
            {
                var asset = new Asset
                {
                    Name=viewModelAsset.Name,
                    CategoryId=viewModelAsset.CategoryId,
                    DateTimeCreated = DateTime.Now
                };
                if (viewModelAsset.Thumbnail != null)
                    asset.Thumbnail = Util.GetThumbnail(this, viewModelAsset.Thumbnail, viewModelAsset.Name, "Assets");
                else
                    asset.Thumbnail = "http://placehold.it/500/CCCCCC&amp&text="+ asset.Name;
                db.Assets.Add(asset);
                db.SaveChanges();
                if (viewModelAsset.UserIds != null)
                {
                    foreach (var uid in viewModelAsset.UserIds)
                    {
                        var ar = new AssetRule
                        {
                            AssetId = asset.Id,
                            UserId = uid
                        };
                        db.AssetRules.Add(ar);
                    }
                    db.SaveChanges();
                }
                //var project = db.Categories.Find(asset.CategoryId).Project;
                //return RedirectToAction("Index", new { id=project.Id });
                ViewBag.Success = true;
            }
            var cat = db.Categories.Find(viewModelAsset.CategoryId);
            ViewBag.Project = db.Projects.Find(cat.ProjectId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", viewModelAsset.CategoryId);
            return View(viewModelAsset);
        }

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
            var model = new AssetViewModel
            {
                Id = asset.Id,
                Name = asset.Name,
                //Thumbnail = asset.Thumbnail,
                CategoryId = asset.CategoryId,
                DateTimeCreated = asset.DateTimeCreated
            };
            var UserIds = new List<int>();
            foreach (var ar in db.AssetRules.ToList())
                if (ar.AssetId == asset.Id)
                    UserIds.Add(ar.UserId);
            model.UserIds = UserIds.ToArray();
            var project = asset.Category.Project;
            ViewBag.Project = project;
            ViewBag.CategoryId = new SelectList(project.Categories, "Id", "Name", asset.CategoryId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CategoryId,Thumbnail,UserIds, DateTimeCreated")] AssetViewModel viewModelAsset)
        {
            if (ModelState.IsValid)
            {
                var asset = db.Assets.Find(viewModelAsset.Id);
                asset.Name = viewModelAsset.Name;
                asset.CategoryId = viewModelAsset.CategoryId;
                asset.DateTimeCreated = viewModelAsset.DateTimeCreated;
                if (viewModelAsset.Thumbnail != null)
                    asset.Thumbnail = Util.GetThumbnail(this, viewModelAsset.Thumbnail, viewModelAsset.Name, "Assets");
                else
                    if (asset.Thumbnail.StartsWith("http://placehold.it"))
                        asset.Thumbnail = "http://placehold.it/500/CCCCCC&amp&text=" + viewModelAsset.Name;
                db.Entry(asset).State = EntityState.Modified;
                db.SaveChanges();
                if (viewModelAsset.UserIds != null)
                {
                    foreach (var ar in db.AssetRules.ToList())
                        if (ar.AssetId == viewModelAsset.Id)
                            db.AssetRules.Remove(ar);
                    db.SaveChanges();
                    foreach (var uid in viewModelAsset.UserIds)
                        db.AssetRules.Add(new AssetRule
                        {
                            UserId = uid,
                            AssetId = viewModelAsset.Id
                        });
                    db.SaveChanges();
                }
                var project = db.Categories.Find(viewModelAsset.CategoryId).Project;
                return RedirectToAction("Index", new { id=project.Id });
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", viewModelAsset.CategoryId);
            ViewBag.Project = db.Assets.Find(viewModelAsset.Id).Category.Project;
            return View(viewModelAsset);
        }

        public ActionResult EditRules(int? id)
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
            var model = new AssetViewModel
            {
                Id = asset.Id,
                Name = asset.Name
            };
            var UserIds = new List<int>();
            foreach (var ar in db.AssetRules.ToList())
            {
                if (ar.AssetId == asset.Id)
                {
                    UserIds.Add(ar.UserId);
                }
            }
            ViewBag.Project = asset.Category.Project;
            model.UserIds = UserIds.ToArray();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRules([Bind(Include = "Id,UserIds")] AssetViewModel viewModelAsset)
        {
            foreach (var ar in db.AssetRules.ToList())
            {
                if (ar.AssetId == viewModelAsset.Id)
                {
                    db.AssetRules.Remove(ar);
                }
            }
            db.SaveChanges();
            if (viewModelAsset.UserIds != null)
            {
                foreach (var uid in viewModelAsset.UserIds)
                {
                    db.AssetRules.Add(new AssetRule
                    {
                        UserId = uid,
                        AssetId = viewModelAsset.Id
                    });
                }
                db.SaveChanges();
            }
            var project = db.Assets.Find(viewModelAsset.Id).Category.Project;
            return RedirectToAction("Index", new { id = project.Id });
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
