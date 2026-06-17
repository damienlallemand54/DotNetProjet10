using System.ComponentModel.DataAnnotations;

namespace FrontendService.Models
{
    public class PatientEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public DateOnly BirthDate { get; set; }

        [Required]
        public string Gender { get; set; } = string.Empty;

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }
    }
}