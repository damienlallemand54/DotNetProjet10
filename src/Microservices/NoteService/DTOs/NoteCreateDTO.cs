using System.ComponentModel.DataAnnotations;

namespace NoteService.DTOs
{
    public class NoteCreateDTO
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public string PatientName { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;
    }
}