using System.ComponentModel.DataAnnotations;

namespace PatientService.DTOs
{
    public class PatientCreateDTO
    {
        [Required(ErrorMessage = "Le prénom est obligatoire.")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire.")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "La date de naissance est obligatoire.")]
        public DateOnly BirthDate { get; set; }

        [Required(ErrorMessage = "Le genre est obligatoire.")]
        public required string Gender { get; set; }

        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}