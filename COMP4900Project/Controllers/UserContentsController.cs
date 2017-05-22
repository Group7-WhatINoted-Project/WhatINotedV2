using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using COMP4900Project.Models;
using Microsoft.AspNet.Identity;
using System.Web.Script.Serialization;

namespace COMP4900Project.Controllers
{
    public class UserContentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserContents
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                string userid = User.Identity.GetUserId();
                string username = User.Identity.GetUserName();
                var userContents = db.UserContents.Include(u => u.Contents).Include(u => u.User).Where(f => f.UserId == userid);

                return View(userContents.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: UserContents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserContent userContent = db.UserContents.Find(id);
            if (userContent == null)
            {
                return HttpNotFound();
            }
            return View(userContent);
        }

        // GET: UserContents/Create
        public ActionResult Create2(int contentid)
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: UserContents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,ContentId")] UserContent userContent)
        {
            if (ModelState.IsValid)
            {
                db.UserContents.Add(userContent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ContentId = new SelectList(db.Contents, "ContentId", "Text", userContent.ContentId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userContent.UserId);
            return View(userContent);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2([Bind(Include = "UserId,ContentId")] UserContent userContent)
        {
            if (ModelState.IsValid)
            {
                db.UserContents.Add(userContent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ContentId = new SelectList(db.Contents, "ContentId", "Text", userContent.ContentId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userContent.UserId);
            return View(userContent);
        }

        // GET: UserContents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserContent userContent = db.UserContents.Find(id);
            if (userContent == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContentId = new SelectList(db.Contents, "ContentId", "Text", userContent.ContentId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userContent.UserId);
            return View(userContent);
        }

        // POST: UserContents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserContentId,UserId,ContentId")] UserContent userContent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userContent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContentId = new SelectList(db.Contents, "ContentId", "Text", userContent.ContentId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userContent.UserId);
            return View(userContent);
        }

        // GET: UserContents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserContent userContent = db.UserContents.Find(id);
            if (userContent == null)
            {
                return HttpNotFound();
            }
            return View(userContent);
        }

        // POST: UserContents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserContent userContent = db.UserContents.Find(id);
            db.UserContents.Remove(userContent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public string GetAllContents()
        {
            string userid = User.Identity.GetUserId();

            var userContents = db.UserContents.Include(u => u.Contents).Include(u => u.User).Where(f => f.UserId == userid);

            JsonResult jsonresult = Json(
                userContents.Select(x => new {
                    ContentId = x.ContentId,
                    UserContentId = x.UserContentId,
                    Note = x.Contents.Note,
                    Reference = x.Contents.Reference,
                    TimeUpdated = x.Contents.TimeUpdated
                }));

            string json = new JavaScriptSerializer().Serialize(jsonresult);
            return json;
        }


        public string GetRecentContents()
        {
            string userid = User.Identity.GetUserId();
            DateTime period = DateTime.Now.AddDays(-7);

            var userContents = db.UserContents.Include(u => u.Contents).Include(u => u.User).Where(f => f.UserId == userid).Where(f => f.Contents.TimeUpdated > period);
             
            JsonResult jsonresult = Json(
                userContents.Select(x => new {
                    ContentId = x.ContentId,
                    Note = x.Contents.Note,
                    Reference = x.Contents.Reference,
                    TimeUpdated = x.Contents.TimeUpdated
                }));

            string json = new JavaScriptSerializer().Serialize(jsonresult);
            return json;
        }
    }
}
