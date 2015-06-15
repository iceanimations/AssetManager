using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AssetManager.Models;
using System.IO;
using System.Web.Mvc;
using AssetManager.ViewModels;

namespace AssetManager.Utils
{
    public class Util
    {
        public static ApplicationDbContext db = new ApplicationDbContext();
        public static void ArchiveComponent(Component comp)
        {
            
        }

        public static bool IsAnonymous(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true))
            {
                return true;
            }
            return false;
        }

        public static string GetThumbnail(Controller controller, HttpPostedFileBase thumbnail, string name, string type)
        {
            var ext = Path.GetExtension(thumbnail.FileName);
            var path = @"~\\Content\\Thumbnails\\"+ type;
            var basePath = controller.Server.MapPath(path);
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);
            var thumbPath = Path.Combine(basePath, name) + ext;
            thumbnail.SaveAs(thumbPath);
            return Path.Combine(path, Path.GetFileName(thumbPath));
        }

        public static string GetAssetPath(Asset asset)
        {
            string path = Path.Combine(asset.Category.Project.ProjectType.LocationUNC, asset.Category.Project.Name,
                asset.Category.Name, asset.Name);
            return path;
        }

        public static string GetComponentPath(Component component)
        {
            string path = Path.Combine(component.Asset.Category.Project.ProjectType.LocationUNC,
                component.Asset.Category.Project.Name, component.Asset.Category.Name, component.Asset.Name);
            path = Path.Combine(path, component.Name);
            return path;
        }

        public static bool isAuthorized(string username, Project project)
        {
            foreach (var pr in db.ProjectRules.ToList())
                if (pr.User.Name == username && pr.ProjectId == project.Id)
                    return true;
            return false;
        }

        public static bool isAuthorized(string username, Category category)
        {
            foreach (var cr in db.CategoryRules.ToList())
                if (cr.User.Name == username && cr.CategoryId == category.Id)
                    return true;
            foreach (var pr in db.ProjectRules.ToList())
                if (pr.ProjectId == category.ProjectId && pr.User.Name == username)
                    return true;
            return false;
        }

        public static bool isAuthorized(string username, Asset asset)
        {
            foreach (var ar in db.AssetRules.ToList())
                if (ar.User.Name == username && ar.AssetId == asset.Id)
                    return true;
            foreach (var cr in db.CategoryRules.ToList())
                if (cr.User.Name == username && cr.CategoryId == asset.CategoryId)
                    return true;
            foreach (var pr in db.ProjectRules.ToList())
                if (pr.User.Name == username && pr.ProjectId == asset.Category.Project.Id)
                    return true;
            return false;
        }

        public static bool isAuthorized(string username, Component component)
        {
            foreach (var cr in db.ComponentRules.ToList())
                if (cr.User.Name == username && cr.ComponentId == component.Id)
                    return true;
            foreach (var ar in db.AssetRules.ToList())
                if (ar.AssetId == component.AssetId && ar.User.Name == username)
                    return true;
            foreach (var cr in db.CategoryRules.ToList())
                if (cr.User.Name == username && component.Asset.Category.Id == cr.CategoryId)
                    return true;
            foreach (var pr in db.ProjectRules.ToList())
                if (pr.User.Name == username && pr.ProjectId == component.Asset.Category.Project.Id)
                    return true;
            return false;
        }

        public static bool IsNameEqual(string s1, string s2)
        {
            if (s1.ToLower().Split('\\').Last() == s2.ToLower().Split('\\').Last())
                return true;
            return false;
        }

        public static string GetArchivePath(Component component, DateTime dateTime)
        {
            string archivePath = Path.Combine(Path.GetDirectoryName(component.FilePath), "Archive", dateTime.ToString().Replace('/', '-').Replace(" ", "__").Replace(':', '.'));
            Directory.CreateDirectory(archivePath);
            return archivePath;
        }
    }
}