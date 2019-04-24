using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;



namespace gosafe_back.Models
{
    public class test
    {
        public string action { get; set; }
        public RegisterPhoneModel Data { get; set; }
    }

    public class journeyReply
    {
        public string result { get; set; }
        //public string Data { get; set; }
        public string errors { get; set; }
    }
    public class journeyFinishModel
    {
        [Required]
        public int JourneyId { get; set; }
        [Required]
        public System.DateTime EndTime { get; set; }
        [Required]
        public decimal ECoordLat { get; set; }
        [Required]
        public decimal ECoordLog { get; set; }
    }

    public class joRetrieveModel
    {
    }
}