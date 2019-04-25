﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;



namespace gosafe_back.Models
{
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