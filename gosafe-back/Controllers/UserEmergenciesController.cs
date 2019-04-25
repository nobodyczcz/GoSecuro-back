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
    [RoutePrefix("api/UserEmergency")]
    public class UserEmergenciesController : ApiController
    {
        private Model1Container db = new Model1Container();

        //// GET: UserEmergencies
        //public ActionResult Index()
        //{
        //    var userEmergency = db.UserEmergency.Include(u => u.UserProfile).Include(u => u.EmergencyContact);
        //    return View(userEmergency.ToList());
        //}

        //// GET: UserEmergencies/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserEmergency userEmergency = db.UserEmergency.Find(id);
        //    if (userEmergency == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(userEmergency);
        //}

        //// GET: UserEmergencies/Create
        //public ActionResult Create()
        //{
        //    ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address");
        //    ViewBag.EmergencyContactPhone = new SelectList(db.EmergencyContact, "Phone", "Phone");
        //    return View();
        //}

        // POST: UserEmergencies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [Route("create")]
        public IHttpActionResult Create(ContactModel theContact)
        {
            Reply reply = new Reply();
            String json = "";
            var userID = User.Identity.GetUserId();  //get user ID  
            UserEmergency userEmergency = new UserEmergency();
            userEmergency.UserProfileId = userID;
            userEmergency.EmergencyContactPhone = theContact.EmergencyContactPhone;
            userEmergency.ECname = theContact.ECname;
            if (ModelState.IsValid)
            {
                db.UserEmergency.Add(userEmergency);
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

        //// GET: UserEmergencies/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserEmergency userEmergency = db.UserEmergency.Find(id);
        //    if (userEmergency == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.UserProfileId = new SelectList(db.UserProfile, "Id", "Address", userEmergency.UserProfileId);
        //    ViewBag.EmergencyContactPhone = new SelectList(db.EmergencyContact, "Phone", "Phone", userEmergency.EmergencyContactPhone);
        //    return View(userEmergency);
        //}

        // POST: UserEmergencies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [Route("edit")]
        public IHttpActionResult Edit(ContactEditModel theContact)
        {
            Reply reply = new Reply();
            String json = "";
            var userID = User.Identity.GetUserId();  //get user ID  

            UserEmergency userEmergency = new UserEmergency();
            userEmergency.UserProfileId = userID;
            userEmergency.EmergencyContactPhone = theContact.pre.EmergencyContactPhone;
            userEmergency.ECname = theContact.pre.ECname;

            UserEmergency newContact = new UserEmergency();
            newContact.UserProfileId = userID;
            newContact.EmergencyContactPhone = theContact.now.EmergencyContactPhone;
            newContact.ECname = theContact.now.ECname;

            if (ModelState.IsValid)
            {
                //Check whether old userEmergency exist
                UserEmergency delete = db.UserEmergency.Find(userEmergency.UserProfileId,userEmergency.EmergencyContactPhone);
                if (delete == null)
                {
                    reply.result = "Not Found";
                    json = JsonConvert.SerializeObject(reply);
                    return Ok(json);
                }

                //Check whether new userEmergency exist
                UserEmergency addNew = db.UserEmergency.Find(newContact.UserProfileId, newContact.EmergencyContactPhone);
                if (addNew != null)
                {
                    reply.result = "Contact Existed";
                    json = JsonConvert.SerializeObject(reply);
                    return Ok(json);
                }

                db.UserEmergency.Remove(delete);
                db.UserEmergency.Add(newContact);
                db.SaveChanges();
                reply.result = "Edit success";
                json = JsonConvert.SerializeObject(reply);
                
                //store Contactphone to table EmergencyContacts
                EmergencyContact thisContact = db.EmergencyContact.Find(newContact.EmergencyContactPhone);
                if (thisContact != null)
                {
                    reply.result = "contact exist, edit success";
                    json = JsonConvert.SerializeObject(reply);
                    return Ok(json);
                }
                thisContact.Phone = newContact.EmergencyContactPhone;
                db.EmergencyContact.Add(thisContact);
                db.SaveChanges();
                reply.result = "contact create, edit success";
                json = JsonConvert.SerializeObject(reply);
                return Ok(json);
            }
            reply.result = "failed";
            reply.errors = "Data Type not validate";
            json = JsonConvert.SerializeObject(reply);
            return Ok(json);
        }

        //// GET: UserEmergencies/Delete/5
        //public IHttpActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserEmergency userEmergency = db.UserEmergency.Find(id);
        //    if (userEmergency == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(userEmergency);
        //}

        // POST: UserEmergencies/Delete/5
        [Authorize]
        [Route("delete")]
        public IHttpActionResult Delete(int id)
        {
            Reply reply = new Reply();
            String json = "";
            var userID = User.Identity.GetUserId();  //get user ID  
            UserEmergency userEmergency = db.UserEmergency.Find(id,userID);
            if (userEmergency == null)
            {
                reply.result = "failed";
                reply.errors = "Not Found";
                json = JsonConvert.SerializeObject(reply);
                return Ok(json);
            }
            db.UserEmergency.Remove(userEmergency);
            db.SaveChanges();
            reply.result = "success";
            json = JsonConvert.SerializeObject(reply);
            return Ok(json);
        }


        // POST: UserEmergencies/RetrieveUser/5
        [Authorize]
        [Route("retrieveUser")]
        public IHttpActionResult RetrieveUser()
        {
            Reply reply = new Reply();
            String json = "";
            var userID = User.Identity.GetUserId();  //get user ID  
            //UserProfile userEmergency = db.UserEmergency.Find(userID);
            //if (userEmergency == null)
            //{
            //    reply.result = "failed";
            //    reply.errors = "Not Found";
            //    json = JsonConvert.SerializeObject(reply);
            //    return Ok(json);
            //}
            //db.UserEmergency.Remove(userEmergency);
            //db.SaveChanges();
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
