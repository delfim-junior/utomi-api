using System;
using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext()
        {
        }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<MedicalConsultation> MedicalConsultations { get; set; }
        public DbSet<MedicalConsultationStatus> MedicalConsultationStatus { get; set; }
        public DbSet<DoctorDocument> DoctorDocuments { get; set; }
        public DbSet<DoctorRegistrationStatus> DoctorRegistrationStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

    }
}