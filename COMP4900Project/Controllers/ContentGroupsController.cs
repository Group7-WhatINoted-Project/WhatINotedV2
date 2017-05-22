using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using COMP4900Project.Models;
using System.Web.Script.Serialization;

namespace COMP4900Project.Controllers
{
    public class ContentGroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ContentGroups
        public ActionResult Index()
        {
            return View(db.ContentGroups.ToList());
        }

        // GET: ContentGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentGroup contentGroup = db.ContentGroups.Find(id);
            if (contentGroup == null)
            {
                return HttpNotFound();
            }
            return View(contentGroup);
        }

        // GET: ContentGroups/Create
        public ActionResult Create2(int groupid)
        {
            ViewBag.ContentId = new SelectList(db.Contents, "ContentId", "Text");
            return View();
        }

        // POST: ContentGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContentGroupId,ContentId,GroupId")] ContentGroup contentGroup)
        {
            if (ModelState.IsValid)
            {
                db.ContentGroups.Add(contentGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ContentId = new SelectList(db.Contents, "ContentId", "Text", contentGroup.ContentId);
            ViewBag.GroupId = new SelectList(db.Groups, "GroupId", "GroupName", contentGroup.GroupId);
            return View(contentGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2([Bind(Include = "ContentId,GroupId")] ContentGroup contentGroup)
        {
            if (ModelState.IsValid)
            {
                db.ContentGroups.Add(contentGroup);
                db.SaveChanges();
                return RedirectToAction("Details", "Groups", new { id = contentGroup.GroupId });
            }

            ViewBag.ContentId = new SelectList(db.Contents, "ContentId", "Text", contentGroup.ContentId);
            ViewBag.GroupId = new SelectList(db.Groups, "GroupId", "GroupName", contentGroup.GroupId);
            return View(contentGroup);
        }

        // GET: ContentGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentGroup contentGroup = db.ContentGroups.Find(id);
            if (contentGroup == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContentId = new SelectList(db.Contents, "ContentId", "Text", contentGroup.ContentId);
            ViewBag.GroupId = new SelectList(db.Groups, "GroupId", "GroupName", contentGroup.GroupId);
            return View(contentGroup);
        }

        // POST: ContentGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContentGroupId,ContentId,GroupId")] ContentGroup contentGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contentGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContentId = new SelectList(db.Contents, "ContentId", "Text", contentGroup.ContentId);
            ViewBag.GroupId = new SelectList(db.Groups, "GroupId", "GroupName", contentGroup.GroupId);
            return View(contentGroup);
        }

        // GET: ContentGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentGroup contentGroup = db.ContentGroups.Find(id);
            if (contentGroup == null)
            {
                return HttpNotFound();
            }
            return View(contentGroup);
        }

        // POST: ContentGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContentGroup contentGroup = db.ContentGroups.Find(id);
            db.ContentGroups.Remove(contentGroup);
            db.SaveChanges();
            return RedirectToAction("Index", "UserGroups");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }




        public string GetGroupContents(int groupid)
        {
            var groupContents = db.ContentGroups.Include(u => u.Content).Include(u => u.Group).Where(u => u.GroupId == groupid);

            JsonResult jsonresult = Json(
                groupContents.Select(x => new
                {
                    ContentId = x.ContentId,
                    ContentGroupId = x.ContentGroupId,
                    Note = x.Content.Note,
                    Reference = x.Content.Reference,
                    TimeUpdated = x.Content.TimeUpdated
                }));

            string json = new JavaScriptSerializer().Serialize(jsonresult);
            return json;
        }
    }
}
