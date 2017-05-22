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

namespace COMP4900Project.Controllers
{
    public class UserGroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserGroups
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                string userid = User.Identity.GetUserId();
                string username = User.Identity.GetUserName();
                var userGroups = db.UserGroups.Include(u => u.Group).Include(u => u.User).Where(f => f.UserId == userid);

                return View(userGroups.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: UserGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserGroup userGroup = db.UserGroups.Find(id);
            if (userGroup == null)
            {
                return HttpNotFound();
            }
            return View(userGroup);
        }

        // GET: UserGroups/Create
        public ActionResult Create()
        {
            ViewBag.GroupId = new SelectList(db.Groups, "GroupId", "GroupName");
            string userid = User.Identity.GetUserId();
            UserGroup usergroup = new UserGroup(userid, 0);
            return View(usergroup);
        }

        // POST: UserGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserGroupId,UserId,GroupId")] UserGroup userGroup)
        {
            if (ModelState.IsValid)
            {
                db.UserGroups.Add(userGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GroupId = new SelectList(db.Groups, "GroupId", "GroupName", userGroup.GroupId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userGroup.UserId);
            return View(userGroup);
        }

        // GET: UserGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserGroup userGroup = db.UserGroups.Find(id);
            if (userGroup == null)
            {
                return HttpNotFound();
            }
            ViewBag.GroupId = new SelectList(db.Groups, "GroupId", "GroupName", userGroup.GroupId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userGroup.UserId);
            return View(userGroup);
        }

        // POST: UserGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserGroupId,UserId,GroupId")] UserGroup userGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GroupId = new SelectList(db.Groups, "GroupId", "GroupName", userGroup.GroupId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userGroup.UserId);
            return View(userGroup);
        }

        // GET: UserGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserGroup userGroup = db.UserGroups.Find(id);
            if (userGroup == null)
            {
                return HttpNotFound();
            }
            return View(userGroup);
        }

        // POST: UserGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserGroup userGroup = db.UserGroups.Find(id);
            db.UserGroups.Remove(userGroup);
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
    }
}
