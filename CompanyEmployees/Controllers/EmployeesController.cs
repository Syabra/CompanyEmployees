using AutoMapper;
using CompanyEmployees.ActionFilters.Filters;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using LoggerService;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CompanyEmployees.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
            
        public EmployeesController(IRepositoryManager repository, IMapper mapper, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, [FromQuery]EmployeeParameters employeeParameters)
        {
            if (!employeeParameters.ValidAgeRange)
                return BadRequest("Max age can't be less than min age.");

            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if(company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }                
            
            var employeesFromDb = await _repository.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(employeesFromDb.MetaData));

            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);

            return Ok(employeesDto);
        }


        [HttpPost]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody]EmployeeForCreationDto employee)
        {
            if(employee == null)
            {
                _logger.LogError("Employee object sent from client is null");
                return BadRequest("Employee object is null");
            }
            
            if(!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the EmployeeForCreationDto object");
                return UnprocessableEntity(ModelState);
            }

            var company = _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if(company == null)
            {
                _logger.LogError($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var empoyeeEntity = _mapper.Map<Employee>(employee);
            _repository.Employee.CreateEmployeeForCompany(companyId, empoyeeEntity);
            await _repository.SaveAsync();

            var employeeToReturn = _mapper.Map<EmployeeDto>(empoyeeEntity);
            return CreatedAtRoute("GetEmployeesForCompany", new {companyId, id = employeeToReturn.Id}, employeeToReturn);
        }


        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public async Task<IActionResult> DeleteEmployeeCompany(Guid companyId, Guid id)
        {
            var employeeForCompany = HttpContext.Items["employee"] as Employee;

            _repository.Employee.DeleteEmployee(employeeForCompany);
            await _repository.SaveAsync();

            return NoContent();
        }


        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody]
       EmployeeForUpdateDto employee)
        {
            var employeeEntity = HttpContext.Items["employee"] as Employee;

            _mapper.Map(employee, employeeEntity);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateEmployeeFromCompany(Guid companyId, Guid id, [FromBody]JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if(patchDoc == null)
            {
                _logger.LogError($"PatchDoc object sent from client is null");
                return BadRequest("PatchDoc object is null");
            }

            var employeeEntity = HttpContext.Items["employee"] as Employee;

            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);

            patchDoc.ApplyTo(employeeToPatch, ModelState);
            TryValidateModel(employeeToPatch);
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(employeeToPatch, employeeEntity);
            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
