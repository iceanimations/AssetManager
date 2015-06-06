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
        public static void ArchiveComponent(Component comp)
        {
            
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
            foreach (var pr in project.ProjectRules)
                if (pr.User.Name == username)
                    return true;
            return false;
        }

        public static bool isAuthorized(string username, Category category)
        {
            foreach (var cr in category.CategoryRules)
                if (cr.User.Name == username)
                    return true;
            foreach (var pr in category.Project.ProjectRules)
                if (pr.User.Name == username)
                    return true;
            return false;
        }

        public static bool isAuthorized(string username, Asset asset)
        {
            foreach (var ar in asset.AssetRules)
                if (ar.User.Name == username)
                    return true;
            foreach (var cr in asset.Category.CategoryRules)
                if (cr.User.Name == username)
                    return true;
            foreach (var pr in asset.Category.Project.ProjectRules)
                if (pr.User.Name == username)
                    return true;
            return false;
        }

        public static bool isAuthorized(string username, Component component)
        {
            foreach (var cr in component.ComponentRules)
                if (cr.User.Name == username)
                    return true;
            foreach (var ar in component.Asset.AssetRules)
                if (ar.User.Name == username)
                    return true;
            foreach (var cr in component.Asset.Category.CategoryRules)
                if (cr.User.Name == username)
                    return true;
            foreach (var pr in component.Asset.Category.Project.ProjectRules)
                if (pr.User.Name == username)
                    return true;
            return false;
        }
    }
}