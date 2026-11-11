namespace ProductsService.Business.Interfaces;

public interface IProductService
{
    Task<PagedList<ProductResponse>> GetProductsAsync(ProductRequest productRequest, CancellationToken ct = default);
    Task<ProductResponse> GetProductByIdAsync(int productId, CancellationToken ct = default);
    Task<ProductCreationResponse> CreateProductAsync(ProductAddRequest productResponse, CancellationToken ct = default);
    Task UpdateProductAsync(int productId, ProductUpdateRequest productUpdateRequest, CancellationToken ct = default);
    Task DeleteProductAsync(int productId, CancellationToken ct = default);
    Task<PagedList<ProductResponse>> GetProductsByStockAsync(StockRangeRequest request, CancellationToken ct = default);
    Task UpdateStockQuantity(int productId, int quantity, CancellationToken ct = default);
    Task<PagedList<ProductResponse>> GetProductsByNameAsync(ProductSearchRequest productRequest, CancellationToken ct = default);
}