using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetManager.Models;
using System.Net;
using System.IO;

namespace AssetManager.Controllers
{
    public class ComponentArchivesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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
            ViewBag.Project = archive.Component.Asset.Category.Project;
            ViewBag.Asset = archive.Component.Asset;
            ViewBag.Component = archive.Component;
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
    }
}