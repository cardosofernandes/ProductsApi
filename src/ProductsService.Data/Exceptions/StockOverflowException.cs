namespace ProductsService.Data.Exceptions;

public sealed class StockOverflowException(Exception innerException) : Exception("The resulting stock exceeds the supported range.", innerException);
