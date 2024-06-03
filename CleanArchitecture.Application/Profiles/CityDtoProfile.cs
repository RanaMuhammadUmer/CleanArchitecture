using AutoMapper;
using CleanArchitecture.Application.Dto;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Profiles
{
    internal class CityDtoProfile : Profile
    {
        public CityDtoProfile()
        {
            CreateMap<City, CityDto>();
        }
    }
}