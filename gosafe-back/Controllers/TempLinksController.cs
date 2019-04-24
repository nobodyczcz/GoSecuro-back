using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using gosafe_back.Models;

namespace gosafe_back.Controllers
{
    public class TempLinksController : Controller
    {
        private Model1Container db = new Model1Container();

        // GET: TempLinks
        public ActionResult Index()
        {
            var tempLink = db.TempLink.Include(t => t.Journey).Include(t => t.UserProfile);
            return View(tempLink.ToList());
        }

        // GET: TempLinks/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempLink tempLink = db.TempLink.Find(id);
            if (tempLink == null)
            {
                return HttpNotFound();
            }
            return View(tempLink);
        }

        // GET: TempLinks/Create
        public ActionResult Create()
        {
            ViewBag.JourneyJourneyId = new SelectList(db.Journey, "JourneyId", "EndTime");
            ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address");
            return View();
        }

        // POST: TempLinks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TempLinkId,JourneyJourneyId,UserProfileId")] TempLink tempLink)
        {
            if (ModelState.IsValid)
            {
                db.TempLink.Add(tempLink);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JourneyJourneyId = new SelectList(db.Journey, "JourneyId", "EndTime", tempLink.JourneyJourneyId);
            ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address", tempLink.UserProfileId);
            return View(tempLink);
        }

        // GET: TempLinks/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempLink tempLink = db.TempLink.Find(id);
            if (tempLink == null)
            {
                return HttpNotFound();
            }
            ViewBag.JourneyJourneyId = new SelectList(db.Journey, "JourneyId", "EndTime", tempLink.JourneyJourneyId);
            ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address", tempLink.UserProfileId);
            return View(tempLink);
        }

        // POST: TempLinks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TempLinkId,JourneyJourneyId,UserProfileId")] TempLink tempLink)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tempLink).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JourneyJourneyId = new SelectList(db.Journey, "JourneyId", "EndTime", tempLink.JourneyJourneyId);
            ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address", tempLink.UserProfileId);
            return View(tempLink);
        }

        // GET: TempLinks/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempLink tempLink = db.TempLink.Find(id);
            if (tempLink == null)
            {
                return HttpNotFound();
            }
            return View(tempLink);
        }

        // POST: TempLinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TempLink tempLink = db.TempLink.Find(id);
            db.TempLink.Remove(tempLink);
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
