using ProductsService.Data.Common.Models;
using ProductsService.Data.Entities;

namespace ProductsService.Data.Interfaces;

public interface IProductRepository
{
    Task<PagedList<Product>> FindAllAsync(ProductRequest productRequest, CancellationToken ct = default);
    Task<Product?> FindByIdAsync(int id, bool trackChanges, CancellationToken ct = default);
    Task<Product> CreateAsync(Product product, CancellationToken ct = default);
    Task<PagedList<Product>> SearchAsync(ProductSearchRequest request, CancellationToken ct);
    Task<PagedList<Product>> FindByStockAsync(StockRangeRequest request, CancellationToken ct);
    Product Update(Product product);
    void Remove(Product product);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task<int> UpdateStockQuantity(int productId, int quantity, CancellationToken ct = default);

}