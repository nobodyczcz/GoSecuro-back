using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using gosafe_back.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System.Web.Http;
using System.Diagnostics;

namespace gosafe_back.Controllers
{
    [RoutePrefix("api/JTracking")]
    public class JTrackingsController : ApiController
    {
        private Model1Container db = new Model1Container();

        //// GET: JTrackings
        //public ActionResult Index()
        //{
        //    var jTracking = db.JTracking.Include(j => j.Journey);
        //    return View(jTracking.ToList());
        //}

        //// GET: JTrackings/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    JTracking jTracking = db.JTracking.Find(id);
        //    if (jTracking == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(jTracking);
        //}

        //// GET: JTrackings/Create
        //public ActionResult Create()
        //{
        //    ViewBag.JourneyJourneyId = new SelectList(db.Journey, "JourneyId", "EndTime");
        //    return View();
        //}

        // POST: JTrackings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [Route("create")]
        public IHttpActionResult Create(JTracking jTracking)
        {
            Reply reply = new Reply();
            String json = "";
            if (ModelState.IsValid)
            {
                db.JTracking.Add(jTracking);
                db.SaveChanges();
                reply.result = "success";
                json = JsonConvert.SerializeObject(reply);
                return Ok(json);
            }

            //ViewBag.JourneyJourneyId = new SelectList(db.Journey, "JourneyId", "EndTime", jTracking.JourneyJourneyId);
            reply.result = "failed";
            json = JsonConvert.SerializeObject(reply);
            return BadRequest(json);
        }

        //// GET: JTrackings/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    JTracking jTracking = db.JTracking.Find(id);
        //    if (jTracking == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.JourneyJourneyId = new SelectList(db.Journey, "JourneyId", "EndTime", jTracking.JourneyJourneyId);
        //    return View(jTracking);
        //}

        //// POST: JTrackings/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "JourneyJourneyId,Time,CoordLat,CoordLog")] JTracking jTracking)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(jTracking).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.JourneyJourneyId = new SelectList(db.Journey, "JourneyId", "EndTime", jTracking.JourneyJourneyId);
        //    return View(jTracking);
        //}

        //// GET: JTrackings/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    JTracking jTracking = db.JTracking.Find(id);
        //    if (jTracking == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(jTracking);
        //}

        //// POST: JTrackings/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    JTracking jTracking = db.JTracking.Find(id);
        //    db.JTracking.Remove(jTracking);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
