using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetManager.Models;
using AssetManager.ViewModels;
using AssetManager.Utils;

namespace AssetManager.Authorization
{
    public class AuthorizeProjectAttribute : ActionFilterAttribute
    {
        ApplicationDbContext db = new ApplicationDbContext();

        ~AuthorizeProjectAttribute()
        {
            ((IDisposable)db).Dispose();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }
}