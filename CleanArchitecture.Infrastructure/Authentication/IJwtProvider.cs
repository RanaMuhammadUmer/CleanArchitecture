
using CleanArchitecture.Application.Dto;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Authentication
{
    public interface IJwtProvider
    {
        Task<AuthResult> Generate(IdentityUser user);
        Task<AuthResult> VerifyGenerateToken(TokenRequest user);
    }
}
