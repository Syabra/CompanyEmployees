﻿using AutoMapper;
using Contracts;
using Entities;
using Entities.Models;
using LoggerService;
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
        public IActionResult GetEmployeesForCompany(Guid companyId)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges: false);
            if(company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");

                return NotFound();
            }
            else
            {
                var employeesFromDb = _repository.Employee.GetEmployees(companyId, trackChanges: false);
                var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);

                return Ok(employeesDto);
            }
        }

        [HttpPost]
        public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody]EmployeeForCreationDto employee)
        {
            if(employee == null)
            {
                _logger.LogError("Employee object sent from client is null");
                return BadRequest("Employee object is null");
            }

            var company = _repository.Company.GetCompany(companyId, trackChanges: false);
            if(company == null)
            {
                _logger.LogError($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }

            var empoyeeEntity = _mapper.Map<Employee>(employee);
            _repository.Employee.CreateEmployeeForCompany(companyId, empoyeeEntity);
            _repository.Save();

            var employeeToReturn = _mapper.Map<EmployeeDto>(empoyeeEntity);
            return CreatedAtRoute("GetEmployeesForCompany", new {companyId, id = employeeToReturn.Id}, employeeToReturn);
        }
    }
}
