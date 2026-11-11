using ProductsService.Business.Exceptions;
using ProductsService.Business.Interfaces;
using ProductRequest = ProductsService.Data.Common.Models.ProductRequest;

namespace ProductsService.Business.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<PagedList<ProductResponse>> GetProductsAsync(ProductRequest productRequest, CancellationToken ct = default)
    {
        var products = await productRepository.FindAllAsync(productRequest, ct);
        var productDto = products.Adapt<PagedList<ProductResponse>>();
        return productDto;
    }

    public async Task<ProductResponse> GetProductByIdAsync(int productId, CancellationToken ct = default)
    {
        var product = await GetProductAndCheckIfItExists(productId, cancellationToken: ct);
        return product.Adapt<ProductResponse>();
    }

    public async Task<ProductCreationResponse> CreateProductAsync(ProductAddRequest productAddRequest, CancellationToken ct = default)
    {
        var product = productAddRequest.Adapt<Product>();
        var newProduct = await productRepository.CreateAsync(product, ct);
        await productRepository.SaveChangesAsync(ct);
        return newProduct.Adapt<ProductCreationResponse>();
    }

    public async Task UpdateProductAsync(int productId, ProductUpdateRequest productUpdateRequest, CancellationToken ct = default)
    {
        var product = await GetProductAndCheckIfItExists(productId, trackChanges: true, cancellationToken: ct);
        productUpdateRequest.Adapt(product);
        product.UpdatedAt = DateTime.UtcNow;
        await productRepository.SaveChangesAsync(ct);
    }

    public async Task DeleteProductAsync(int productId, CancellationToken ct = default)
    {
        var product = await GetProductAndCheckIfItExists(productId, cancellationToken: ct);
        productRepository.Remove(product);
        await productRepository.SaveChangesAsync(ct);
    }

    public async Task UpdateStockQuantity(int productId, int quantity, CancellationToken ct = default)
    {
        if (await productRepository.UpdateStockQuantity(productId, quantity, ct) == 0)
            throw new ProductNotFoundException(productId);
    }

    public async Task<PagedList<ProductResponse>> GetProductsByNameAsync(ProductSearchRequest productRequest, CancellationToken ct = default)
    {
        return (await productRepository.SearchAsync(productRequest, ct)).Adapt<PagedList<ProductResponse>>();
    }

    public async Task<PagedList<ProductResponse>> GetProductsByStockAsync(StockRangeRequest request, CancellationToken ct = default) =>
        (await productRepository.FindByStockAsync(request, ct)).Adapt<PagedList<ProductResponse>>();

    private async Task<Product> GetProductAndCheckIfItExists(int productId, bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.FindByIdAsync(productId, trackChanges, cancellationToken);
        return product ?? throw new ProductNotFoundException(productId);
    }
}