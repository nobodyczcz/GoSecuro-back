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
            Reply reply = new Reply();
            String json = "";
            if (ModelState.IsValid)
            {
                journey.Status = "Started";
                db.Journey.Add(journey);
                
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

                TempLink theTemp = new TempLink();
                theTemp.TempLinkId = checkcode;
                theTemp.JourneyJourneyId = journey.JourneyId;
                theTemp.UserProfileId = journey.UserProfileId;
                db.TempLink.Add(theTemp);

                db.SaveChanges();
                reply.result = "success";
                json = JsonConvert.SerializeObject(reply);
                return Ok(json);
            }
            reply.result = "failed";
            reply.errors = "data not match";
            json = JsonConvert.SerializeObject(reply);
            return Ok(json);
        }


        //POST Journey Finish Details
        [Authorize]
        [Route("journeyFinish")]
        public IHttpActionResult journeyFinish(journeyFinishModel finishModel)
        {
            Reply reply = new Reply();
            String json = "";
            if (User.Identity.IsAuthenticated)
            {
                var userID = User.Identity.GetUserId();  //get user ID  
                Journey theJourney = db.Journey.Find(finishModel.JourneyId);
                if (userID == theJourney.UserProfileId)
                {
                    theJourney.EndTime = finishModel.EndTime;
                    theJourney.ECoordLat = finishModel.ECoordLat;
                    theJourney.ECoordLog = finishModel.ECoordLog;
                    theJourney.Status = "Finished";
                    //delete templink
                    TempLink theTemp = db.TempLink.Find(theJourney.JourneyId);
                    db.TempLink.Remove(theTemp);

                    db.Entry(theJourney).State = EntityState.Modified;
                    db.SaveChanges();
                    reply.result = "success";
                    json = JsonConvert.SerializeObject(reply);
                    return Ok(json);
                }
            }
            reply.result = "failed";
            reply.errors = "data not match";
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
}
