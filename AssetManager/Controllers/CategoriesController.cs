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
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        public ActionResult Index(int? ProjectId)
        {
            Project project;
            List<Category> categories = new List<Category>();
            if (ProjectId != null)
            {
                project = db.Projects.Find(ProjectId);
                ViewBag.ProjectName = project.Name;
                foreach (var cat in db.Categories)
                {
                    if (cat.ProjectId == ProjectId)
                    {
                        categories.Add(cat);
                    }
                }
            }
            else
            {
                categories.AddRange(db.Categories.ToList());
                ViewBag.ProjectName = "";
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ProjectId);
            return View(categories.ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            return View(new CategoryViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ProjectId,UserIds,DateTimeCreated")] CategoryViewModel viewModelCategory)
        {
            ViewBag.Success = false; // to show the fadeIn msg after successful submission
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Name=viewModelCategory.Name,
                    ProjectId=viewModelCategory.ProjectId,
                    DateTimeCreated = DateTime.Now
                };
                db.Categories.Add(category);
                db.SaveChanges();
                if (viewModelCategory.UserIds != null)
                {
                    foreach (var uid in viewModelCategory.UserIds)
                    {
                        var cr = new CategoryRule
                        {
                            CategoryId = category.Id,
                            UserId = uid
                        };
                        db.CategoryRules.Add(cr);
                    }
                    db.SaveChanges();
                }
                //return RedirectToAction("Index");
                ViewBag.Success = true;
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", viewModelCategory.ProjectId);
            return View(viewModelCategory);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            var model = new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                ProjectId = category.ProjectId,
                DateTimeCreated = category.DateTimeCreated
            };
            var UserIds = new List<int>();
            foreach (var cr in db.CategoryRules.ToList())
            {
                if (cr.CategoryId == category.Id)
                {
                    UserIds.Add(cr.UserId);
                }
            }
            model.UserIds = UserIds.ToArray();
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", category.ProjectId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ProjectId,UserIds,DateTimeCreated")] CategoryViewModel viewModelCategory)
        {
            if (ModelState.IsValid)
            {
                var category = db.Categories.Find(viewModelCategory.Id);
                category.Name = viewModelCategory.Name;
                category.ProjectId = viewModelCategory.ProjectId;
                category.DateTimeCreated = viewModelCategory.DateTimeCreated;
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                foreach (var cr in db.CategoryRules.ToList())
                {
                    if (cr.CategoryId == viewModelCategory.Id)
                    {
                        db.CategoryRules.Remove(cr);
                    }
                }
                db.SaveChanges();
                if (viewModelCategory.UserIds != null)
                {
                    foreach (var uid in viewModelCategory.UserIds)
                    {
                        db.CategoryRules.Add(new CategoryRule
                        {
                            UserId = uid,
                            CategoryId = viewModelCategory.Id
                        });
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", viewModelCategory.ProjectId);
            return View(viewModelCategory);
        }

        public ActionResult EditRules(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            var model = new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name
            };
            model.Project = category.Project;
            var UserIds = new List<int>();
            foreach (var cr in db.CategoryRules.ToList())
            {
                if (cr.CategoryId == category.Id)
                {
                    UserIds.Add(cr.UserId);
                }
            }
            model.UserIds = UserIds.ToArray();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRules([Bind(Include = "Id,UserIds")] CategoryViewModel viewModelCategory)
        {
            foreach (var cr in db.CategoryRules.ToList())
            {
                if (cr.CategoryId == viewModelCategory.Id)
                {
                    db.CategoryRules.Remove(cr);
                }
            }
            db.SaveChanges();
            if (viewModelCategory.UserIds != null)
            {
                foreach (var uid in viewModelCategory.UserIds)
                {
                    db.CategoryRules.Add(new CategoryRule
                    {
                        UserId = uid,
                        CategoryId = viewModelCategory.Id
                    });
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
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
