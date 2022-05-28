using Contracts;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CompanyEmployees.ActionFilters.Filters
{
    public class ValidateEmployeeForCompanyExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public ValidateEmployeeForCompanyExistsAttribute(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = (method.Equals("PUT")) || (method.Equals("PATH")) ? true : false;
            var companyId = (Guid)context.ActionArguments["companyId"];

            var company = await _repository.Company.GetCompanyAsync(companyId, false);
            if (company == null)
            {
                _logger.LogError($"Company with id: {companyId} doesn't exist in the database.");
                context.Result = new NotFoundResult();
                return;
            }

            var id = (Guid)context.ActionArguments["id"];

            var employee = await _repository.Employee.GetEmployeeAsync(companyId, id, false);
            if(employee == null)
            {
                _logger.LogError($"Employee with id: {companyId} doesn't exist in the database.");
                context.Result = new NotFoundResult();             
            }
            else
            {
                context.HttpContext.Items.Add("employee", employee);
                await next();
            }

        }
    }
}
