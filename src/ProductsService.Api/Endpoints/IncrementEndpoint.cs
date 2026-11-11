using ProductsService.Business.Exceptions;

namespace ProductsService.Api.Endpoints;

public static class IncrementEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/{productId:int}/add-to-stock/{quantity}", async (int productId, int quantity, IProductService productService, CancellationToken ct) =>
        {
            if(quantity <= 0)
                throw new StockQuantityOutOfRangeException(quantity);

            await productService.UpdateStockQuantity(productId, quantity, ct);
            return Results.Ok();
        })
        .WithSummary("Increment stock")
        .WithDescription("Increments the stock quantity of a product.")
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}