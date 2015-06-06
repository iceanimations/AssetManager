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
    public class AuthorizeCategoryAttribute : ActionFilterAttribute
    {
        ApplicationDbContext db = new ApplicationDbContext();

        ~AuthorizeCategoryAttribute()
        {
            ((IDisposable)db).Dispose();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            foreach (var param in filterContext.ActionParameters)
            {
                if (param.Value is CategoryViewModel)
                {
                    var model = param.Value as CategoryViewModel;
                    Project project = db.Projects.Find(model.ProjectId);
                    bool isAuthorized = Util.isAuthorized(filterContext.HttpContext.User.Identity.Name, project);
                    if (!isAuthorized)
                        filterContext.Result = new HttpUnauthorizedResult();
                }
                else if (param.Value is int)
                {
                    Category category = db.Categories.Find(param.Value);
                    if (category != null)
                    {
                        bool isAuthorized = Util.isAuthorized(filterContext.HttpContext.User.Identity.Name, category);
                        if (!isAuthorized)
                            filterContext.Result = new HttpUnauthorizedResult();
                    }
                }
            }
        }
    }
}