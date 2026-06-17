using System.ComponentModel.DataAnnotations;

namespace FrontendService.Models
{
    public class NoteCreateViewModel
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;
    }
}