namespace FrontendService.Models
{
    public class PatientDetailsViewModel
    {
        public PatientViewModel Patient { get; set; } = new();
        public List<NoteViewModel> Notes { get; set; } = new();
    }
}