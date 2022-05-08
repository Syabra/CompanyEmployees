using Entities;
using NLog;
using NLog.Web;
using Microsoft.EntityFrameworkCore;
using Contracts;
using Repository;
using LoggerService;

var logger = LogManager.Setup().LoadConfigurationFromFile().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Canfigure dataBase connection
    builder.Services.AddDbContext<RepositoryContext>(
        opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"),
        opt2 => opt2.MigrationsAssembly("CompanyEmployees")));

    builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
    builder.Services.AddScoped<ILoggerManager, LoggerManager>();

    builder.Services.AddAutoMapper(typeof(Program).Assembly);
    builder.Services.AddControllers();

    // NLog: Setup NLog for DI
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    var app = builder.Build();


    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseAuthorization();

    app.MapControllers();

    app.UseRouting();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
    });

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Stopped brogram because of exeption");
    throw;
}
finally
{
    LogManager.Shutdown();
}
