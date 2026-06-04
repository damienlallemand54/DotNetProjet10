namespace PatientService.DTOs
{
    public class PatientReadDTO
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public required string Gender { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}