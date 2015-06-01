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
    }
}