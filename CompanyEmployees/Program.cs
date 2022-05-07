using Entities;
using NLog;
using NLog.Web;
using Microsoft.EntityFrameworkCore;

var logger = LogManager.Setup().LoadConfigurationFromFile().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Canfigure dataBase connection
    builder.Services.AddDbContext<RepositoryContext>(
        opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"),
        opt2 => opt2.MigrationsAssembly("CompanyEmployees")));

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    // builder.Services.AddSwaggerGen();

    // NLog: Setup NLog for DI
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseAuthorization();

    app.MapControllers();

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
