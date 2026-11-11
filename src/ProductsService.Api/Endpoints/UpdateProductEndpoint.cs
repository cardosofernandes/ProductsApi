using ProductsService.Api.Filters;

namespace ProductsService.Api.Endpoints;

public static class UpdateProductEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPut("{productId:int}", async (int productId, ProductUpdateRequest productUpdateRequest, IProductService productService, CancellationToken ct) =>
        {
            await productService.UpdateProductAsync(productId, productUpdateRequest, ct);
            return Results.NoContent();
        })
        .WithSummary("Update product")
        .WithDescription("Updates product details by Id. Use the dedicated stock endpoints to change stock quantities.")
        .ProducesProblem(StatusCodes.Status404NotFound)
        .AddEndpointFilter<ValidationFilter<ProductUpdateRequest>>();;

    }
}
