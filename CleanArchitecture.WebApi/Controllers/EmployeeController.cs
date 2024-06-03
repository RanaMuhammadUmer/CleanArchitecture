using AutoMapper;
using CleanArchitecture.Application.Dto;
using CleanArchitecture.Application.Helper;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebApi.Controllers
{
   // [Authorize]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepositry _employeeRepositry;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeController(IEmployeeRepositry employeeRepositry, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _employeeRepositry = employeeRepositry;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Return All Employees
        /// </summary>
        /// <response code="200">Return All Employees</response>
        /// <response code="400">The server was unable to process the request</response>
        /// <response code="401">Client is not authenticated</response>
        /// <response code="403">Client lack sufficient permission to perform the request</response>
        /// <response code="500">Unexpected Error encountered</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<PagedList<EmployeeDto>>> Get(int page, int pageSize)
        {
            var employees = await _employeeRepositry.Get(page, pageSize);

            var employeeDto = PagedList<EmployeeDto>.CreateDto(_mapper.Map<List<EmployeeDto>>(employees.Items),
                employees.Page,
                employees.PageSize,
                employees.TotalCount
            );

            return Ok(employeeDto);
        }

        /// <summary>
        /// Creat an Employee
        /// </summary>
        /// <response code="201">Create an Employees</response>
        /// <response code="400">The server was unable to process the request</response>
        /// <response code="401">Client is not authenticated</response>
        /// <response code="403">Client lack sufficient permission to perform the request</response>
        /// <response code="500">Unexpected Error encountered</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<ActionResult> Post(CreateEmployeeRequestParameters param)
        {
            if(ModelState.IsValid)
            {

            }

            var employee = _mapper.Map<Employee>(param);

            employee = await _employeeRepositry.Add(employee);

            if (employee is null)
            {
                return BadRequest("Unable to create a new employee");
            }
            await _unitOfWork.SaveChangesAsync();

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return Created("", employeeDto);
        }

        /// <summary>
        /// Delete an Employee
        /// </summary>
        /// <response code="204">Delete an Employees</response>
        /// <response code="400">The server was unable to process the request</response>
        /// <response code="401">Client is not authenticated</response>
        /// <response code="403">Client lack sufficient permission to perform the request</response>
        /// <response code="500">Unexpected Error encountered</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]

        [Route("Id")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var employee = await _employeeRepositry.Delete(Id);

            if (!employee)
            {
                return BadRequest("Unable to delete the employee of given Id");
            }
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
