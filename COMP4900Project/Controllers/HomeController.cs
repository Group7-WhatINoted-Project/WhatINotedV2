using COMP4900Project.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COMP4900Project.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                ApplicationDbContext db = new ApplicationDbContext();

                string userid = User.Identity.GetUserId();
                string username = User.Identity.GetUserName();
                var userContents = db.UserContents.Where(f => f.UserId == userid);

                return View(userContents.ToList());
            }
            else
            {
                return View();
            }



            //ApplicationDbContext db = new ApplicationDbContext();
            //return View(db.UserContents.ToList());
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

        public ActionResult QA()
        {
            ViewBag.Message = "Your QA page.";

            return View();
        }
    }
}