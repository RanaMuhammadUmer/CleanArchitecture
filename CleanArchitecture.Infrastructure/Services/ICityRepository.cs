using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Infrastructure.Services
{
    public interface ICityRepository
    {
        Task<List<City>> Get();
        Task<City?> GetById(Guid Id);
        Task<City> Add(City city);
        Task<bool> Delete(Guid Id);
    }
}
