using ProductsService.Business.Exceptions;

namespace ProductsService.Api.Endpoints;

public static class DecrementEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/{productId:int}/decrement-stock/{quantity}", async (int productId, int quantity, IProductService productService, CancellationToken ct) =>
        {
            if(quantity <= 0)
                throw new StockQuantityOutOfRangeException(quantity);

            await productService.UpdateStockQuantity(productId, -quantity, ct);
            return Results.Ok();
        })
        .WithSummary("Decrement stock")
        .WithDescription("Decrements the stock quantity of a product.")
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}