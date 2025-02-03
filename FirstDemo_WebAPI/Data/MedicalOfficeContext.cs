using FirstDemo_WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstDemo_WebAPI.Data
{
    public class MedicalOfficeContext : DbContext
    {
        public MedicalOfficeContext(DbContextOptions<MedicalOfficeContext> options)
            : base(options)
        {

        }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Prevent Cascade Delete from Doctor to Patient
            //so we are prevented from deleting a Doctor with
            //Patients assigned
            modelBuilder.Entity<Doctor>()
                .HasMany(p => p.Patients)
                .WithOne(d => d.Doctor)
                .OnDelete(DeleteBehavior.Restrict);

            //Add a unique index to the OHIP Number
            modelBuilder.Entity<Patient>()
            .HasIndex(p => p.OHIP)
            .IsUnique();

            //To deal with multiple births among our patients
            //add a unique index to the combination
            //of DOB, Last and First Names
            modelBuilder.Entity<Patient>()
            .HasIndex(p => new { p.DOB, p.LastName, p.FirstName })
            .IsUnique();

        }
    }
}
