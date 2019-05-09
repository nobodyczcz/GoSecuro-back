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
        // POST: UserEmergencies/Create
        //Delete the User's emergency contact.
        [HttpPost]
        [Authorize]
        [Route("delete")]
        public IHttpActionResult DeleteConfirmed(EmergencyDelete theEmergency)
        {
            Reply reply = new Reply();
            String json = "";
            var userID = User.Identity.GetUserId();  //get user ID  

            UserEmergency userEmergency = db.UserEmergency.Find(theEmergency.EmergencyContactPhone,userID);
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

        // POST: UserEmergencies/Create
        // Create a User's emergency contact.
        [Authorize]
        [Route("create")]
        public IHttpActionResult Create(ContactModel theContact)
        {
            Reply reply = new Reply();
            String json = "";
            var userID = User.Identity.GetUserId();  //get user ID 

            if (ModelState.IsValid)
            {
                //check does the phone number already in emergencyContact table. create one if not exist.
                if (db.EmergencyContact.Find(theContact.EmergencyContactPhone) == null)
                {
                    createNewContact(theContact.EmergencyContactPhone);

                }

                UserEmergency userEmergency = new UserEmergency();
                userEmergency.UserProfileId = userID;
                userEmergency.EmergencyContactPhone = theContact.EmergencyContactPhone;
                userEmergency.ECname = theContact.ECname;

                if (db.UserEmergency.Find(userEmergency.EmergencyContactPhone, userEmergency.UserProfileId) != null)
                {
                    reply.result = "failed";
                    reply.errors = "Existed";
                    json = JsonConvert.SerializeObject(reply);
                    return BadRequest(json);
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

        // POST: UserEmergencies/Edit/5
        // Edit the User's emergency contact.
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

        // POST: UserEmergencies/RetrieveUser
        //Retrieve the users' profile who set this user to be a emergency contact.
        [Authorize]
        [Route("retrieveUser")]
        public IHttpActionResult RetrieveUser()
        {
            Reply reply = new Reply();
            string json = "";
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

        // POST: UserEmergencies/RetrieveEmergency
        //Retrieve the user's all emergency contact.
        [Authorize]
        [Route("retrieveEmergencies")]
        public IHttpActionResult RetrieveEmergencies()
        {
            //return all emergency contacts' name and phone for current user
            Reply reply = new Reply();
            string json = "";
            var userID = User.Identity.GetUserId();  //get user ID  
            UserProfile user = db.UserProfile.Find(userID);
            List<UserEmergency> emergencies = user.UserEmergency.ToList();

            List<ReplyAllEmergencies> result = new List<ReplyAllEmergencies>();
            foreach (UserEmergency eme in emergencies)
            {
                ReplyAllEmergencies temp = new ReplyAllEmergencies();
                temp.EmergencyContactPhone = eme.EmergencyContactPhone;
                temp.ECname = eme.ECname;
                result.Add(temp);
            }
            
            reply.result = "success";
            reply.data = JsonConvert.SerializeObject(result);
            json = JsonConvert.SerializeObject(reply);
            return Ok(json);
        }
        // POST: UserEmergencies/getUser
        //Get a single user profile.
        public Users getUser(UserEmergency theProfile)
        {
            Users result = new Users();
            result.phone = theProfile.EmergencyContactPhone;
            UserProfile thisUser = db.UserProfile.Find(theProfile.UserProfileId);
            userProfileModel theUser = new userProfileModel();
            theUser.id = thisUser.Id;
            theUser.address = thisUser.Address;
            theUser.FirstName = thisUser.FirstName;
            theUser.LastName = thisUser.LastName;
            theUser.Gender = thisUser.Gender;
            result.userDetails.Add(theUser);
            return result;
        }
        // POST: UserEmergencies/CreateNewContact
        //Create a new record of the emergency contact, if the contact is not included in EmergencyContacts table.
        private void createNewContact(string EmergencyContactPhone)
        {
           
            EmergencyContact newContact = new EmergencyContact();
            newContact.Phone = EmergencyContactPhone;

            //Trace.WriteLine(JsonConvert.SerializeObject("~~~~~"));
            //Trace.WriteLine(JsonConvert.SerializeObject(newContact.Phone));

            var userProfileID = identitydb.Users.Where(s => s.PhoneNumber == EmergencyContactPhone).ToList();
            if (userProfileID.LongCount() > 0)
            {
                UserProfile userProfile = db.UserProfile.Find(userProfileID[0].Id);
                newContact.UserProfile = userProfile;
            }
            newContact = db.EmergencyContact.Add(newContact);
            //db.SaveChanges();
            //Trace.WriteLine(JsonConvert.SerializeObject("--------"));
            //Trace.WriteLine(JsonConvert.SerializeObject(db.EmergencyContact.Find(newContact.Phone)));
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
