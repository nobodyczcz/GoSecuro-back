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
using System.Diagnostics;


namespace gosafe_back.Controllers
{
    [RoutePrefix("api/TempLinks")]
    public class TempLinksController : ApiController
    {
        private Model1Container db = new Model1Container();
        private ApplicationDbContext identitydb = new ApplicationDbContext();

        // POST: TempLinks/Create
        // Create a templink for a new journey.
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

        //Find avaliable links.
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
            if (profiles.Count == 0)
            {
                reply.result = "No user set you as emergency contact";
                json = JsonConvert.SerializeObject(reply);
                return Ok(json);
            }
            Trace.WriteLine("[INFO] find profiles: "+profiles.Count);
            foreach (UserEmergency theProfile in profiles)
            {
                links = links.Concat(theProfile.UserProfile.TempLink.ToList()).ToList();
            }
            Trace.WriteLine("[INFO] find links: " + links.Count);
            List<ReplyTempLinks> result = new List<ReplyTempLinks>();
            
            foreach (TempLink link in links)
            {
                ReplyTempLinks temp = new ReplyTempLinks();
                temp.TempLinkId = link.TempLinkId;
                temp.firstName = link.UserProfile.FirstName;
                temp.lastName = link.UserProfile.LastName;
                result.Add(temp);
            }

            reply.result = "success";
            reply.data = JsonConvert.SerializeObject(result);
            json = JsonConvert.SerializeObject(reply);
            return Ok(json);
        }

        //Update the longitude and latitude of current location.
        [Authorize]
        [Route("updateLocations")]
        public IHttpActionResult UpdateLocations(List<LinkAndTime> data)
        {
            if (ModelState.IsValid)
            {
                List<ReplyUpdateLocations> reply = new List<ReplyUpdateLocations>(); 
                foreach (LinkAndTime linkTime in data)
                {
                    var locations = db.TempLink.Find(linkTime.TempLinkId).Journey.JTracking.Where(s => s.Time > linkTime.lastLocTime).Select(s=> new {s.Time,s.CoordLat,s.CoordLog }).OrderBy(s=>s.Time).ToList();
                    ReplyUpdateLocations temp = new ReplyUpdateLocations();
                    temp.TempLinkId = linkTime.TempLinkId;
                    temp.locationListJson = JsonConvert.SerializeObject(locations);
                    reply.Add(temp);

                }

                return Ok(reply);
            }
            //ViewBag.JourneyJourneyId = new SelectList(db.Journey, "JourneyId", "EndTime", tempLink.JourneyJourneyId);
            //ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address", tempLink.UserProfileId);
            return BadRequest("Check your data");
        }

        //Retrieve a single emergency contact's profile by the user.
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

        // POST: TempLinks/Delete/5
        // Delete the temp link after journey finished.
        [Authorize]
        [Route("delete")]
        public IHttpActionResult DeleteConfirmed(TempLinkDelete theTempLink)
        {
            Reply reply = new Reply();
            String json = "";

            TempLink tempLink = db.TempLink.Find(theTempLink.TempLinkId);
            if (tempLink == null)
            {
                reply.result = "failed";
                reply.errors = "NotFound";
                json = JsonConvert.SerializeObject(reply);
                return BadRequest(json);
            }
            
            db.TempLink.Remove(tempLink);
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
