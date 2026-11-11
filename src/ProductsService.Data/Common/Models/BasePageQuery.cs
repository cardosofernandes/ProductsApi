namespace ProductsService.Data.Common.Models;

public abstract class PagedRequest
{
    public const int MaxPageSize = 50;
    
    public int PageNumber { get; init; } = 1;
    
    public int PageSize { get; init; } = 10;
}

public sealed class ProductRequest : PagedRequest
{
    public ProductRequest(int pageNumber = 1, int pageSize = 10)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public sealed class StockRangeRequest : PagedRequest
{
    public StockRangeRequest(int? minStock = null, int? maxStock = null, int pageNumber = 1, int pageSize = 10)
    {
        MinStock = minStock;
        MaxStock = maxStock;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public int? MinStock { get; init; }
    
    public int? MaxStock { get; init; }
}

public sealed class ProductSearchRequest : PagedRequest
{
    public ProductSearchRequest(string name = "", int pageNumber = 1, int pageSize = 10)
    {
        Name = name;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public string Name { get; init; } = string.Empty;
}
