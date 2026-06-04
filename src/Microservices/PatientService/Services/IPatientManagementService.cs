using PatientService.DTOs;

namespace PatientService.Services
{
    public interface IPatientManagementService
    {
        Task<IEnumerable<PatientReadDTO>> GetAllPatientsAsync();
        Task<PatientReadDTO?> GetPatientByIdAsync(int id);
        Task<PatientReadDTO> CreatePatientAsync(PatientCreateDTO createDto);
        Task<bool> UpdatePatientAsync(int id, PatientUpdateDTO updateDto);
        Task<bool> DeletePatientAsync(int id);
    }
}