namespace ProductsService.Business.Exceptions;

public abstract class AppException(string message) : Exception(message);

public sealed class ProductNotFoundException(int productId) : AppException($"Product with Id {productId} was not found.");

public sealed class StockQuantityOutOfRangeException(int quantity) : AppException($"Stock quantity {quantity} is out of range.");