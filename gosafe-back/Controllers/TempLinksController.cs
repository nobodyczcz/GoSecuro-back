using gosafe_back.Models;
using System.Web.Http;

namespace gosafe_back.Controllers
{
    [RoutePrefix("api/TempLinks")]
    public class TempLinksController : ApiController
    {
        private Model1Container db = new Model1Container();

        //// GET: TempLinks
        //public ActionResult Index()
        //{
        //    var tempLink = db.TempLink.Include(t => t.Journey).Include(t => t.UserProfile);
        //    return View(tempLink.ToList());
        //}

        //// GET: TempLinks/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TempLink tempLink = db.TempLink.Find(id);
        //    if (tempLink == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tempLink);
        //}

        //// GET: TempLinks/Create
        //public ActionResult Create()
        //{
        //    ViewBag.JourneyJourneyId = new SelectList(db.Journey, "JourneyId", "EndTime");
        //    ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address");
        //    return View();
        //}

        // POST: TempLinks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [Route("create")]
        public IHttpActionResult Create(TempLink tempLink)
        {
            if (ModelState.IsValid)
            {
                db.TempLink.Add(tempLink);
                db.SaveChanges();
                return Ok("Index");
            }
            //ViewBag.JourneyJourneyId = new SelectList(db.Journey, "JourneyId", "EndTime", tempLink.JourneyJourneyId);
            //ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address", tempLink.UserProfileId);
            return Ok(tempLink);
        }

        //// GET: TempLinks/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TempLink tempLink = db.TempLink.Find(id);
        //    if (tempLink == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.JourneyJourneyId = new SelectList(db.Journey, "JourneyId", "EndTime", tempLink.JourneyJourneyId);
        //    ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address", tempLink.UserProfileId);
        //    return View(tempLink);
        //}

        //// POST: TempLinks/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize]
        //[Route("Edit")]
        //public ActionResult Edit([Bind(Include = "TempLinkId,JourneyJourneyId,UserProfileId")] TempLink tempLink)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(tempLink).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.JourneyJourneyId = new SelectList(db.Journey, "JourneyId", "EndTime", tempLink.JourneyJourneyId);
        //    ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address", tempLink.UserProfileId);
        //    return View(tempLink);
        //}

        //// GET: TempLinks/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TempLink tempLink = db.TempLink.Find(id);
        //    if (tempLink == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tempLink);
        //}

        // POST: TempLinks/Delete/5
        [Authorize]
        [Route("delete")]
        public IHttpActionResult DeleteConfirmed(string id)
        {
            TempLink tempLink = db.TempLink.Find(id);
            db.TempLink.Remove(tempLink);
            db.SaveChanges();
            return Ok("Index");
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
