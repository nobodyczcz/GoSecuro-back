﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Model1Container : DbContext
    {
        public Model1Container()
            : base("name=Model1Container")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Suburb> Suburb { get; set; }
        public virtual DbSet<CrimeRate> CrimeRate { get; set; }
        public virtual DbSet<TempLink> TempLink { get; set; }
        public virtual DbSet<JTracking> JTracking { get; set; }
        public virtual DbSet<UserProfile> UserProfile { get; set; }
        public virtual DbSet<UserEmergency> UserEmergency { get; set; }
        public virtual DbSet<EmergencyContact> EmergencyContact { get; set; }
        public virtual DbSet<Journey> Journey { get; set; }
        public virtual DbSet<Pin> Pin { get; set; }
        public virtual DbSet<CCTV> CCTV { get; set; }
        public virtual DbSet<StreetLight> StreetLight { get; set; }
        public virtual DbSet<ExperienceType> ExperienceType { get; set; }
    }
}
