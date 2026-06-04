using Microsoft.EntityFrameworkCore;
using PatientService.Data;
using PatientService.Entities;

namespace PatientService.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientDbContext _context;
        public PatientRepository(PatientDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _context.Patients.ToListAsync();
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await _context.Patients.FindAsync(id);
        }

        public async Task AddAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
        }

        public void Update(Patient patient)
        {
            _context.Patients.Update(patient);
        }
        public void Delete(Patient patient)
        {
            _context.Patients.Remove(patient);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}