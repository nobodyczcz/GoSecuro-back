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


namespace gosafe_back.Controllers
{
    [RoutePrefix("api/EmergencyContact")]
    public class EmergencyContactsController : ApiController
    {
        private Model1Container db = new Model1Container();

        //// GET: EmergencyContacts
        //public IHttpActionResult Index()
        //{
        //    return Ok(db.EmergencyContact.ToList());
        //}

        //// GET: EmergencyContacts/Details/5
        //public IHttpActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    EmergencyContact emergencyContact = db.EmergencyContact.Find(id);
        //    if (emergencyContact == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(emergencyContact);
        //}

        //// GET: EmergencyContacts/Create
        //public IHttpActionResult Create()
        //{
        //    return Ok();
        //}

        // POST: EmergencyContacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [Route("create")]
        public IHttpActionResult Create(EmergencyContact emergencyContact)
        {
            Reply reply = new Reply();
            String json = "";
            if (ModelState.IsValid)
            {
                EmergencyContact theContact= db.EmergencyContact.Find(emergencyContact.Phone);
                if (theContact.Phone == emergencyContact.Phone)
                {
                    reply.result = "existed";
                    json = JsonConvert.SerializeObject(reply);
                    return Ok(json);
                }
                db.EmergencyContact.Add(emergencyContact);
                db.SaveChanges();
                reply.result = "success";
                json = JsonConvert.SerializeObject(reply);
                return Ok(json);
            }
            reply.result = "failed";
            json = JsonConvert.SerializeObject(reply);
            return Ok(json);
        }

        //// GET: EmergencyContacts/Edit/5
        //public IHttpActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    EmergencyContact emergencyContact = db.EmergencyContact.Find(id);
        //    if (emergencyContact == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(emergencyContact);
        //}

        //// POST: EmergencyContacts/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Phone")] EmergencyContact emergencyContact)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(emergencyContact).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(emergencyContact);
        //}

        //// GET: EmergencyContacts/Delete/5
        //[Authorize]
        //[Route("delete")]
        //public IHttpActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    EmergencyContact emergencyContact = db.EmergencyContact.Find(id);
        //    if (emergencyContact == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(emergencyContact);
        //}

        // POST: EmergencyContacts/Delete/5
        [Authorize]
        [Route("delete")]
        public IHttpActionResult DeleteConfirmed(EmeContDelete theEmeCont)
        {
            Reply reply = new Reply();
            String json = "";
            var userID = User.Identity.GetUserId();

            EmergencyContact emergencyContact = db.EmergencyContact.Find(theEmeCont.EmergencyContactPhone,userID);
            if (emergencyContact == null)
            {
                reply.result = "failed";
                reply.errors = "NotFound";
                json = JsonConvert.SerializeObject(reply);
                return BadRequest(json);
            }
            db.EmergencyContact.Remove(emergencyContact);
            db.SaveChanges();
            reply.result = "success";
            json = JsonConvert.SerializeObject(reply);
            return Ok(json);
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
