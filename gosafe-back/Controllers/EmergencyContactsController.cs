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

        // POST: EmergencyContacts/Create
        // Create a emergency contact.
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

        // POST: EmergencyContacts/Delete/5
        // Delete the emergency contact from table EmergencyContacts.
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
