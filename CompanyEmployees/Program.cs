using Entities;
using NLog;
using NLog.Web;
using Microsoft.EntityFrameworkCore;
using Contracts;
using Repository;
using LoggerService;
using CompanyEmployees.Extensions;
using Microsoft.AspNetCore.Mvc;

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

var builder = WebApplication.CreateBuilder(args);

// Canfigure dataBase connection
builder.Services.AddDbContext<RepositoryContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"),
    opt2 => opt2.MigrationsAssembly("CompanyEmployees")));

builder.Services.Configure<ApiBehaviorOptions>(opt =>
    opt.SuppressModelStateInvalidFilter = true);

builder.Services.ConfigureLoggerService();

builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<ILoggerManager, LoggerManager>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson()
  .AddXmlDataContractSerializerFormatters()
  .AddCustomCSVFormatter();

// NLog: Setup NLog for DI
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();


var app = builder.Build();

if(builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}
var logger = new LoggerManager();
app.ConfigureExceptionHandler(logger);

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
