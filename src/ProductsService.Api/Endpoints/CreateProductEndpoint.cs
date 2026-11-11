using ProductsService.Api.Filters;

namespace ProductsService.Api.Endpoints;

public static class CreateProductEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (ProductAddRequest product, IProductService productService, CancellationToken ct) => 
        {
            var result = await productService.CreateProductAsync(product, ct);
            return Results.Created($"/api/v1/products/{result.Id}", result);
        })
        .WithSummary("Create product")
        .WithDescription("Creates a new product.")
        .Produces<ProductCreationResponse>(StatusCodes.Status201Created)
        .AddEndpointFilter<ValidationFilter<ProductAddRequest>>();
    }
}