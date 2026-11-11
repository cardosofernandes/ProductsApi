namespace ProductsService.Api.Endpoints;

public static class GetProductByIdEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("{productId:int}", async (int productId, IProductService productService, CancellationToken ct) =>
        {
            var result = await productService.GetProductByIdAsync(productId, ct);
            return Results.Ok(result);
        })
        .WithSummary("Get product by ID")
        .WithDescription("Returns the details of a product.")
        .Produces<ProductResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound);

    }
}