namespace CleanArchitecture.Application.Dto
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? Dob { get; set; }
        public string? PhoneNumber { get; set; } = string.Empty;
    }
}
