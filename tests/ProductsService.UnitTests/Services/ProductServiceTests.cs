using Mapster;
using Moq;
using ProductsService.Business.DTO;
using ProductsService.Business.Exceptions;
using ProductsService.Business.Mapping;
using ProductsService.Business.Services;
using ProductsService.Data.Common.Models;
using ProductsService.Data.Entities;
using ProductsService.Data.Interfaces;
using Shouldly;

namespace ProductsService.UnitTests.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repository = new(MockBehavior.Strict);
    private readonly ProductService _service;
    private readonly CancellationToken _ct = new CancellationTokenSource().Token;

    static ProductServiceTests() => new ProductMappingConfig().Register(TypeAdapterConfig.GlobalSettings);

    public ProductServiceTests() => _service = new ProductService(_repository.Object);

    private static Product Product() => new()
    {
        Id = 100100, Name = "Keyboard", Description = "Mechanical", Price = 49.99m,
        Stock = 10, CategoryId = 1, Category = new Category { Id = 1, Name = "Electronics" }
    };

    private static void AssertPage(PagedList<ProductResponse> result)
    {
        result.PageNumber.ShouldBe(2);
        result.PageSize.ShouldBe(1);
        result.TotalCount.ShouldBe(3);
        result.TotalPages.ShouldBe(3);
        result.HasPrevious.ShouldBeTrue();
        result.HasNext.ShouldBeTrue();
        result.Items.Single().ShouldBe(new ProductResponse(100100, "Keyboard", "Mechanical", 49.99m, 10, "Electronics"));
    }

    [Fact]
    public async Task GetProducts_returns_mapped_page_and_metadata()
    {
        var request = new ProductRequest { PageNumber = 2, PageSize = 1 };
        _repository.Setup(r => r.FindAllAsync(request, _ct))
            .ReturnsAsync(new PagedList<Product>([Product()], 2, 1, 3));
        AssertPage(await _service.GetProductsAsync(request, _ct));
    }

    [Fact]
    public async Task Search_returns_mapped_page_and_metadata()
    {
        var request = new ProductSearchRequest { Name = "key", PageNumber = 2, PageSize = 1 };
        _repository.Setup(r => r.SearchAsync(request, _ct))
            .ReturnsAsync(new PagedList<Product>([Product()], 2, 1, 3));
        AssertPage(await _service.GetProductsByNameAsync(request, _ct));
    }

    [Fact]
    public async Task Stock_filter_returns_mapped_page_and_metadata()
    {
        var request = new StockRangeRequest { MinStock = 1, MaxStock = 20, PageNumber = 2, PageSize = 1 };
        _repository.Setup(r => r.FindByStockAsync(request, _ct))
            .ReturnsAsync(new PagedList<Product>([Product()], 2, 1, 3));
        AssertPage(await _service.GetProductsByStockAsync(request, _ct));
    }

    [Fact]
    public async Task GetProduct_returns_details_including_category_name()
    {
        _repository.Setup(r => r.FindByIdAsync(100100, false, _ct)).ReturnsAsync(Product());
        (await _service.GetProductByIdAsync(100100, _ct))
            .ShouldBe(new ProductResponse(100100, "Keyboard", "Mechanical", 49.99m, 10, "Electronics"));
    }

    [Theory]
    [InlineData("get")]
    [InlineData("update")]
    [InlineData("delete")]
    public async Task Missing_product_throws_without_saving(string operation)
    {
        _repository.Setup(r => r.FindByIdAsync(123456, operation == "update", _ct)).ReturnsAsync((Product?)null);
        var exception = await Should.ThrowAsync<ProductNotFoundException>(async () =>
        {
            switch (operation)
            {
                case "get": await _service.GetProductByIdAsync(123456, _ct); break;
                case "update": await _service.UpdateProductAsync(123456, new("New", null, 1m, 1), _ct); break;
                case "delete": await _service.DeleteProductAsync(123456, _ct); break;
            }
        });
        exception.Message.ShouldContain("123456");
        _repository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Create_maps_request_saves_and_returns_generated_id()
    {
        Product? captured = null;
        _repository.Setup(r => r.CreateAsync(It.IsAny<Product>(), _ct))
            .ReturnsAsync((Product product, CancellationToken _) =>
            {
                captured = product;
                return product;
            });
        _repository.Setup(r => r.SaveChangesAsync(_ct)).Callback(() => captured!.Id = 100123).ReturnsAsync(1);

        var result = await _service.CreateProductAsync(new("Keyboard", "Mechanical", 49.99m, 10, 1), _ct);

        result.ShouldBe(new ProductCreationResponse(100123, "Keyboard", "Mechanical", 49.99m, 10, 1));
        _repository.Verify(r => r.SaveChangesAsync(_ct), Times.Once);
    }

    [Fact]
    public async Task Update_changes_details_and_preserves_stock()
    {
        var product = Product();
        _repository.Setup(r => r.FindByIdAsync(product.Id, true, _ct)).ReturnsAsync(product);
        _repository.Setup(r => r.SaveChangesAsync(_ct)).ReturnsAsync(1);

        await _service.UpdateProductAsync(product.Id, new("Updated", null, 25m, 2), _ct);

        product.Name.ShouldBe("Updated");
        product.Description.ShouldBeNull();
        product.Price.ShouldBe(25m);
        product.CategoryId.ShouldBe(2);
        product.Stock.ShouldBe(10);
        _repository.Verify(r => r.SaveChangesAsync(_ct), Times.Once);
    }

    [Fact]
    public async Task Delete_removes_existing_product_and_saves()
    {
        var product = Product();
        _repository.Setup(r => r.FindByIdAsync(product.Id, false, _ct)).ReturnsAsync(product);
        _repository.Setup(r => r.Remove(product));
        _repository.Setup(r => r.SaveChangesAsync(_ct)).ReturnsAsync(1);
        await _service.DeleteProductAsync(product.Id, _ct);
        _repository.Verify(r => r.Remove(product), Times.Once);
        _repository.Verify(r => r.SaveChangesAsync(_ct), Times.Once);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(-5)]
    public async Task Stock_update_passes_signed_quantity_to_repository(int quantity)
    {
        _repository.Setup(r => r.UpdateStockQuantity(100100, quantity, _ct)).ReturnsAsync(1);
        await _service.UpdateStockQuantity(100100, quantity, _ct);
        _repository.Verify(r => r.UpdateStockQuantity(100100, quantity, _ct), Times.Once);
    }

    [Fact]
    public async Task Stock_update_throws_when_product_does_not_exist()
    {
        _repository.Setup(r => r.UpdateStockQuantity(123456, 5, _ct)).ReturnsAsync(0);
        await Should.ThrowAsync<ProductNotFoundException>(() => _service.UpdateStockQuantity(123456, 5, _ct));
    }

    [Fact]
    public async Task Create_propagates_save_failure()
    {
        var failure = new InvalidOperationException("Save failed");
        _repository.Setup(r => r.CreateAsync(It.IsAny<Product>(), _ct))
            .ReturnsAsync((Product product, CancellationToken _) => product);
        _repository.Setup(r => r.SaveChangesAsync(_ct)).ThrowsAsync(failure);
        var actual = await Should.ThrowAsync<InvalidOperationException>(() =>
            _service.CreateProductAsync(new("Keyboard", null, 1m, 1, 1), _ct));
        actual.ShouldBeSameAs(failure);
    }
}
