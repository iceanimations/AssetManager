using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssetManager.Models;

namespace AssetManager.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                bool flag = false;
                foreach (User user in db.Users.ToList())
                {
                    if (user.Name.Split('\\').Last() == User.Identity.Name.Split('\\').Last())
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    db.Users.Add(new User { Name = User.Identity.Name });
                    db.SaveChanges();
                }
            }
            return View(db.Projects.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "IDAM (ICE Digital Asset Manager)";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "";

            return View();
        }
    }
}