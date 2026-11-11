using Npgsql;
using ProductsService.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using ProductsService.Data.Common.Extensions;
using ProductsService.Data.Common.Models;
using ProductsService.Data.Entities;
using ProductsService.Data.Interfaces;

namespace ProductsService.Data.Repositories;

public sealed class ProductRepository(AppDbContext dbContext) : IProductRepository
{
    public Task<PagedList<Product>> FindAllAsync(ProductRequest productRequest, CancellationToken ct) =>
        dbContext.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .ToPagedListAsync(productRequest, ct);

    public Task<PagedList<Product>> SearchAsync(ProductSearchRequest request, CancellationToken ct) =>
        dbContext.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .Where(p => p.Name.ToLower().Contains(request.Name.Trim().ToLower()))
            .OrderBy(p => p.Id)
            .ToPagedListAsync(request, ct);

    public Task<PagedList<Product>> FindByStockAsync(StockRangeRequest request, CancellationToken ct) =>
        dbContext.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .Where(p => p.Stock >= request.MinStock && p.Stock <= request.MaxStock)
            .OrderBy(p => p.Id).ToPagedListAsync(request, ct);

    public Task<Product?> FindByIdAsync(int productId, bool trackChanges, CancellationToken ct = default) => 
        dbContext.Products
            .Include(p => p.Category)
            .TrackChanges(trackChanges)
            .SingleOrDefaultAsync(p => p.Id == productId, ct);

    public async Task<Product> CreateAsync(Product product, CancellationToken ct = default) => 
        (await dbContext.Products.AddAsync(product, ct)).Entity;

    public Product Update(Product product) => 
        dbContext.Products.Update(product).Entity;

    public void Remove(Product product) => 
        dbContext.Products.Remove(product);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => dbContext.SaveChangesAsync(ct);

    public async Task<int> UpdateStockQuantity(int productId, int quantity, CancellationToken ct = default)
    {
        try
        {
            return await dbContext.Products
                .Where(p => p.Id == productId)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(p => p.Stock, p => p.Stock + quantity)
                        .SetProperty(p => p.UpdatedAt, DateTime.UtcNow), ct);
        }
        catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.NumericValueOutOfRange)
        {
            throw new StockOverflowException(ex);
        }
    }
}
