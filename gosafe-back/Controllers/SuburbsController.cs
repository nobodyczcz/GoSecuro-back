using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using gosafe_back.Models;
using Newtonsoft.Json;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace gosafe_back.Controllers
{
    public class SuburbList
    {
        public List<String> suburbs { get; set; }
    }

    public class SuburbCrime
    {
        public String suburbname { get; set; }
        public String crimeRate { get; set; }
        public String boundary { get; set; }
    }

    public class SuburbsController : Controller
    {
        private Model1Container db = new Model1Container();

        // GET: Suburbs
        public ActionResult Index()
        {
            var suburb = db.Suburb.Include(s => s.CrimeRate);
            return View();
        }

        // GET: Suburbs/Details
        [HttpPost]
        public ActionResult Details(List<String> suburbs)
        {
            System.Diagnostics.Debug.WriteLine("debug:");
            System.Diagnostics.Debug.WriteLine(suburbs);
            if (suburbs != null)
            {
                //find suburbs
                List<SuburbCrime> SuburbCrimeList = new List<SuburbCrime>();
                foreach (var i in suburbs)
                {
                    Suburb theSuburb = db.Suburb.Find(i);
                    CrimeRate theRate = db.CrimeRate.Find(i);
                    if (theSuburb != null)
                    {
                        var newSub = new SuburbCrime();
                        newSub.suburbname = i;
                        newSub.boundary = theSuburb.Boundary1 + theSuburb.Boundary2;
                        newSub.crimeRate = theRate.Rate;
                        SuburbCrimeList.Add(newSub);
                    }
                        
                    
                }

                String json = JsonConvert.SerializeObject(SuburbCrimeList);

                //output Json type suburb details to front-end
                return Json(json);
            }
            else
            {
                return Json("failed");
            }
            //Suburb suburb = db.Suburb.Find(id);

        }

        [HttpPost]
        public ActionResult Message(List<String> message)
        {
            string accountSid = "AC90b329101b6566d76485239d50f6ea00";
            string authToken = "be4151a4e580bf43eadb860c61217e2b";
            System.Diagnostics.Debug.WriteLine("debug:");
            System.Diagnostics.Debug.WriteLine(message);
            if (message != null)
            {
                TwilioClient.Init(accountSid, authToken);

                var content = MessageResource.Create(
                    body: message[1],
                    from: new Twilio.Types.PhoneNumber("+61480015535"),
                    to: new Twilio.Types.PhoneNumber(message[0])
                );

                Console.WriteLine(content.Sid);


                return Json(content.Sid);
            }
            else
            {
                return Json("failed");
            }
            //Suburb suburb = db.Suburb.Find(id);

        }

        // GET: Suburbs/Create
        public ActionResult Create()
        {
            ViewBag.SuburbName = new SelectList(db.CrimeRate, "SuburbSuburbName", "Rate");
            return View();
        }

        // POST: Suburbs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SuburbName,Postcode,Boundary1,Boundary2")] Suburb suburb)
        {
            if (ModelState.IsValid)
            {
                db.Suburb.Add(suburb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SuburbName = new SelectList(db.CrimeRate, "SuburbSuburbName", "Rate", suburb.SuburbName);
            return View(suburb);
        }

        // GET: Suburbs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suburb suburb = db.Suburb.Find(id);
            if (suburb == null)
            {
                return HttpNotFound();
            }
            ViewBag.SuburbName = new SelectList(db.CrimeRate, "SuburbSuburbName", "Rate", suburb.SuburbName);
            return View(suburb);
        }

        // POST: Suburbs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SuburbName,Postcode,Boundary1,Boundary2")] Suburb suburb)
        {
            if (ModelState.IsValid)
            {
                db.Entry(suburb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SuburbName = new SelectList(db.CrimeRate, "SuburbSuburbName", "Rate", suburb.SuburbName);
            return View(suburb);
        }

        // GET: Suburbs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suburb suburb = db.Suburb.Find(id);
            if (suburb == null)
            {
                return HttpNotFound();
            }
            return View(suburb);
        }

        // POST: Suburbs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Suburb suburb = db.Suburb.Find(id);
            db.Suburb.Remove(suburb);
            db.SaveChanges();
            return RedirectToAction("Index");
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
