using ProductsService.Api.Endpoints;
using ProductsService.Api.Startup;
using ProductsService.Business;
using ProductsService.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog((services, logger) => logger
    .ReadFrom.Configuration(builder.Configuration)
    .ReadFrom.Services(services));

builder.Services
    .AddProblemDetailsErrorHandling()
    .AddBusiness()
    .AddInfrastructure(builder.Configuration)
    .AddApiDocumentation();

builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(DatabaseOptions.SectionName));

var app = builder.Build();

app
    .UseExceptionHandler()
    .UseStatusCodePages()
    .UseSerilogRequestLogging();

app.MapAllEndpoints();
app.MapDocumentation();
await app.InitializeDatabaseAsync();

app.Run();

public partial class Program { }
