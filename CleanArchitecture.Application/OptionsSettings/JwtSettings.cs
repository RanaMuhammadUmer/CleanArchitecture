
namespace CleanArchitecture.Application.OptionsSettings
{
    public class JwtSettings
    {
        public string Key { get; set; } = string.Empty;
        public string Issure { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public TimeSpan ExpiryTime { get; set; }
    }
}
