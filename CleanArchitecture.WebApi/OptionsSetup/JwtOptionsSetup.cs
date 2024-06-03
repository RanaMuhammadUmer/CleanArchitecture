using CleanArchitecture.Application;
using CleanArchitecture.Application.OptionsSettings;
using Microsoft.Extensions.Options;

namespace TestApi.Configuration.Options
{
    public class JwtOptionsSetup : IConfigureOptions<JwtSettings>
    {
        private readonly IConfiguration _configuration;

        public JwtOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(JwtSettings options)
        {
            _configuration.GetSection(nameof(JwtSettings)).Bind(options);

        }
    }
}
