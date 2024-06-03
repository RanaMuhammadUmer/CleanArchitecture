using AutoMapper;
using CleanArchitecture.Application.Dto;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Profiles
{
    public class EmployeeDtoProfile : Profile
    {
        public EmployeeDtoProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(d => d.Id,
                opt => opt.MapFrom(t => t.Guid));
        }
    }
}
