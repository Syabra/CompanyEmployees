using LoggerService;

namespace CompanyEmployees.Extensions
{
    public static class LoggerServiceExtension
    {
        public static void ConfigureLoggerService(this IServiceCollection services) => services.AddScoped<ILoggerManager, LoggerManager>();
    }
}
