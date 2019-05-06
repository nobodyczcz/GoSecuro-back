using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;



namespace gosafe_back.Models
{
    public class SuburbList
    {
        public List<String> suburbs { get; set; }
    }
    public class SuburbCrime
    {
        public String suburbname { get; set; }
        public float? crimeRate { get; set; }
        public String boundary { get; set; }
    }

  
    public class RegisterPhoneModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }
    }

    public class JourneyModel
    {
        public int Journeyid { get; set; }
        public DateTime StartTime { get; set; }
        public Nullable<DateTime> EndTime { get; set; }
        public string NavigateRoute { get; set; }
        public double SCoordLat { get; set; }
        public double SCoordLog { get; set; }
        public Nullable<double> ECoordLat { get; set; }
        public Nullable<double> ECoordLog { get; set; }
        public string Status { get; set; }
        public string UserProfileId { get; set; }
    }
    public class JTrackModel
    {
        public int JourneyJourneyId { get; set; }
        public System.DateTime Time { get; set; }
        public double CoordLat { get; set; }
        public double CoordLog { get; set; }

    }
    public class SingleJourney {
        public JourneyModel journeyDetails { get; set; }
        public List<JTrackModel> trackDetails { get; set; }
    }

    public class userEmergency
    {
        public string ECname { get; set; }
        public string EmergencyContactPhone { get; set; }
        public string UserProfileId { get; set; }

    }
    public class EmergencyDelete
    {
        public string EmergencyContactPhone { get; set; }

    }
    public class userProfileModel
    {
        public string id { get; set; }
        public string address { get; set; }
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    public class Users
    {
        [Phone]
        public string phone { get; set; }
        public List<userProfileModel> userDetails { get; set; }
    }

    public class test
    {
        public string action { get; set; }
        public RegisterPhoneModel Data { get; set; }
    }

    public class Reply
    {
        public string result { get; set; }
        public string data { get; set; }
        public string errors { get; set; }
    }
    public class journeyFinishModel
    {
        [Required]
        public int JourneyId { get; set; }
        [Required]
        public double ECoordLat { get; set; }
        [Required]
        public double ECoordLog { get; set; }
    }
    public class ContactModel
    {
        [Phone]
        public string EmergencyContactPhone { get; set; }
        public string ECname { get; set; }

    }

    public class ContactEditModel
    {
        public ContactModel pre { get; set; }
        public ContactModel now { get; set; }

    }

    public class JourneyCreateReplyData
    {
        public int journeyID { get; set; }
        public string TempLinkId { get; set; }

    }

    public class ReplyTempLinks
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string TempLinkId { get; set; }
    }

    public class ReplyAllEmergencies
    {
        public string ECname { get; set; }
        public string EmergencyContactPhone { get; set; }
    }

    public class ReplyEmergencyRetrieve
    {
        public string TempLinkId { get; set; }
        public System.DateTime StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public string NavigateRoute { get; set; }
        public double SCoordLat { get; set; }
        public double SCoordLog { get; set; }
        public Nullable<double> ECoordLat { get; set; }
        public Nullable<double> ECoordLog { get; set; }
    }
    public class EmergencyRetrive
    {
        public string TempLinkId { get; set; }
    }
    public class ReplyRetreiveProfile {
        public string Address { get; set; }
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    public class LinkAndTime
    {
        [Required]
        public string TempLinkId { get; set; }
        [Required]
        public DateTime lastLocTime { get; set; }
    }

    public class ReplyUpdateLocations
    {
        public string TempLinkId { get; set; }
        public string locationListJson { get; set; }
    }
    public class SinglePin
    {
        public PinRetrieve pinDetails { get; set; }
    }

    public class PinRetrieve
    {
        public int PinId { get; set; }
        public System.DateTime Time { get; set; }
        public double CoordLat { get; set; }
        public double CoordLog { get; set; }
        public string StreetLight { get; set; }
        public string CCTV { get; set; }
        public string ExperienceType { get; set; }
        public string Experience { get; set; }
        public string OtherDetails { get; set; }
        public string UserProfileId { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
        public string SuburbSuburbName { get; set; }

    }
}