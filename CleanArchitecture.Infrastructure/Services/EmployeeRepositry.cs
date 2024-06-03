using CleanArchitecture.Application.Helper;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Services
{
    public class EmployeeRepositry : IEmployeeRepositry
    {
        private readonly CleanArchitectureDbContext _context;

        public EmployeeRepositry(CleanArchitectureDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> Add(Employee employee)
        {
            await _context.Employees.AddAsync(employee);

            return employee;
        }

        public async Task<bool> Delete(Guid Id)
        {
            bool isDeleted = true;
            var emplyee = await GetById(Id);

            if (emplyee != null)
            {
                _context.Employees.Remove(emplyee);
            }
            else
            {
                isDeleted = false;
            }

            return isDeleted;
        }

        public async Task<PagedList<Employee>> Get(int page, int pageSize)
        {
            var query = _context.Employees;
             return await PagedList<Employee>.CreateAsync(query, page, pageSize);
        }

        public Task<Employee?> GetById(Guid Id)
        {
            return _context.Employees.SingleOrDefaultAsync(x => x.Guid == Id);
        }
    }
}
