using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetManager.Models;
using System.Net;
using System.IO;
using AssetManager.Utils;
using System.Data.Entity;
using AssetManager.Authorization;

namespace AssetManager.Controllers
{
    [AuthorizeComponentArchive]
    public class ComponentArchivesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult Index(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Component component = db.Components.Find(id);
            if (component == null)
                return HttpNotFound();
            ViewBag.Project = component.Asset.Category.Project;
            ViewBag.Asset = component.Asset;
            ViewBag.Component = component;
            return View(component.ComponentArchives.ToList());
        }

        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ComponentArchive archive = db.ComponentArchives.Find(id);
            if (archive == null)
                return HttpNotFound();
            return View(archive);
        }

        [AllowAnonymous]
        public ActionResult Download(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ComponentArchive archive = db.ComponentArchives.Find(id);
            if (archive == null)
                return HttpNotFound();
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", Path.GetFileName(archive.FilePath)));
            Response.WriteFile(archive.FilePath);
            Response.End();
            return null;
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ComponentArchive archive = db.ComponentArchives.Find(id);
            if (archive == null)
                return HttpNotFound();
            return View(archive);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            ComponentArchive archive = db.ComponentArchives.Find(id);
            Component component = db.Components.Find(archive.ComponentId);
            db.ComponentArchives.Remove(archive);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = component.Id });
        }

        public ActionResult Revive(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ComponentArchive archive = db.ComponentArchives.Find(id);
            if (archive == null)
                return HttpNotFound();
            return View(archive);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Revive(int id)
        {
            ComponentArchive archive = db.ComponentArchives.Find(id);
            Component component = archive.Component;
            ComponentArchive newArchive = new ComponentArchive
            {
                ArchiveDate = DateTime.Now,
                ComponentId = component.Id,
                ComponentDateTimeCreated = component.DateTimeCreated,
                ComponentDateTimeUpdated = component.DateTimeUpdated
            };
            string newArchivePath = Util.GetArchivePath(component, newArchive.ArchiveDate);
            string newArchiveFilePath = Path.Combine(newArchivePath, Path.GetFileName(component.FilePath));
            System.IO.File.Move(component.FilePath, newArchiveFilePath);
            newArchive.FilePath = newArchiveFilePath;
            db.ComponentArchives.Add(newArchive);
            db.SaveChanges();
            string revivedPath = Path.Combine(Path.GetDirectoryName(component.FilePath), Path.GetFileName(archive.FilePath));
            System.IO.File.Move(archive.FilePath, revivedPath);
            component.FilePath = revivedPath;
            component.DateTimeCreated = archive.ComponentDateTimeCreated;
            component.DateTimeUpdated = archive.ComponentDateTimeUpdated;
            db.Entry(component).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { id = component.Id });
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