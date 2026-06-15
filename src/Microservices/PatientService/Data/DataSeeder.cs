using PatientService.Data;
using PatientService.Entities;

namespace PatientService.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(PatientDbContext context)
        {
            if (context.Patients.Any()) return;

            context.Patients.AddRange(
                new Patient { FirstName = "Test", LastName = "TestNone", BirthDate = new DateOnly(1966, 12, 31), Gender = "F", Address = "1 Brookside St", PhoneNumber = "100-222-3333" },
                new Patient { FirstName = "Test", LastName = "TestBorderline", BirthDate = new DateOnly(1945, 6, 24), Gender = "M", Address = "2 High St", PhoneNumber = "200-333-4444" },
                new Patient { FirstName = "Test", LastName = "TestInDanger", BirthDate = new DateOnly(2004, 6, 18), Gender = "M", Address = "3 Club Road", PhoneNumber = "300-444-5555" },
                new Patient { FirstName = "Test", LastName = "TestEarlyOnset", BirthDate = new DateOnly(2002, 6, 28), Gender = "F", Address = "4 Valley Dr", PhoneNumber = "400-555-6666" }
            );

            await context.SaveChangesAsync();
        }
    }
}