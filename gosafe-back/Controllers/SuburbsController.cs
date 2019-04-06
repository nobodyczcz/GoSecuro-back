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
        public ActionResult Details(SuburbList suburbList)
        {
            if (suburbList != null)
            {
                System.Diagnostics.Debug.WriteLine(suburbList);
                String respond = "";
                for (int i=0;i<suburbList.suburbs.Count();i++)
                {
                    respond += suburbList.suburbs[i];
                }
                return Json(respond);
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
