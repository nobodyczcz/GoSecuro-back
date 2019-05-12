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
    [RoutePrefix("api/Pin")]
    public class PinsController : ApiController
    {
        private Model1Container db = new Model1Container();

        // POST: Pins/Create
        // Create a new pin.
        [Authorize]
        [Route("Create")]
        public IHttpActionResult Create(Pin pin)
        {
            Trace.WriteLine("Receive create Pin: " + JsonConvert.SerializeObject(pin));
            Reply reply = new Reply();
            String json = "";
            var userID = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                pin.Time = DateTime.Now;
                pin.UserProfileId = userID;
                db.Pin.Add(pin);
                db.SaveChanges();
                reply.result = "success";
                json = JsonConvert.SerializeObject(reply);
                return Ok(json);
            }
            reply.result = "failed";
            reply.errors = "data not match";
            json = JsonConvert.SerializeObject(reply);
            return BadRequest(json);
            //ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address", pin.UserProfileId);
            //ViewBag.SuburbSuburbName = new SelectList(db.Suburb, "SuburbName", "Boundary1", pin.SuburbSuburbName);
        }

        //POST:Pins/Retrieve/5
        //Retrieve the pin.
        [Authorize]
        [Route("Retrieve")]
        public IHttpActionResult Retrieve()
        {
            Reply reply = new Reply();
            String json = "";
            List<SinglePin> PinList = new List<SinglePin>();
            var userID = User.Identity.GetUserId();
            List<Pin> pins = db.Pin.Where(s => s.UserProfileId == userID).ToList();

            if (pins == null)
            {
                return NotFound();
            }

            foreach (Pin thePin in pins)
            {
                SinglePin single = getPin(thePin);
                PinList.Add(single);
            }
            reply.result = "success";
            reply.data = JsonConvert.SerializeObject(PinList);
            json = JsonConvert.SerializeObject(reply);
            return Ok(json);
        }

        //POST:Pins/Retrieve/5
        //Retrieve the pin.
        [Authorize]
        [Route("AllPins")]
        public IHttpActionResult AllPins(pinTime time)
        {
            Reply reply = new Reply();
            String json = "";
            List<SinglePin> PinList = new List<SinglePin>();
            List<Pin> pins;
            if (time.Time == null)
            {
                pins = db.Pin.OrderBy(s => s.Time).ToList();
            }
            else
            {
                pins = db.Pin.Where(s => s.Time > time.Time).OrderBy(s => s.Time).ToList();

            }

            if (pins == null)
            {
                return NotFound();
            }

            foreach (Pin thePin in pins)
            {
                SinglePin single = getPin(thePin);
                PinList.Add(single);
            }
            reply.result = "success";
            reply.data = JsonConvert.SerializeObject(PinList);
            return Ok(reply);
        }

        //Find a single pin.
        public SinglePin getPin(Pin thePin)
        {
            SinglePin result = new SinglePin();
            PinRetrieve thisPin = new PinRetrieve();
            thisPin.PinId = thePin.PinId;
            thisPin.Time = thePin.Time;
            thisPin.CoordLat = thePin.CoordLat;
            thisPin.CoordLog = thePin.CoordLog;
            //Find descriptions of StreetLight, CCTV and ExperienceType
            var theStreetLight = db.StreetLight.Find(thePin.StreetLightId);
            thisPin.StreetLight = theStreetLight.Description;
            var theCCTV = db.CCTV.Find(thePin.CCTVId);
            thisPin.CCTV = theCCTV.Description;
            var theType = db.CCTV.Find(thePin.ExperienceTypeId);
            thisPin.ExperienceType = theType.Description;
            thisPin.Experience = thePin.Experience;
            thisPin.OtherDetails = thePin.OtherDetails;
            thisPin.UserProfileId = thePin.UserProfileId;
            thisPin.State = thePin.State;
            thisPin.Street = thePin.Street;
            thisPin.SuburbSuburbName = thePin.SuburbSuburbName;
            result.pinDetails = thisPin;
            return result;
        }

        // POST: Pins/Edit/5
        // Edit the pin.
        [Authorize]
        [Route("Edit")]
        public IHttpActionResult Edit(Pin thePin)
        {
            Reply reply = new Reply();
            String json = "";

            if (ModelState.IsValid)
            {
                thePin.Time = DateTime.Now;
                db.Entry(thePin).State = EntityState.Modified;
                db.SaveChanges();
                reply.result = "success";
                json = JsonConvert.SerializeObject(reply);
                return Ok(json);
            }
            reply.result = "failed";
            reply.errors = "Not Found";
            return BadRequest(json);
            //ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address", pin.UserProfileId);
            //ViewBag.SuburbSuburbName = new SelectList(db.Suburb, "SuburbName", "Boundary1", pin.SuburbSuburbName);
        }

        // POST: Pins/Delete/5
        // Delete a Pin.
        [Authorize]
        [Route("delete")]
        public IHttpActionResult DeleteConfirmed(PinDelete thePin)
        {
            Reply reply = new Reply();
            String json = "";
            var userID = User.Identity.GetUserId();

            Pin pin = db.Pin.Find(thePin.PinId);
            if (pin == null)
            {
                reply.result = "failed";
                reply.errors = "NotFound";
                json = JsonConvert.SerializeObject(reply);
                return BadRequest(json);
            }

            db.Pin.Remove(pin);
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
