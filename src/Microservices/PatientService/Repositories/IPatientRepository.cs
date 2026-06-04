using PatientService.Entities;

namespace PatientService.Repositories
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllAsync();
        Task<Patient?> GetByIdAsync(int id);
        Task AddAsync(Patient patient);
        void Update(Patient patient);
        void Delete(Patient patient);
        Task<bool> SaveChangesAsync();
    }
}