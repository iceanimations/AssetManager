using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetManager.Models;
using AssetManager.Utils;
using AssetManager.ViewModels;

namespace AssetManager.Authorization
{
    public class AuthorizeComponentArchiveAttribute : ActionFilterAttribute
    {
        ApplicationDbContext db = new ApplicationDbContext();

        ~AuthorizeComponentArchiveAttribute()
        {
            ((IDisposable)db).Dispose();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (!Util.IsAnonymous(filterContext))
            {
                foreach (var param in filterContext.ActionParameters)
                {
                    ComponentArchive archive = db.ComponentArchives.Find(param.Value);
                    if (archive != null)
                    {
                        Component component = archive.Component;
                        if (component != null)
                        {
                            bool isAuthorized = Util.isAuthorized(filterContext.HttpContext.User.Identity.Name, component);
                            var project = db.Projects.First();
                            if (!isAuthorized)
                                filterContext.Result = new HttpUnauthorizedResult();
                        }
                    }
                }
            }
        }
    }
}