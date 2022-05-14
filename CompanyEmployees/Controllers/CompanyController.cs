﻿using AutoMapper;
using Contracts;
using Entities;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public CompanyController(IRepositoryManager repository, IMapper mapper, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;   
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCompanies()
        {
            /* var companies = _repository.Company.GetAllCompanies(trackChanges: true);
             var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

             return Ok(companiesDto);*/
            throw new Exception("Exception");
        }
    }
}
