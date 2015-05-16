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
    public class ComponentRulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ComponentRules
        public ActionResult Index()
        {
            return View(db.ComponentRules.ToList());
        }

        // GET: ComponentRules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComponentRule componentRule = db.ComponentRules.Find(id);
            if (componentRule == null)
            {
                return HttpNotFound();
            }
            return View(componentRule);
        }

        // GET: ComponentRules/Create
        public ActionResult Create()
        {
            ViewBag.ComponentId = new SelectList(db.Components, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: ComponentRules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ComponentId,UserId")] ComponentRule componentRule)
        {
            if (ModelState.IsValid)
            {
                db.ComponentRules.Add(componentRule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ComponentId = new SelectList(db.Components, "Id", "Name", componentRule.ComponentId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", componentRule.UserId);
            return View(componentRule);
        }

        // GET: ComponentRules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComponentRule componentRule = db.ComponentRules.Find(id);
            if (componentRule == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComponentId = new SelectList(db.Components, "Id", "Name", componentRule.ComponentId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", componentRule.UserId);
            return View(componentRule);
        }

        // POST: ComponentRules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ComponentId,UserId")] ComponentRule componentRule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(componentRule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ComponentId = new SelectList(db.Components, "Id", "Name", componentRule.ComponentId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", componentRule.UserId);
            return View(componentRule);
        }

        // GET: ComponentRules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComponentRule componentRule = db.ComponentRules.Find(id);
            if (componentRule == null)
            {
                return HttpNotFound();
            }
            return View(componentRule);
        }

        // POST: ComponentRules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ComponentRule componentRule = db.ComponentRules.Find(id);
            db.ComponentRules.Remove(componentRule);
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
