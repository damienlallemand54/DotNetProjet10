using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PatientService.Entities;

namespace PatientService.Data
{
    public class PatientDbContext : IdentityDbContext<IdentityUser>
    {
        public PatientDbContext(DbContextOptions<PatientDbContext> options) : base(options)
        {
        }
        public DbSet<Patient> Patients { get; set; } = null!;
    }
}