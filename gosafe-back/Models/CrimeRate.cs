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
    
    public partial class CrimeRate
    {
        public string SuburbSuburbName { get; set; }
        public string Rate { get; set; }
        public string OffenceCount { get; set; }
        public string Totpopulation { get; set; }
    
        public virtual Suburb Suburb { get; set; }
    }
}
