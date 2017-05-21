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
using System.Drawing;
using Newtonsoft.Json.Linq;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;

namespace COMP4900Project.Controllers
{
    public class ContentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Contents
        public ActionResult Index()
        {
            string username = User.Identity.GetUserName();

            if (Request.IsAuthenticated && username == "Admin")
            {
                return View(db.Contents.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Contents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // GET: Contents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContentId,Text,Note,Reference")] Content content)
        {
            //string baseUrl = "https://openlibrary.org/api/books?bibkeys=ISBN:{0}&jscmd=details&format=json";
            //var url = string.Format(baseUrl, content.Reference);

            //var syncClient = new WebClient();
            //var data = syncClient.DownloadString(url);

            //JObject o = JObject.Parse(data);

            //string author = (string)o["ISBN:" + content.Reference]["details"]["authors"][0]["name"];
            //string[] authorArray = author.Split(' ');
            //string surname = authorArray.Last();
            //string initial = authorArray[0][0] + ".";

            //string publish_date = (string)o["ISBN:" + content.Reference]["details"]["publish_date"];

            //string title = (string)o["ISBN:" + content.Reference]["details"]["title"];

            //string publish_places = (string)o["ISBN:" + content.Reference]["details"]["publish_places"][0];
            //string[] publish_placesArray = publish_places.Split(' ');
            //string publish_city = publish_placesArray.First();
            //string publishers = (string)o["ISBN:" + content.Reference]["details"]["publishers"][0];

            //string citation = surname + ", " + initial + " (" + publish_date + "). <i>" +
            //    title + "</i> (p. pages_used). " + publish_city + ": " + publishers + ".";

            //content.Reference = citation;


            content.TimeUpdated = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Contents.Add(content);
                db.SaveChanges();
                //return RedirectToAction("Index");
                //return RedirectToAction("Create", "UserContents");

                string userid = User.Identity.GetUserId();
                int contentid = content.ContentId;

                var result = new UserContentsController().Create(new UserContent(userid, contentid));

                return RedirectToAction("Index", "UserContents");
            }

            return View(content);
        }

        // GET: Contents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        public ActionResult EditGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }


        public string GetNote(int? id)
        {
            //string userid = User.Identity.GetUserId();
            //DateTime period = DateTime.Now.AddDays(-7);

            var content = db.Contents.Where(f => f.ContentId == id);

            //var userContents = db.UserContents.Include(u => u.Contents).Include(u => u.User).Where(f => f.UserId == userid).Where(f => f.Contents.TimeUpdated > period);

            JsonResult jsonresult = Json(
                content.Select(x => new {
                    ContentId = x.ContentId,
                    Text = x.Text,
                    Note = x.Note,
                    Reference = x.Reference,
                    TimeUpdated = x.TimeUpdated
                }));

            string json = new JavaScriptSerializer().Serialize(jsonresult);
            return json;




            //if (id == null)
            //{
            //    return "";
            //}
            //Content content = db.Contents.Find(id);
            //if (content == null)
            //{
            //    return "";
            //}
            //return content.Note;
        }



        // POST: Contents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContentId,Text,Note,Reference")] Content content)
        {
            content.TimeUpdated = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Entry(content).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Index", "UserContents");
            }
            return View(content);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditGroup([Bind(Include = "ContentId,Text,Note,Reference")] Content content)
        {
            content.TimeUpdated = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Entry(content).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Index", "ContentGroups");
            }
            return View(content);
        }

        // GET: Contents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        public ActionResult DeleteGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // POST: Contents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Content content = db.Contents.Find(id);
            db.Contents.Remove(content);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("Index", "UserContents");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedGroup(int id)
        {
            Content content = db.Contents.Find(id);
            db.Contents.Remove(content);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("Index", "ContentGroups");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [ActionName("ExploreNote")]
        public ActionResult ExploreNote(int? id)
        {
            // this action will create text file 'note.txt' with data from
            // string variable 'string_with_your_data', which will be downloaded by
            // your browser

            //todo: add some data from your database into that string:
            Content content = db.Contents.Find(id);
             //remove the html tag in the note
            string temp = StripHTML(content.Note);
            var string_with_your_data = temp;
            var byteArray = Encoding.ASCII.GetBytes(string_with_your_data);
            var stream = new MemoryStream(byteArray);

            return File(stream, "text/plain", "Note.txt");
        }

        //explore the text file with text data
        [ActionName("ExploreText")]
        public ActionResult ExploreText(int? id)
        {
            // this action will create text file 'Text.txt' with data from
            // string variable 'string_with_your_data', which will be downloaded by
            // your browser

            //todo: add some data from your database into that string:
            Content content = db.Contents.Find(id);
            var string_with_your_data = content.Text;
            var byteArray = Encoding.ASCII.GetBytes(string_with_your_data);
            var stream = new MemoryStream(byteArray);

            return File(stream, "text/plain", "Text.txt");
        }

         //remove the html tag 
        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }




        private OCRTools.OCR OCR1;

        [HttpPost]
        public string OCR([Bind(Include = "ContentId,Text,Note,Reference")] Content content, HttpPostedFileBase ePic = null)
        {
            OCR1 = new OCRTools.OCR();
            OCR1.DefaultFolder = Server.MapPath("/bin");

            OCR1.BitmapImage = (Bitmap)Image.FromStream(ePic.InputStream, true, true);

            //OCR1.BitmapImageFile = ePic.FileName;
            OCR1.Process();
            content.Note = OCR1.Text;

            return content.Note;
        }



        [HttpPost]
        public string ISBN(string Reference1 = null, string pages = null, string style = null)
        {
            string baseUrl = "https://openlibrary.org/api/books?bibkeys=ISBN:{0}&jscmd=details&format=json";
            var url = string.Format(baseUrl, Reference1);

            var syncClient = new WebClient();
            var data = syncClient.DownloadString(url);

            JObject o = JObject.Parse(data);

            string author = (string)o["ISBN:" + Reference1]["details"]["authors"][0]["name"];
            string[] authorArray = author.Split(' ');
            string surname = authorArray.Last();
            string firstname = authorArray.First();
            string initial = authorArray[0][0] + ".";

            string publish_date = (string)o["ISBN:" + Reference1]["details"]["publish_date"];

            string title = (string)o["ISBN:" + Reference1]["details"]["title"];

            //string publish_places = (string)o["ISBN:" + Reference1]["details"]["publish_places"][0];
            string publish_places = "";
            string[] publish_placesArray = publish_places.Split(' ');
            string publish_city = publish_placesArray.First();
            string publishers = (string)o["ISBN:" + Reference1]["details"]["publishers"][0];

            string citation = "";

            if (style == "APA")
            {
                citation = surname + ", " + initial + " (" + publish_date + "). <i>" +
                    title + "</i> " + (pages == "" ? "" : "(p. " + pages + "). ") + publish_city + ": " + publishers + ".";
                
            }
            else
            {
                citation = surname + ", " + firstname + ". " + title + ". " + publishers +
                    ", " + publish_date + ".";
            }


            return citation;



            //OCR1 = new OCRTools.OCR();
            //OCR1.DefaultFolder = Server.MapPath("/bin");

            //OCR1.BitmapImage = (Bitmap)Image.FromStream(ePic.InputStream, true, true);

            ////OCR1.BitmapImageFile = ePic.FileName;
            //OCR1.Process();
            //content.Note = OCR1.Text;

            //return content.Note;
        }
    }
}
