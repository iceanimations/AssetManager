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
    public class CategoryRulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CategoryRules
        public ActionResult Index()
        {
            var categoryRules = db.CategoryRules.Include(c => c.Category).Include(c => c.User);
            return View(categoryRules.ToList());
        }

        // GET: CategoryRules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryRule categoryRule = db.CategoryRules.Find(id);
            if (categoryRule == null)
            {
                return HttpNotFound();
            }
            return View(categoryRule);
        }

        // GET: CategoryRules/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: CategoryRules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryId,UserId")] CategoryRule categoryRule)
        {
            if (ModelState.IsValid)
            {
                db.CategoryRules.Add(categoryRule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", categoryRule.CategoryId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", categoryRule.UserId);
            return View(categoryRule);
        }

        // GET: CategoryRules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryRule categoryRule = db.CategoryRules.Find(id);
            if (categoryRule == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", categoryRule.CategoryId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", categoryRule.UserId);
            return View(categoryRule);
        }

        // POST: CategoryRules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId,UserId")] CategoryRule categoryRule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoryRule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", categoryRule.CategoryId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", categoryRule.UserId);
            return View(categoryRule);
        }

        // GET: CategoryRules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryRule categoryRule = db.CategoryRules.Find(id);
            if (categoryRule == null)
            {
                return HttpNotFound();
            }
            return View(categoryRule);
        }

        // POST: CategoryRules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CategoryRule categoryRule = db.CategoryRules.Find(id);
            db.CategoryRules.Remove(categoryRule);
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
