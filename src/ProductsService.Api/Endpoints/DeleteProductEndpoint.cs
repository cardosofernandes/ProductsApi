namespace ProductsService.Api.Endpoints;

public static class DeleteProductEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapDelete("{productId:int}", async (int productId, IProductService productService, CancellationToken ct) =>
        {
            await productService.DeleteProductAsync(productId, ct);
            return Results.NoContent();
        })
        .WithSummary("Delete product")
        .WithDescription("Deletes a product by Id.")
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}