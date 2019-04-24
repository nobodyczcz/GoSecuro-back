using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using gosafe_back.Models;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;

namespace gosafe_back.Controllers
{
    public class JourneysController : Controller
    {
        private Model1Container db = new Model1Container();

        // GET: Journeys
        public ActionResult Index()
        {
            var journey = db.Journey.Include(j => j.UserProfile);
            return View(journey.ToList());
        }

        // GET: Journeys/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Journey journey = db.Journey.Find(id);
            if (journey == null)
            {
                return HttpNotFound();
            }
            return View(journey);
        }

        // GET: Journeys/Create
        public ActionResult Create()
        {
            ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address");
            return View();
        }

        // POST: Journeys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JourneyId,StartTime,EndTime,NavigateRoute,SCoordLat,SCoordLog,ECoordLat,ECoordLog,Status,UserProfileId")] Journey journey)
        {
            journeyReply reply = new journeyReply();
            String json = "";
            if (ModelState.IsValid)
            {
                db.Journey.Add(journey);
                db.SaveChanges();
                reply.result = "success";
                json = JsonConvert.SerializeObject(reply);
                return Json(json);
            }
            reply.result = "failed";
            reply.errors = "data not match";
            json = JsonConvert.SerializeObject(reply);
            return Json(json);
        }

        
        [HttpPost]
        public ActionResult journeyFinish(journeyFinishModel finishModel)
        {
            journeyReply reply = new journeyReply();
            String json = "";
            if (User.Identity.IsAuthenticated)
            {
                var userID = User.Identity.GetUserId();  //get user ID  
                Journey theJourney = db.Journey.Find(finishModel.JourneyId);
                if (userID == theJourney.UserProfileId)
                {
                    theJourney.EndTime = finishModel.EndTime;
                    theJourney.ECoordLat = finishModel.ECoordLat;
                    theJourney.ECoordLog = finishModel.ECoordLog;
                    theJourney.Status = "Finished";

                    db.Entry(theJourney).State = EntityState.Modified;
                    db.SaveChanges();
                    reply.result = "success";
                    json = JsonConvert.SerializeObject(reply);
                    return Json(json);
                }
            }
            reply.result = "failed";
            reply.errors = "data not match";
            json = JsonConvert.SerializeObject(reply);
            return Json(json);
        }

        [HttpPost]
        public ActionResult journeyRetrieve(joRetrieveModel retrieveModel)
        {
            return View(retrieveModel);
        }
        // GET: Journeys/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Journey journey = db.Journey.Find(id);
            if (journey == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address", journey.UserProfileId);
            return View(journey);
        }

        // POST: Journeys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JourneyId,StartTime,EndTime,NavigateRoute,SCoordLat,SCoordLog,ECoordLat,ECoordLog,Status,UserProfileId")] Journey journey)
        {
            if (ModelState.IsValid)
            {
                db.Entry(journey).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address", journey.UserProfileId);
            return View(journey);
        }

        // GET: Journeys/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Journey journey = db.Journey.Find(id);
            if (journey == null)
            {
                return HttpNotFound();
            }
            return View(journey);
        }

        // POST: Journeys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Journey journey = db.Journey.Find(id);
            db.Journey.Remove(journey);
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
