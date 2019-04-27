using gosafe_back.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Diagnostics;


namespace gosafe_back.Controllers
{
    [RoutePrefix("api/UserEmergency")]
    public class UserEmergenciesController : ApiController
    {
        private Model1Container db = new Model1Container();
        private ApplicationDbContext identitydb = new ApplicationDbContext();

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
            Trace.WriteLine(JsonConvert.SerializeObject(theContact));
            Reply reply = new Reply();
            String json = "";
            var userID = User.Identity.GetUserId();  //get user ID  
            UserEmergency userEmergency = new UserEmergency();
            userEmergency.UserProfileId = userID;
            userEmergency.EmergencyContactPhone = theContact.EmergencyContactPhone;
            userEmergency.ECname = theContact.ECname;


            if (ModelState.IsValid)
            {
                //check does the phone number already in emergencyContact table. create one if not exist.
                if (db.EmergencyContact.Find(theContact.EmergencyContactPhone) == null)
                {
                    createNewContact(theContact.EmergencyContactPhone);

                }

                db.UserEmergency.Add(userEmergency);
                db.SaveChanges();
                reply.result = "success";
                json = JsonConvert.SerializeObject(reply);
                return Ok(json);
            }
            reply.result = "failed";
            reply.errors = "data not match";
            json = JsonConvert.SerializeObject(reply);
            return BadRequest(json);
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
                var userEmergencies = db.UserProfile.Find(userID).UserEmergency;
                var oldEmergency = userEmergencies.Where(s => s.EmergencyContactPhone == userEmergency.EmergencyContactPhone);

                //Check does the phone exist if not store Contactphone to table EmergencyContacts
                EmergencyContact thisContact = db.EmergencyContact.Find(newContact.EmergencyContactPhone);
                if (thisContact == null)
                {
                    createNewContact(newContact.EmergencyContactPhone);
                }


                //Check whether old userEmergency exist
                if (oldEmergency.Count() == 0)
                {
                    reply.result = "failed";
                    reply.errors = "Contact do not exist";
                    return BadRequest(JsonConvert.SerializeObject(reply));
                }
                oldEmergency.First().EmergencyContactPhone = theContact.now.EmergencyContactPhone;
                oldEmergency.First().ECname = theContact.now.ECname;
                db.Entry(oldEmergency.First()).State = EntityState.Modified;
                db.SaveChanges();
                reply.result = "Edit success";
                json = JsonConvert.SerializeObject(reply);
                
                
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

        [Authorize]
        [Route("delete")]
        public IHttpActionResult Delete(string phone)
        {
            Reply reply = new Reply();
            String json = "";
            var userID = User.Identity.GetUserId();  //get user ID  
            UserEmergency userEmergency = db.UserEmergency.Find(phone, userID);
            if (userEmergency == null)
            {
                reply.result = "failed";
                reply.errors = "Not Found";
                json = JsonConvert.SerializeObject(reply);
                return BadRequest(json);
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
            List<Users> UserList = new List<Users>();
            var userID = User.Identity.GetUserId();  //get user ID  
            var thisUser = identitydb.Users.Find(userID);
            List<UserEmergency> profiles = db.UserEmergency.Where(s => s.EmergencyContactPhone == thisUser.PhoneNumber).ToList();
            if (profiles == null)
            {
                reply.result = "Not found";
                json = JsonConvert.SerializeObject(reply);
                return BadRequest(json);
            }
            foreach (UserEmergency theProfile in profiles)
            {
                Users user = getUser(theProfile);
                UserList.Add(user);
            }
            reply.result = "success";
            reply.data = JsonConvert.SerializeObject(UserList);
            json = JsonConvert.SerializeObject(reply);
            return Ok(json);
        }

        public Users getUser(UserEmergency theProfile)
        {
            Users result = new Users();
            result.phone = theProfile.EmergencyContactPhone;
            result.userDetails = db.UserProfile.Where(s => s.Id == theProfile.UserProfileId).ToList();
            return result;
        }

        private void createNewContact(string EmergencyContactPhone)
        {
            EmergencyContact newContact = new EmergencyContact();
            newContact.Phone = EmergencyContactPhone;
            var userProfileID = identitydb.Users.Where(s => s.PhoneNumber == EmergencyContactPhone).ToList();
            if (userProfileID.LongCount() > 0)
            {
                UserProfile userProfile = db.UserProfile.Find(userProfileID[0].Id);
                newContact.UserProfile = userProfile;
            }
            newContact = db.EmergencyContact.Add(newContact);
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
