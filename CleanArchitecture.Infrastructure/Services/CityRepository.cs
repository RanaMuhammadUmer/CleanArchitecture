using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services
{
    public class CityRepository : ICityRepository
    {
        private readonly CleanArchitectureDbContext _context;

        public CityRepository(CleanArchitectureDbContext context)
        {
            _context = context;
        }

        public Task<City> Add(City city)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<City>> Get()
        {
            return await _context.Cities.ToListAsync();
        }

        public Task<City?> GetById(Guid Id)
        {
            throw new NotImplementedException();
        }
    }
}
