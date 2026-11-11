using ProductsService.Api.Filters;
using ProductsService.Data.Common.Models;

namespace ProductsService.Api.Endpoints;

public static class FilterProductsEndpoints
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/search", async ([AsParameters] ProductSearchRequest request, IProductService service, CancellationToken ct) =>
        {
            var result = await service.GetProductsByNameAsync(request, ct);
            return Results.Ok(result);
        })
        .Produces<PagedList<ProductResponse>>().ProducesValidationProblem()
        .AddEndpointFilter<ValidationFilter<ProductSearchRequest>>();
        
        app.MapGet("/stock-level", async ([AsParameters] StockRangeRequest request, IProductService service, CancellationToken ct) =>
        {
            var result = await service.GetProductsByStockAsync(request, ct);
            return Results.Ok(result);
        })
        .Produces<PagedList<ProductResponse>>().ProducesValidationProblem()
        .AddEndpointFilter<ValidationFilter<StockRangeRequest>>();
    }
}
