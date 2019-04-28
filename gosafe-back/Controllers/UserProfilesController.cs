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
    [RoutePrefix("api/UserProfiles")]
    public class UserProfilesController : ApiController
    {
        private Model1Container db = new Model1Container();

        //// GET: UserProfiles
        //public ActionResult Index()
        //{
        //    var userProfile = db.UserProfile.Include(u => u.EmergencyContact);
        //    return View(userProfile.ToList());
        //}

        //// GET: UserProfiles/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserProfile userProfile = db.UserProfile.Find(id);
        //    if (userProfile == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(userProfile);
        //}

        //// GET: UserProfiles/Create
        //public ActionResult Create()
        //{
        //    ViewBag.Id = new SelectList(db.EmergencyContact, "Phone", "Phone");
        //    return View();
        //}

        // POST: UserProfiles/Retrieve
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [Route("Retrieve")]
        public IHttpActionResult RetreiveProfile()
        {
            Reply reply = new Reply();
            String json = "";
            var userID = User.Identity.GetUserId();
            UserProfile theProfile = db.UserProfile.Find(userID);
            if (theProfile == null)
            {
                reply.result = "Failed";
                reply.errors = "Not Found";
                json = JsonConvert.SerializeObject(reply);
                return BadRequest(json);
            }
            reply.data = JsonConvert.SerializeObject(theProfile);
            reply.result = "success";
            return Ok(reply);
        }

        //// GET: UserProfiles/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserProfile userProfile = db.UserProfile.Find(id);
        //    if (userProfile == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.Id = new SelectList(db.EmergencyContact, "Phone", "Phone", userProfile.Id);
        //    return View(userProfile);
        //}

        // POST: UserProfiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [Route("EditProfiles")]
        public IHttpActionResult Edit(UserProfile userProfile)
        {
            Reply reply = new Reply();
            String json = "";
            if (ModelState.IsValid)
            {
                db.Entry(userProfile).State = EntityState.Modified;
                db.SaveChanges();
                reply.result = "success";
                json = JsonConvert.SerializeObject(reply);
                return Ok(json);
            }
            reply.result = "failed";
            reply.errors = "Not Found";
            return BadRequest(json);
        }

        //// GET: UserProfiles/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserProfile userProfile = db.UserProfile.Find(id);
        //    if (userProfile == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(userProfile);
        //}

        // POST: UserProfiles/Delete/5
        [Authorize]
        [Route("Delete Profile")]
        public IHttpActionResult DeleteConfirmed(string id)
        {
            Reply reply = new Reply();
            String json = "";
            UserProfile userProfile = db.UserProfile.Find(id);
            if (userProfile == null)
            {
                reply.result = "failed";
                reply.errors = "Not Found";
                return BadRequest(json);
            }
            db.UserProfile.Remove(userProfile);
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
