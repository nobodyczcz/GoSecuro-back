//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace gosafe_back.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserEmergency
    {
        public int UserProfileUserID { get; set; }
        public int EmergencyContactContactPhone { get; set; }
    
        public virtual UserProfile UserProfile { get; set; }
        public virtual EmergencyContact EmergencyContact { get; set; }
    }
}
