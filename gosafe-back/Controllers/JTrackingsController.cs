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

        // POST: JTrackings/Create
        // Create a Journey tracking record.
        [Authorize]
        [Route("create")]
        public IHttpActionResult Create(JTracking jTracking)
        {
            Trace.WriteLine("Receive create JTracking: " + JsonConvert.SerializeObject(jTracking));

            Reply reply = new Reply();
            String json = "";
            var UserID = User.Identity.GetUserId(); 
            
            if (ModelState.IsValid)
            {
                JTracking newTrking = new JTracking();
                Journey theJourney = db.Journey.Find(jTracking.JourneyJourneyId);
                if (theJourney == null)
                {
                    return BadRequest("journeyId not exist");
                }
                if (UserID != theJourney.UserProfileId)
                {
                    return BadRequest("You are not authorized to chance this journey ID");
                }
                newTrking.JourneyJourneyId = jTracking.JourneyJourneyId;
                newTrking.Time = DateTime.Now;
                newTrking.CoordLat = jTracking.CoordLat;
                newTrking.CoordLog = jTracking.CoordLog;
                
                Trace.WriteLine("Write to JTracking: " + JsonConvert.SerializeObject(newTrking));
                db.JTracking.Add(newTrking);
                UsefulFunction.dbSave(db);
                
                reply.result = "success";
                json = JsonConvert.SerializeObject(reply);
                return Ok(json);
            }

            //ViewBag.JourneyJourneyId = new SelectList(db.Journey, "JourneyId", "EndTime", jTracking.JourneyJourneyId);
            reply.result = "failed";
            json = JsonConvert.SerializeObject(reply);
            return BadRequest(json);
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
