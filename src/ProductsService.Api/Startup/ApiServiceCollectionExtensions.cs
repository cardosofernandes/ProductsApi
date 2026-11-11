using Asp.Versioning;
using ProductsService.Api.Errors;

namespace ProductsService.Api.Startup;

public static class ApiServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddProblemDetailsErrorHandling()
        {
            services.AddProblemDetails(o => o.CustomizeProblemDetails = ctx =>
            {
                ctx.ProblemDetails.Extensions["traceId"] = ctx.HttpContext.TraceIdentifier;
                ctx.ProblemDetails.Extensions["timestamp"] = DateTime.UtcNow;
                ctx.ProblemDetails.Instance = $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}";
            });
            
            services.AddExceptionHandler<GlobalExceptionHandler>();
            return services;
        }

        public void AddApiDocumentation()
        {
            services
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.ReportApiVersions = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ApiVersionReader = new UrlSegmentApiVersionReader();

                })
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                })
                .AddOpenApi();
        }
    }
}