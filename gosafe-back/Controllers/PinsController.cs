using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using gosafe_back.Models;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using System.Web.Http;
using System.Diagnostics;
using System.Data.Entity.Validation;


namespace gosafe_back.Controllers
{
    [RoutePrefix("api/Pin")]
    public class PinsController : ApiController
    {
        private Model1Container db = new Model1Container();

        //// GET: Pins
        //public ActionResult Index()
        //{
        //    var pin = db.Pin.Include(p => p.UserProfile).Include(p => p.Suburb);
        //    return View(pin.ToList());
        //}

        //// GET: Pins/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Pin pin = db.Pin.Find(id);
        //    if (pin == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(pin);
        //}

        //// GET: Pins/Create
        //public ActionResult Create()
        //{
        //    ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address");
        //    ViewBag.SuburbSuburbName = new SelectList(db.Suburb, "SuburbName", "Boundary1");
        //    return View();
        //}

        // POST: Pins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [Route("Create")]
        public IHttpActionResult Create(Pin pin)
        {
            Trace.WriteLine("Receive create Pin: " + JsonConvert.SerializeObject(pin));
            Reply reply = new Reply();
            String json = "";
            var userID = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                pin.Time = DateTime.Now;
                pin.UserProfileId = userID;
                db.Pin.Add(pin);
                db.SaveChanges();
                reply.result = "success";
                json = JsonConvert.SerializeObject(reply);
                return Ok(json);
            }
            reply.result = "failed";
            reply.errors = "data not match";
            json = JsonConvert.SerializeObject(reply);
            return BadRequest(json);
            //ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address", pin.UserProfileId);
            //ViewBag.SuburbSuburbName = new SelectList(db.Suburb, "SuburbName", "Boundary1", pin.SuburbSuburbName);
        }

        //// GET: Pins/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Pin pin = db.Pin.Find(id);
        //    if (pin == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address", pin.UserProfileId);
        //    ViewBag.SuburbSuburbName = new SelectList(db.Suburb, "SuburbName", "Boundary1", pin.SuburbSuburbName);
        //    return View(pin);
        //}

        // POST: Pins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [Route("Edit")]
        public IHttpActionResult Edit(Pin pin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pin).State = EntityState.Modified;
                db.SaveChanges();
                return Ok("Index");
            }
            //ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address", pin.UserProfileId);
            //ViewBag.SuburbSuburbName = new SelectList(db.Suburb, "SuburbName", "Boundary1", pin.SuburbSuburbName);
            return Ok(pin);
        }

        //// GET: Pins/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Pin pin = db.Pin.Find(id);
        //    if (pin == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(pin);
        //}

        // POST: Pins/Delete/5
        [Authorize]
        [Route("Delete")]
        public IHttpActionResult DeleteConfirmed(int id)
        {
            Pin pin = db.Pin.Find(id);
            db.Pin.Remove(pin);
            db.SaveChanges();
            return Ok("Index");
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
