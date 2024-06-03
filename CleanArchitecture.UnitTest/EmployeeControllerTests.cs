
using CleanArchitecture.Application.Dto;
using CleanArchitecture.Application.Helper;
using System.Net;
using System.Net.Http.Json;

namespace CleanArchitecture.UnitTest
{
    public class EmployeeControllerTests:IntegrationTest
    {
        [Fact]
        public async Task Employee_controller_Get_Should_Return_Employees()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await _httpClient.GetAsync($"https://localhost:7068/Employee?page=2&pageSize=3");

            var result = await response.Content.ReadFromJsonAsync<PagedList<EmployeeDto>>();

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
            Assert.NotEmpty(result!.Items);
        }

        [Fact]
        public async Task Employee_Controller_Post_Should_Create_Employee()
        {
            //Arrange
            await AuthenticateAsync();

            var employeeCreationParameters = new CreateEmployeeRequestParameters
            {
                Name = "Asad",
                Email = "asd@gmail.com",
            };
            //Act
            var response = await _httpClient.PostAsJsonAsync($"https://localhost:7068/Employee",employeeCreationParameters);
            
            var result = await response.Content.ReadFromJsonAsync<EmployeeDto>();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(employeeCreationParameters.Name,result.Name);
            Assert.Equal(employeeCreationParameters.Email, result.Email);
        }
    }
}
