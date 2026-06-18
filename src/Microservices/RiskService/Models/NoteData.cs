namespace RiskService.Models
{
    public class NoteData
    {
        public string Id { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}