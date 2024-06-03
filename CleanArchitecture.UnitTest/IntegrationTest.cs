
using CleanArchitecture.Application.Dto;
using CleanArchitecture.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CleanArchitecture.UnitTest
{
    public class IntegrationTest
    {
        protected readonly HttpClient _httpClient;

        public IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(CleanArchitectureDbContext));
                        services.AddDbContext<CleanArchitectureDbContext>(options =>
                        {
                            options.UseInMemoryDatabase("MyTestDb");
                        });
                    });
                });
            _httpClient = appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetJwtAsync());
        }

        private async Task<string?> GetJwtAsync()
        {
            var response = await _httpClient.PostAsJsonAsync($"https://localhost:7068/api/User/Login", new UserRegistrationRequestDto
            {
                Email = "abc@gmail.com",
                Password = "Abcd12345@"
            });

            var result = await response.Content.ReadFromJsonAsync<AuthResult>();

            return result?.Token;
        }
    }
}
