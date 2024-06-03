namespace CleanArchitecture.Application.Dto
{
    public class AuthResult
    {
        public string Token { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }
}
