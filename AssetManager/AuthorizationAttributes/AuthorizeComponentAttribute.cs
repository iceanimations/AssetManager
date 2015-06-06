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
    public class AuthorizeComponentAttribute : ActionFilterAttribute
    {
        ApplicationDbContext db = new ApplicationDbContext();

        ~AuthorizeComponentAttribute()
        {
            ((IDisposable)db).Dispose();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            foreach (var param in filterContext.ActionParameters)
            {
                if (param.Value is ComponentViewModel)
                {
                    var model = param.Value as ComponentViewModel;
                    Asset asset = db.Assets.Find(model.AssetId);
                    bool isAuthorized = Util.isAuthorized(filterContext.HttpContext.User.Identity.Name, asset);
                    if (!isAuthorized)
                        filterContext.Result = new HttpUnauthorizedResult();
                }
                else if (param.Value is int)
                {
                    Component component = db.Components.Find(param.Value);
                    if (component != null)
                    {
                        bool isAuthorized = Util.isAuthorized(filterContext.HttpContext.User.Identity.Name, component);
                        if (!isAuthorized)
                            filterContext.Result = new HttpUnauthorizedResult();
                    }
                }
            }
        }
    }
}