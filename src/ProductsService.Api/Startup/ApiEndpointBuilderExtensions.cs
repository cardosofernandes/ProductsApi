using Scalar.AspNetCore;

namespace ProductsService.Api.Startup;

public static class ApiEndpointBuilderExtensions
{
    extension(IEndpointRouteBuilder app)
    {
        public void MapDocumentation()
        {
            app
                .MapOpenApi()
                .WithDocumentPerVersion();

            app.MapScalarApiReference(options =>
            {
                options.WithTitle("Products API");
                foreach (var version in app.DescribeApiVersions())
                    options.AddDocument(version.GroupName, version.GroupName);
            });
        }
    }
}