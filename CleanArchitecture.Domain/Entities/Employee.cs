namespace CleanArchitecture.Domain.Entities
{
    public class Employee : Entity
    {
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public DateTime? Dob { get; private set; }
        public string? PhoneNumber { get; private set; } = string.Empty;

        public Employee(Guid guid, string name, string email, DateTime dob, string phoneNumber) : base(guid)
        {
            Name = name;
            Email = email;
            Dob = dob;
            PhoneNumber = phoneNumber;
        }

        private Employee()
        {
            
        }
    }
}
