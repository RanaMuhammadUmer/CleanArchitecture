using AutoMapper;
using CleanArchitecture.Application.Dto;
using CleanArchitecture.Application.Helper;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace CleanArchitecture.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityRepository _cityRepositry;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CityController(ICityRepository cityRepositry, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _cityRepositry = cityRepositry;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Return All Cities
        /// </summary>
        /// <response code="200">Return All Cities</response>
        /// <response code="400">The server was unable to process the request</response>
        /// <response code="401">Client is not authenticated</response>
        /// <response code="403">Client lack sufficient permission to perform the request</response>
        /// <response code="500">Unexpected Error encountered</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[HttpGet]
        [HttpPost]
        public async Task<ActionResult<CityDto>> Get(int id)
        {
            var cities = await _cityRepositry.Get();

            var cityDto = _mapper.Map<List<CityDto>>(cities);

            return Ok(cityDto);
        }
    }
}
