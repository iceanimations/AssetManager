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
    public class AssetRulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AssetRules
        public ActionResult Index()
        {
            var assetRules = db.AssetRules.Include(a => a.Asset).Include(a => a.User);
            return View(assetRules.ToList());
        }

        // GET: AssetRules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetRule assetRule = db.AssetRules.Find(id);
            if (assetRule == null)
            {
                return HttpNotFound();
            }
            return View(assetRule);
        }

        // GET: AssetRules/Create
        public ActionResult Create()
        {
            ViewBag.AssetId = new SelectList(db.Assets, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: AssetRules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssetId,UserId")] AssetRule assetRule)
        {
            if (ModelState.IsValid)
            {
                db.AssetRules.Add(assetRule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssetId = new SelectList(db.Assets, "Id", "Name", assetRule.AssetId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", assetRule.UserId);
            return View(assetRule);
        }

        // GET: AssetRules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetRule assetRule = db.AssetRules.Find(id);
            if (assetRule == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssetId = new SelectList(db.Assets, "Id", "Name", assetRule.AssetId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", assetRule.UserId);
            return View(assetRule);
        }

        // POST: AssetRules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssetId,UserId")] AssetRule assetRule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assetRule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssetId = new SelectList(db.Assets, "Id", "Name", assetRule.AssetId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", assetRule.UserId);
            return View(assetRule);
        }

        // GET: AssetRules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetRule assetRule = db.AssetRules.Find(id);
            if (assetRule == null)
            {
                return HttpNotFound();
            }
            return View(assetRule);
        }

        // POST: AssetRules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AssetRule assetRule = db.AssetRules.Find(id);
            db.AssetRules.Remove(assetRule);
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
