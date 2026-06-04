using PatientService.DTOs;
using PatientService.Entities;
using PatientService.Repositories;

namespace PatientService.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;

        public PatientService(IPatientRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PatientReadDTO>> GetAllPatientsAsync()
        {
            var patients = await _repository.GetAllAsync();

            // Mapping de l'Entité vers le ReadDTO
            return patients.Select(p => MapToReadDTO(p));
        }

        public async Task<PatientReadDTO?> GetPatientByIdAsync(int id)
        {
            var patient = await _repository.GetByIdAsync(id);
            if (patient == null) return null;

            return MapToReadDTO(patient);
        }

        public async Task<PatientReadDTO> CreatePatientAsync(PatientCreateDTO createDto)
        {
            // Mapping du CreateDTO vers l'Entité de base de données
            var patientEntity = new Patient
            {
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                BirthDate = createDto.BirthDate,
                Gender = createDto.Gender,
                Address = createDto.Address,
                PhoneNumber = createDto.PhoneNumber
            };

            await _repository.AddAsync(patientEntity);
            await _repository.SaveChangesAsync();

            // Envoi du ReadDTO (qui contient l'Id généré par la BDD)
            return MapToReadDTO(patientEntity);
        }

        public async Task<bool> UpdatePatientAsync(int id, PatientUpdateDTO updateDto)
        {
            var existingPatient = await _repository.GetByIdAsync(id);
            if (existingPatient == null) return false;

            // Mise à jour des champs autorisés par le UpdateDTO
            existingPatient.FirstName = updateDto.FirstName;
            existingPatient.LastName = updateDto.LastName;
            existingPatient.BirthDate = updateDto.BirthDate;
            existingPatient.Gender = updateDto.Gender;
            existingPatient.Address = updateDto.Address;
            existingPatient.PhoneNumber = updateDto.PhoneNumber;

            _repository.Update(existingPatient);
            return await _repository.SaveChangesAsync();
        }

        // Méthodes d'aide au Mapping et calcul d'âge
        private static PatientReadDTO MapToReadDTO(Patient patient)
        {
            return new PatientReadDTO
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Gender = patient.Gender,
                Address = patient.Address,
                PhoneNumber = patient.PhoneNumber,
                Age = CalculateAge(patient.BirthDate)
            };
        }

        private static int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            // Gestion cas où l'anniversaire n'est pas encore passé cette année
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}