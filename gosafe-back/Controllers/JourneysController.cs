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

        //// GET: Journeys
        //public ActionResult Index()
        //{
        //    var journey = db.Journey.Include(j => j.UserProfile);
        //    return View(journey.ToList());
        //}

        //// GET: Journeys/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Journey journey = db.Journey.Find(id);
        //    if (journey == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(journey);
        //}

        //// GET: Journeys/Create
        //public ActionResult Create()
        //{
        //    ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address");
        //    return View();
        //}

        // POST: Journeys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        
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
                UsefulFunction.chekProfileId(db,userID);

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


                TempLink theTemp = new TempLink();
                theTemp.TempLinkId = checkcode;
                theTemp.JourneyJourneyId = journey.JourneyId; ;
                theTemp.UserProfileId = journey.UserProfileId;
                db.TempLink.Add(theTemp);

                UsefulFunction.dbSave(db);

                JourneyCreateReplyData data = new JourneyCreateReplyData();
                data.journeyID = journey.JourneyId;
                data.tempLinkID = theTemp.TempLinkId;
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


        //POST Journey Finish Details
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
                UsefulFunction.chekProfileId(db, userID);
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
        [Route("EmergencyRetrieve")]
        public IHttpActionResult EmergencyRetrive(string templinkId)
        {
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
            var theJourney = theTemp.Journey;
            reply.data = JsonConvert.SerializeObject(theJourney);
            reply.result = "success";
            json = JsonConvert.SerializeObject(reply);
            return Ok(json);
        }

        //POST: Journey Retrieve Details
        [Authorize]
        [Route("retrieveHistory")]
        public IHttpActionResult retrieveHistory()
        {
            Reply reply = new Reply();
            String json = "";
            List<SingleJourney> journeyList = new List<SingleJourney>();
            var userID = User.Identity.GetUserId();
            UsefulFunction.chekProfileId(db, userID);
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

        public SingleJourney getJourney(Journey theJourney)
        {
            SingleJourney result = new SingleJourney();
            result.journeyDetails = theJourney;
            result.trackDetails = db.JTracking.Where(s => s.JourneyJourneyId == theJourney.JourneyId).ToList();
            return result ;
        }

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

       

        // GET: Journeys/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Journey journey = db.Journey.Find(id);
        //    if (journey == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address", journey.UserProfileId);
        //    return View(journey);
        //}

        //// POST: Journeys/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "JourneyId,StartTime,EndTime,NavigateRoute,SCoordLat,SCoordLog,ECoordLat,ECoordLog,Status,UserProfileId")] Journey journey)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(journey).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address", journey.UserProfileId);
        //    return View(journey);
        //}

        //// GET: Journeys/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Journey journey = db.Journey.Find(id);
        //    if (journey == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(journey);
        //}

        //// POST: Journeys/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Journey journey = db.Journey.Find(id);
        //    db.Journey.Remove(journey);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }

    public static class UsefulFunction
    {
        public static void chekProfileId(Model1Container db,string userID)
        {
            if (db.UserProfile.Find(userID) == null)
            {
                UserProfile newProfile = new UserProfile();
                newProfile.Id = userID;
                db.UserProfile.Add(newProfile);
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
