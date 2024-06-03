using AutoMapper;
using CleanArchitecture.Application.Dto;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Profiles
{
    public class CreateEmployeeRequestParameterProfile : Profile
    {
        public CreateEmployeeRequestParameterProfile() {
            CreateMap<CreateEmployeeRequestParameters, Employee>();
        }
    }
}
