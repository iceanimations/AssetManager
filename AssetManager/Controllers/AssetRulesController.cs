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
            return View(db.AssetRules.ToList());
        }

        // GET: AssetRules/Details/5
        public ActionResult Details(int? assetId, int? userId)
        {
            if (assetId == null || userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetRule assetRule = db.AssetRules.Find(assetId, userId);
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
        public ActionResult Edit(int? assetId, int? userId)
        {
            if (assetId == null || userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetRule assetRule = db.AssetRules.Find(assetId, userId);
            if (assetRule == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssetId = new SelectList(db.Assets, "Id", "Name", assetRule.AssetId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", assetRule.UserId);
            return View(assetRule);
        }

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
        public ActionResult Delete(int? assetId, int? userId)
        {
            if (assetId == null || userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetRule assetRule = db.AssetRules.Find(assetId, userId);
            if (assetRule == null)
            {
                return HttpNotFound();
            }
            return View(assetRule);
        }

        // POST: AssetRules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int assetId, int? userId)
        {
            AssetRule assetRule = db.AssetRules.Find(assetId, userId);
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
