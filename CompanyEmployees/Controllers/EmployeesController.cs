﻿using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using LoggerService;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

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


        [HttpGet("{id}", Name = "GetEmployeesForCompany")]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if(company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");

                return NotFound();
            }
            else
            {
                var employeesFromDb = await _repository.Employee.GetEmployeesAsync(companyId, trackChanges: false);
                var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);

                return Ok(employeesDto);
            }
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
        public async Task<IActionResult> DeleteEmployeeCompany(Guid companyId, Guid id)
        {
            var company = _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if(company == null)
            {
                _logger.LogError($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var employeeForCompany = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges: false);

            if(employeeForCompany == null)
            {
                _logger.LogError($"Employee with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            _repository.Employee.DeleteEmployee(employeeForCompany);
            await _repository.SaveAsync();

            return NoContent();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody]
       EmployeeForUpdateDto employee)
        {
            if (employee == null)
            {
                _logger.LogError("EmployeeForUpdateDto object sent from client is null.");
                return BadRequest("EmployeeForUpdateDto object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the EmployeeForUpdateDto object");
                return UnprocessableEntity(ModelState);
            }

            var company = _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database."); 
                return NotFound();
            }

            var employeeEntity = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges: true);
            if (employeeEntity == null)
            {
                _logger.LogInfo($"Employee with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            _mapper.Map(employee, employeeEntity);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PartiallyUpdateEmployeeFromCompany(Guid companyId, Guid id, [FromBody]JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if(patchDoc == null)
            {
                _logger.LogError($"PatchDoc object sent from client is null");
                return BadRequest("PatchDoc object is null");
            }

            var company = _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if(company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database");
                return NotFound();
            }

            var employeeEntity = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges: true);
            if(employeeEntity == null)
            {
                _logger.LogInfo($"Employee with: {id} doesn't exist in the database");
                return NotFound();
            }

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
