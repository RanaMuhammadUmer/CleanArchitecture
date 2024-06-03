using CleanArchitecture.Application.Helper;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Infrastructure.Services
{
    public interface IEmployeeRepositry
    {
        Task<PagedList<Employee>> Get(int page, int pageSize);
        Task<Employee?> GetById(Guid Id);
        Task<Employee> Add(Employee employee);
        Task<bool> Delete(Guid Id);
    }
}
