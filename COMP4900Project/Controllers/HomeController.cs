using COMP4900Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COMP4900Project.Controllers
{
    //private ApplicationDbContext db = new ApplicationDbContext();

    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var userContents = db.UserContents.Include(u => u.Contents).Include(u => u.User);
            //return View(userContents.ToList());
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}