using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using gosafe_back.Models;

namespace gosafe_back.Controllers
{
    public class CrimeRatesController : Controller
    {
        private Model1Container db = new Model1Container();

        // GET: CrimeRates
        public ActionResult Index()
        {
            var crimeRate = db.CrimeRate.Include(c => c.Suburb);
            return View(crimeRate.ToList());
        }

        // GET: CrimeRates/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrimeRate crimeRate = db.CrimeRate.Find(id);
            if (crimeRate == null)
            {
                return HttpNotFound();
            }
            return View(crimeRate);
        }

        // GET: CrimeRates/Create
        public ActionResult Create()
        {
            ViewBag.SuburbSuburbName = new SelectList(db.Suburb, "SuburbName", "Postcode");
            return View();
        }

        // POST: CrimeRates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SuburbSuburbName,Rate,OffenceCount,Totpopulation")] CrimeRate crimeRate)
        {
            if (ModelState.IsValid)
            {
                db.CrimeRate.Add(crimeRate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SuburbSuburbName = new SelectList(db.Suburb, "SuburbName", "Postcode", crimeRate.SuburbSuburbName);
            return View(crimeRate);
        }

        // GET: CrimeRates/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrimeRate crimeRate = db.CrimeRate.Find(id);
            if (crimeRate == null)
            {
                return HttpNotFound();
            }
            ViewBag.SuburbSuburbName = new SelectList(db.Suburb, "SuburbName", "Postcode", crimeRate.SuburbSuburbName);
            return View(crimeRate);
        }

        // POST: CrimeRates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SuburbSuburbName,Rate,OffenceCount,Totpopulation")] CrimeRate crimeRate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crimeRate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SuburbSuburbName = new SelectList(db.Suburb, "SuburbName", "Postcode", crimeRate.SuburbSuburbName);
            return View(crimeRate);
        }

        // GET: CrimeRates/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrimeRate crimeRate = db.CrimeRate.Find(id);
            if (crimeRate == null)
            {
                return HttpNotFound();
            }
            return View(crimeRate);
        }

        // POST: CrimeRates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CrimeRate crimeRate = db.CrimeRate.Find(id);
            db.CrimeRate.Remove(crimeRate);
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
