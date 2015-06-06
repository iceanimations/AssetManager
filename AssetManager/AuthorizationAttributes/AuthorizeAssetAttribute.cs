using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetManager.Utils;
using AssetManager.Models;
using AssetManager.ViewModels;


namespace AssetManager.Authorization
{
    public class AuthorizeAssetAttribute : ActionFilterAttribute
    {
        ApplicationDbContext db = new ApplicationDbContext();

        ~AuthorizeAssetAttribute()
        {
            ((IDisposable)db).Dispose();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            foreach (var param in filterContext.ActionParameters)
            {
                if (param.Value is AssetViewModel)
                {
                    var model = param.Value as AssetViewModel;
                    Category category = db.Categories.Find(model.CategoryId);
                    bool isAuthorized = Util.isAuthorized(filterContext.HttpContext.User.Identity.Name, category);
                    if (!isAuthorized)
                        filterContext.Result = new HttpUnauthorizedResult();
                }
                else if (param.Value is int)
                {
                    Asset asset = db.Assets.Find(param.Value);
                    if (asset != null)
                    {
                        bool isAuthorized = Util.isAuthorized(filterContext.HttpContext.User.Identity.Name, asset);
                        if (!isAuthorized)
                            filterContext.Result = new HttpUnauthorizedResult();
                    }
                }
            }
        }
    }
}