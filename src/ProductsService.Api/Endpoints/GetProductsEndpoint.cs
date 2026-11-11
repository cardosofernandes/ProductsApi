using ProductsService.Api.Filters;
using ProductsService.Data.Common.Models;

namespace ProductsService.Api.Endpoints;

public static class GetProductsEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("", async ([AsParameters]ProductRequest productRequest, IProductService productService, CancellationToken ct) => 
        {
            var result = await productService.GetProductsAsync(productRequest, ct);
            return Results.Ok(result);
        })
        .WithSummary("Get all products")
        .WithDescription("Returns the list of products.")
        .Produces<PagedList<ProductResponse>>()
        .AddEndpointFilter<ValidationFilter<ProductRequest>>();
    }
}