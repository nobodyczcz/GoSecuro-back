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
        private ApplicationDbContext identitydb = new ApplicationDbContext();

        // POST: UserProfiles/Retrieve
        // Retrieve the user's profile.
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
            ReplyRetreiveProfile profile = new ReplyRetreiveProfile();
            profile.Address = theProfile.Address;
            profile.Gender = theProfile.Gender;
            profile.FirstName = theProfile.FirstName;
            profile.LastName = theProfile.LastName;
            var user = identitydb.Users.Find(userID);
            profile.Phone = user.PhoneNumber;
            profile.Email = user.Email;

            reply.data = JsonConvert.SerializeObject(profile);
            reply.result = "success";
            return Ok(reply);
        }

        // POST: UserProfiles/Edit/5
        // Edit the user's profile.
        [Authorize]
        [Route("EditProfiles")]
        public IHttpActionResult Edit(UserProfile userProfile)
        {
            Reply reply = new Reply();
            String json = "";
            
            if (ModelState.IsValid)
            {
                userProfile.Id = User.Identity.GetUserId();
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

        // POST: UserProfiles/Delete/5
        // Delete the user's profile.
        [Authorize]
        [Route("delete")]
        public IHttpActionResult DeleteConfirmed()
        {
            Reply reply = new Reply();
            String json = "";
            var userID = User.Identity.GetUserId();

            UserProfile userProfile = db.UserProfile.Find(userID);

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
