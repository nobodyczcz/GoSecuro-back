using gosafe_back.Models;
using System.Web.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Validation;

namespace gosafe_back.Controllers
{
    [RoutePrefix("api/TempLinks")]
    public class TempLinksController : ApiController
    {
        private Model1Container db = new Model1Container();
        private ApplicationDbContext identitydb = new ApplicationDbContext();


        //// GET: TempLinks
        //public ActionResult Index()
        //{
        //    var tempLink = db.TempLink.Include(t => t.Journey).Include(t => t.UserProfile);
        //    return View(tempLink.ToList());
        //}

        //// GET: TempLinks/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TempLink tempLink = db.TempLink.Find(id);
        //    if (tempLink == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tempLink);
        //}

        //// GET: TempLinks/Create
        //public ActionResult Create()
        //{
        //    ViewBag.JourneyJourneyId = new SelectList(db.Journey, "JourneyId", "EndTime");
        //    ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address");
        //    return View();
        //}

        // POST: TempLinks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [Route("create")]
        public IHttpActionResult Create(TempLink tempLink)
        {
            if (ModelState.IsValid)
            {
                db.TempLink.Add(tempLink);
                db.SaveChanges();
                return Ok("Index");
            }
            //ViewBag.JourneyJourneyId = new SelectList(db.Journey, "JourneyId", "EndTime", tempLink.JourneyJourneyId);
            //ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address", tempLink.UserProfileId);
            return Ok(tempLink);
        }

        [Authorize]
        [Route("avaliableLinks")]
        public IHttpActionResult avaliableLinks()
        {
            Reply reply = new Reply();
            string json = "";
            List<TempLink> links = new List<TempLink>();
            var userID = User.Identity.GetUserId();  //get user ID  
            var thisUser = identitydb.Users.Find(userID);
            List<UserEmergency> profiles = db.UserEmergency.Where(s => s.EmergencyContactPhone == thisUser.PhoneNumber).ToList();
            if (profiles == null)
            {
                reply.result = "No user set you as emergency contact";
                json = JsonConvert.SerializeObject(reply);
                return Ok(json);
            }

            foreach (UserEmergency theProfile in profiles)
            {
                links.Concat(theProfile.UserProfile.TempLink.ToList());
            }
            List<ReplyTempLinks> result = new List<ReplyTempLinks>();
            
            foreach (TempLink link in links)
            {
                ReplyTempLinks temp = new ReplyTempLinks();
                temp.tempLink = link.TempLinkId;
                temp.firstName = link.UserProfile.FirstName;
                temp.lastName = link.UserProfile.LastName;
                result.Add(temp);
            }

            reply.result = "success";
            reply.data = JsonConvert.SerializeObject(result);
            json = JsonConvert.SerializeObject(reply);
            return Ok(json);
        }



        private Users getUser(UserEmergency theProfile)
        {
            Users result = new Users();
            result.phone = theProfile.EmergencyContactPhone;
            UserProfile thisUser = db.UserProfile.Find(theProfile.UserProfileId);
            userProfileModel theUser = new userProfileModel();
            theUser.id = thisUser.Id;
            theUser.address = thisUser.Address;
            theUser.FirstName = thisUser.FirstName;
            theUser.LastName = thisUser.LastName;
            theUser.Gender = thisUser.Gender;
            result.userDetails.Add(theUser);
            return result;
        }

        //// GET: TempLinks/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TempLink tempLink = db.TempLink.Find(id);
        //    if (tempLink == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.JourneyJourneyId = new SelectList(db.Journey, "JourneyId", "EndTime", tempLink.JourneyJourneyId);
        //    ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address", tempLink.UserProfileId);
        //    return View(tempLink);
        //}

        //// POST: TempLinks/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize]
        //[Route("Edit")]
        //public ActionResult Edit([Bind(Include = "TempLinkId,JourneyJourneyId,UserProfileId")] TempLink tempLink)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(tempLink).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.JourneyJourneyId = new SelectList(db.Journey, "JourneyId", "EndTime", tempLink.JourneyJourneyId);
        //    ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address", tempLink.UserProfileId);
        //    return View(tempLink);
        //}

        //// GET: TempLinks/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TempLink tempLink = db.TempLink.Find(id);
        //    if (tempLink == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tempLink);
        //}

        // POST: TempLinks/Delete/5
        [Authorize]
        [Route("delete")]
        public IHttpActionResult DeleteConfirmed(string id)
        {
            TempLink tempLink = db.TempLink.Find(id);
            db.TempLink.Remove(tempLink);
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
