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
    [RoutePrefix("api/Journey")]
    public class JourneysController : ApiController
    {
        private Model1Container db = new Model1Container();
        private ApplicationDbContext identitydb = new ApplicationDbContext();

        // POST: Journeys/Create
        // Create a new journey.
        [Authorize]
        [Route("create")]
        public IHttpActionResult Create(Journey journey)
        {

            Trace.WriteLine("Receive create journey: " + JsonConvert.SerializeObject(journey));
            Reply reply = new Reply();
            String json = "";
            if (ModelState.IsValid)
            {
                journey.Status = "Started";
                var userID = User.Identity.GetUserId();

                journey.UserProfileId = userID;
                journey.StartTime = DateTime.Now;
                journey.EndTime = null;
                Trace.WriteLine("Write to database: " + JsonConvert.SerializeObject(journey));
                journey = db.Journey.Add(journey);
                Trace.WriteLine("Add finish, journey ID: " + journey.JourneyId);
                //Create Templink;
                string checkcode = this.generateTempLink();

                //if the checkcode already exist in database, re-generate one if exist
                while (db.TempLink.Find(checkcode) != null)
                {
                    checkcode = this.generateTempLink();
                }

                List<TempLink> availiableLinks = db.TempLink.Where(s => s.UserProfileId == userID).ToList();
                foreach (TempLink tempLink in availiableLinks)
                {
                    db.TempLink.Remove(tempLink);
                }

                TempLink theTemp = new TempLink();
                theTemp.TempLinkId = checkcode;
                theTemp.JourneyJourneyId = journey.JourneyId; ;
                theTemp.UserProfileId = journey.UserProfileId;
                db.TempLink.Add(theTemp);

                JTracking newTrack = new JTracking();
                newTrack.JourneyJourneyId = journey.JourneyId;
                newTrack.Time = DateTime.Now;
                newTrack.CoordLat = journey.SCoordLat;
                newTrack.CoordLog = journey.SCoordLog;
                db.JTracking.Add(newTrack);

                UsefulFunction.dbSave(db);

                JourneyCreateReplyData data = new JourneyCreateReplyData();
                data.journeyID = journey.JourneyId;
                data.TempLinkId = theTemp.TempLinkId;
                reply.result = "success";
                reply.data = JsonConvert.SerializeObject(data);
                json = JsonConvert.SerializeObject(reply);
                return Ok(json);
            }
            reply.result = "failed";
            reply.errors = "data not match";
            json = JsonConvert.SerializeObject(reply);
            return BadRequest(json);
        }


        // POST Journey Finish Details
        // Finish a journey, and change the status, end longitude and end latitude of the journey.
        [Authorize]
        [Route("journeyFinish")]
        public IHttpActionResult journeyFinish(journeyFinishModel finishModel)
        {
            Trace.WriteLine("Receive create journey: " + JsonConvert.SerializeObject(finishModel));
            Reply reply = new Reply();
            String json = "";
            if (User.Identity.IsAuthenticated)
            {
                var userID = User.Identity.GetUserId();  //get user ID  
                Journey theJourney = db.Journey.Find(finishModel.JourneyId);
                if (theJourney == null)
                {
                    return BadRequest("Journey ID do not exist");
                }
                if (userID == theJourney.UserProfileId)
                {
                    theJourney.EndTime = DateTime.Now;
                    theJourney.ECoordLat = finishModel.ECoordLat;
                    theJourney.ECoordLog = finishModel.ECoordLog;
                    theJourney.Status = "Finished";
                    //delete templink
                    List<TempLink> theTemp = theJourney.TempLink.ToList();
                    Trace.WriteLine("find temp links: "+theTemp.Count());
                        //db.TempLink.Where(s=>s.JourneyJourneyId == theJourney.JourneyId).ToList();
                    foreach (TempLink temp in theTemp)
                    {
                        db.TempLink.Remove(temp);
                    } 

                    db.Entry(theJourney).State = EntityState.Modified;
                    UsefulFunction.dbSave(db);
                    reply.result = "success";
                    json = JsonConvert.SerializeObject(reply);
                    return Ok(json);
                }
            }
            reply.result = "failed";
            reply.errors = "data not match";
            json = JsonConvert.SerializeObject(reply);
            return BadRequest(json);
        }

        //POST: Emergency Retrieve Journey Details
        //Retrieve the journey by the user's emergency contact.
        [AllowAnonymous]
        [Route("EmergencyRetrieve")]
        public IHttpActionResult EmergencyRetrive(EmergencyRetrive model)
        {
            string templinkId = model.TempLinkId;
            Reply reply = new Reply();
            String json = "";
            var theTemp = db.TempLink.Find(templinkId);
            if (theTemp == null)
            {
                reply.result = "failed";
                reply.errors = "Not found";
                json = JsonConvert.SerializeObject(reply);
                return BadRequest(json);
            }
            ReplyEmergencyRetrieve theJourney = new ReplyEmergencyRetrieve();
            theJourney.TempLinkId = templinkId;
            theJourney.StartTime = theTemp.Journey.StartTime;
            theJourney.EndTime = theTemp.Journey.EndTime;
            theJourney.NavigateRoute = theTemp.Journey.NavigateRoute;
            theJourney.SCoordLat = theTemp.Journey.SCoordLat;
            theJourney.SCoordLog = theTemp.Journey.SCoordLog;
            theJourney.ECoordLat = theTemp.Journey.ECoordLat;
            theJourney.ECoordLog = theTemp.Journey.ECoordLog;

            reply.data = JsonConvert.SerializeObject(theJourney);
            reply.result = "success";
            json = JsonConvert.SerializeObject(reply);
            return Ok(json);
        }

        


        //POST: Journey Retrieve Details
        //Retrieve the journey by the user itself.
        [Authorize]
        [Route("retrieveHistory")]
        public IHttpActionResult retrieveHistory()
        {
            Reply reply = new Reply();
            String json = "";
            List<SingleJourney> journeyList = new List<SingleJourney>();
            var userID = User.Identity.GetUserId();
            List<Journey> journeys = db.Journey.Where(s => s.UserProfileId == userID).ToList();
            if (journeys == null)
            {
                return NotFound();
            }

            foreach (Journey theJourney in journeys)
            {
                SingleJourney single = getJourney(theJourney);
                journeyList.Add(single);
            }
            reply.result = "success";
            reply.data = JsonConvert.SerializeObject(journeyList);
            json = JsonConvert.SerializeObject(reply);
            return Ok(json);
        }

        //Find a single journey details.
        public SingleJourney getJourney(Journey theJourney)
        {
            SingleJourney result = new SingleJourney();
            JourneyModel thisjourney = new JourneyModel();
            thisjourney.ECoordLat = theJourney.ECoordLat;
            thisjourney.ECoordLog = theJourney.ECoordLog;
            thisjourney.EndTime = theJourney.EndTime;
            thisjourney.Journeyid = theJourney.JourneyId;
            thisjourney.NavigateRoute = theJourney.NavigateRoute;
            thisjourney.SCoordLat = theJourney.SCoordLat;
            thisjourney.SCoordLog = theJourney.SCoordLog;
            thisjourney.StartTime = theJourney.StartTime;
            thisjourney.Status = theJourney.Status;
            thisjourney.UserProfileId = theJourney.UserProfileId;
            result.journeyDetails = thisjourney;
            
            List<JTracking> theTrack = db.JTracking.Where(s => s.JourneyJourneyId == theJourney.JourneyId).ToList();
            foreach (JTracking t in theTrack)
            {
                JTrackModel thisTrack = new JTrackModel();
                thisTrack.CoordLat = t.CoordLat;
                thisTrack.CoordLog = t.CoordLog;
                thisTrack.JourneyJourneyId = t.JourneyJourneyId;
                thisTrack.Time = t.Time;
                result.trackDetails.Add(thisTrack);
            }
            //result.trackDetails = db.JTracking.Where(s => s.JourneyJourneyId == theJourney.JourneyId).ToList();
            return result;
        }

        //Generate a temp link for a new journey.
        private string generateTempLink()
        {
            //Create Templink;
            int number = 0;
            char code;
            string checkcode = string.Empty;
            System.Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                number = random.Next();
                if (number % 2 == 0)
                {
                    code = (char)('0' + (char)(number % 10));
                }
                else
                {
                    code = (char)('A' + (char)(number % 26));
                }
                checkcode += code.ToString();
            }
            return checkcode;
        }

        // POST: Journeys/Delete/5
        // Delete Journey and JTracking records.
        [Authorize]
        [Route("delete")]
        public IHttpActionResult DeleteConfirmed(JourneyDelete theJourney)
        {
            Reply reply = new Reply();
            String json = "";
            var userID = User.Identity.GetUserId();

            Journey journey = db.Journey.Find(theJourney.Journeyid);
            if (journey == null)
            {
                reply.result = "failed";
                reply.errors = "NotFound";
                json = JsonConvert.SerializeObject(reply);
                return BadRequest(json);
            }

            JTracking jTracking = db.JTracking.Find(theJourney.Journeyid);
            db.JTracking.Remove(jTracking);
            db.Journey.Remove(journey);

            reply.result = "success";
            json = JsonConvert.SerializeObject(reply);
            return Ok(json);
        }
    }

    public static class UsefulFunction
    {
        private static ApplicationDbContext identitydb = new ApplicationDbContext();

        public static void chekProfileId(Model1Container db,string userID, Boolean save = false)
        {
            
            Trace.WriteLine("Detecting does userID " + userID + " exist");
            if (db.UserProfile.Find(userID) == null)
            {
                Trace.WriteLine("Can't find " + userID);
                UserProfile newProfile = new UserProfile();
                newProfile.Id = userID;
                newProfile = db.UserProfile.Add(newProfile);

                
                var identity = identitydb.Users.Find(userID);

                if (identity.UserName != null && db.EmergencyContact.Find(identity.UserName) == null)
                {
                    Trace.WriteLine("[INFO]" + identity.UserName);
                    EmergencyContact contact = new EmergencyContact();
                    contact.Phone = identity.UserName;
                    contact.UserProfile = newProfile;
                    Trace.WriteLine("[INFO]" + contact.Phone);
                    contact = db.EmergencyContact.Add(contact);
                    Trace.WriteLine("[INFO]" + contact.Phone);
                }

                if (save)
                {
                    try
                    {
                        db.SaveChanges();
                    }
                    catch(Exception e)
                    {
                        Trace.WriteLine(e);
                        throw;
                    }      
                }
            }
            else
            {
                Trace.WriteLine("Find " + userID);
            }
        }
        public static void dbSave(Model1Container db)
        {
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                foreach (var i in e.EntityValidationErrors)
                {
                    Trace.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    Trace.WriteLine("entity of type \"{0}\" in state \"{1}\" has the following validation errors:");
                    Trace.WriteLine(i.Entry.Entity.GetType().Name);
                    Trace.WriteLine(i.Entry.State);
                    foreach (var ve in i.ValidationErrors)
                    {
                        Trace.WriteLine("- property: \"{0}\", error: \"{1}\"");
                        Trace.WriteLine(ve.PropertyName);
                        Trace.WriteLine(ve.PropertyName);
                    };
                }

                throw;
            }
        }
    }
}
