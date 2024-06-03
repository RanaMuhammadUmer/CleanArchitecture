using CleanArchitecture.Application.Dto;
using CleanArchitecture.Application.OptionsSettings;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanArchitecture.Infrastructure.Authentication
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtSettings _options;
        private readonly CleanArchitectureDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TokenValidationParameters _tokenValidationParameter;
        public JwtProvider(IOptions<JwtSettings> options, CleanArchitectureDbContext context, TokenValidationParameters tokenValidationParameter, UserManager<IdentityUser> userManager)
        {
            _options = options.Value;
            _context = context;
            _tokenValidationParameter = tokenValidationParameter;
            _userManager = userManager;
        }

        public async Task<AuthResult> Generate(IdentityUser user)
        {
            var claims = new Claim[]
            {
                new (JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new (JwtRegisteredClaimNames.Email,user.Email!.ToString()),
                new (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
        };

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_options.Issure,
                _options.Audience,
                claims, null,
                DateTime.UtcNow.Add(_options.ExpiryTime),
                signingCredentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                Token = RandomStringGenerator(22),
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(30),
                IsRevok = false,
                IsUsed = false,
                UserId = user.Id
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthResult
            {
                Token = jwt,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthResult> VerifyGenerateToken(TokenRequest request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                _tokenValidationParameter.ValidateLifetime = false;

                var tokenInVarification = tokenHandler.ValidateToken(request.Token,
                    _tokenValidationParameter, out var validToken);

                if (validToken is JwtSecurityToken jwtSecurityToken)
                {
                    bool result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (!result)
                    {
                        return null!;
                    }
                }

                var utcExpiryDate = long.Parse(tokenInVarification.Claims
                    .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)!.Value);

                var expDate = UnixTimestampToDateTime(utcExpiryDate);

                if (expDate > DateTime.Now)
                {
                    return new AuthResult
                    {
                        Error = "Token is not expire Yet"
                    };
                }

                var storeToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == request.RefreshToken);

                if (storeToken == null || storeToken.IsUsed || storeToken.IsUsed)
                {
                    return new AuthResult
                    {
                        Error = "Invalid Token"
                    };
                }

                var jti = tokenInVarification.Claims
                    .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)!.Value;

                if (storeToken.JwtId != jti)
                {
                    return new AuthResult
                    {
                        Error = "Invalid Token"
                    };
                }

                if (storeToken.ExpiryDate < DateTime.UtcNow)
                {
                    return new AuthResult
                    {
                        Error = "Expire Token"
                    };
                }

                storeToken.IsUsed = true;
                _context.RefreshTokens.Update(storeToken);
                await _context.SaveChangesAsync();

                var user = await _userManager.FindByIdAsync(storeToken.UserId);

                return await Generate(user!);
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    Error = ex.Message
                };
            }
        }

        private static DateTime UnixTimestampToDateTime(long utcExpiryDate)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            return dateTimeVal.AddSeconds(utcExpiryDate);
        }

        private static string RandomStringGenerator(int length)
        {
            var random = new Random();

            var chars = "ABCDEFGHIGKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwxyz";

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
