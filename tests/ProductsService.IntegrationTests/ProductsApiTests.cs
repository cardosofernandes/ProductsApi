using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductsService.Data;
using ProductsService.Business.DTO;
using ProductsService.Data.Common.Models;
using Shouldly;

namespace ProductsService.IntegrationTests;

public sealed class ProductsApiTests(ProductsApiFactory factory) : IClassFixture<ProductsApiFactory>, IAsyncLifetime
{
    private const string Products = "/api/v1/products";
    private HttpClient _client = null!;

    public async Task InitializeAsync()
    {
        await factory.ResetAsync();
        _client = factory.CreateClient(new() { AllowAutoRedirect = false });
    }

    public Task DisposeAsync()
    {
        _client.Dispose();
        return Task.CompletedTask;
    }

    private async Task<ProductCreationResponse> CreateAsync(string name = "Keyboard", int stock = 10)
    {
        using var response = await _client.PostAsJsonAsync(Products, new ProductAddRequest(name, "Mechanical", 49.99m, stock, 1));
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var created = (await response.Content.ReadFromJsonAsync<ProductCreationResponse>()).ShouldNotBeNull();
        response.Headers.Location.ShouldBe(new Uri($"{Products}/{created.Id}", UriKind.Relative));
        return created;
    }

    private async Task<ProductResponse> GetAsync(int id)
    {
        using var response = await _client.GetAsync($"{Products}/{id}");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        return (await response.Content.ReadFromJsonAsync<ProductResponse>()).ShouldNotBeNull();
    }

    private static async Task<ProblemDetails> AssertProblemAsync(HttpResponseMessage response, HttpStatusCode status)
    {
        response.StatusCode.ShouldBe(status);
        response.Content.Headers.ContentType!.MediaType.ShouldBe("application/problem+json");
        var problem = (await response.Content.ReadFromJsonAsync<ProblemDetails>()).ShouldNotBeNull();
        AssertProblemDetails(problem, status);
        return problem;
    }

    private static void AssertProblemDetails(ProblemDetails problem, HttpStatusCode status)
    {
        problem.Status.ShouldBe((int)status);
        problem.Title.ShouldNotBeNullOrWhiteSpace();
        problem.Extensions.ShouldContainKey("traceId");
        problem.Extensions.ShouldContainKey("timestamp");
        problem.Instance.ShouldNotBeNullOrWhiteSpace();
    }

    private static async Task AssertValidationAsync(HttpResponseMessage response, string field)
    {
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType!.MediaType.ShouldBe("application/problem+json");
        var problem = (await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>()).ShouldNotBeNull();
        AssertProblemDetails(problem, HttpStatusCode.BadRequest);
        problem.Errors.ShouldContainKey(field);
    }

    [Fact]
    public async Task Create_returns_location_and_get_returns_persisted_product()
    {
        var created = await CreateAsync();
        created.Id.ShouldBeInRange(100000, 999999);
        created.ShouldBe(new ProductCreationResponse(created.Id, "Keyboard", "Mechanical", 49.99m, 10, 1));
        (await GetAsync(created.Id)).ShouldBe(new ProductResponse(created.Id, "Keyboard", "Mechanical", 49.99m, 10, "Electronics"));
    }

    [Fact]
    public async Task List_returns_ordered_page_and_metadata()
    {
        await CreateAsync("First");
        var second = await CreateAsync("Second");
        await CreateAsync("Third");
        using var response = await _client.GetAsync($"{Products}?PageNumber=2&PageSize=1");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var page = (await response.Content.ReadFromJsonAsync<PagedList<ProductResponse>>()).ShouldNotBeNull();
        page.Items.Single().Id.ShouldBe(second.Id);
        page.PageNumber.ShouldBe(2);
        page.PageSize.ShouldBe(1);
        page.TotalCount.ShouldBe(3);
        page.TotalPages.ShouldBe(3);
        page.HasPrevious.ShouldBeTrue();
        page.HasNext.ShouldBeTrue();
    }

    [Theory]
    [InlineData("?PageNumber=1&PageSize=10")]
    [InlineData("/search?Name=absent&PageNumber=1&PageSize=10")]
    [InlineData("/stock-level?MinStock=0&MaxStock=10&PageNumber=1&PageSize=10")]
    public async Task Empty_queries_return_empty_pages(string suffix)
    {
        using var response = await _client.GetAsync(Products + suffix);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var page = (await response.Content.ReadFromJsonAsync<PagedList<ProductResponse>>()).ShouldNotBeNull();
        page.Items.ShouldBeEmpty();
        page.TotalCount.ShouldBe(0);
        page.TotalPages.ShouldBe(0);
        page.HasNext.ShouldBeFalse();
    }

    [Fact]
    public async Task Update_returns_no_content_and_persists_details_preserving_stock()
    {
        var created = await CreateAsync();
        using var response = await _client.PutAsJsonAsync($"{Products}/{created.Id}", new ProductUpdateRequest("Updated", null, 15.50m, 2));
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        (await GetAsync(created.Id)).ShouldBe(new ProductResponse(created.Id, "Updated", null, 15.50m, 10, "Clothing"));
    }

    [Fact]
    public async Task Delete_returns_no_content_and_product_is_no_longer_found()
    {
        var created = await CreateAsync();
        using var response = await _client.DeleteAsync($"{Products}/{created.Id}");
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        using var read = await _client.GetAsync($"{Products}/{created.Id}");
        await AssertProblemAsync(read, HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("GET", "")]
    [InlineData("PUT", "")]
    [InlineData("DELETE", "")]
    [InlineData("POST", "/add-to-stock/1")]
    [InlineData("POST", "/decrement-stock/1")]
    public async Task Missing_product_returns_404_problem(string method, string suffix)
    {
        using var request = new HttpRequestMessage(new HttpMethod(method), $"{Products}/999999{suffix}");
        if (method == "PUT") request.Content = JsonContent.Create(new ProductUpdateRequest("Missing", null, 1m, 1));
        using var response = await _client.SendAsync(request);
        var problem = await AssertProblemAsync(response, HttpStatusCode.NotFound);
        problem.Title.ShouldBe("Product not found");
        problem.Detail.ShouldNotBeNull().ShouldContain("999999");
    }

    [Theory]
    [InlineData("add-to-stock", 5, 15)]
    [InlineData("decrement-stock", 10, 0)]
    public async Task Stock_endpoints_persist_quantity(string operation, int quantity, int expected)
    {
        var created = await CreateAsync();
        using var response = await _client.PostAsync($"{Products}/{created.Id}/{operation}/{quantity}", null);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await GetAsync(created.Id)).Stock.ShouldBe(expected);
    }

    [Theory]
    [InlineData("add-to-stock", 0)]
    [InlineData("add-to-stock", -1)]
    [InlineData("decrement-stock", 0)]
    [InlineData("decrement-stock", -1)]
    [InlineData("decrement-stock", 11)]
    public async Task Invalid_stock_change_returns_400_without_changing_stock(string operation, int quantity)
    {
        var created = await CreateAsync();
        using var response = await _client.PostAsync($"{Products}/{created.Id}/{operation}/{quantity}", null);
        await AssertProblemAsync(response, HttpStatusCode.BadRequest);
        (await GetAsync(created.Id)).Stock.ShouldBe(10);
    }

    [Fact]
    public async Task Concurrent_stock_requests_do_not_lose_updates()
    {
        var created = await CreateAsync();
        await Task.WhenAll(Enumerable.Range(0, 8).Select(async _ =>
        {
            using var response = await _client.PostAsync($"{Products}/{created.Id}/add-to-stock/1", null);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }));
        (await GetAsync(created.Id)).Stock.ShouldBe(18);
    }

    [Fact]
    public async Task Duplicate_product_returns_409_and_does_not_insert()
    {
        await CreateAsync();
        using var response = await _client.PostAsJsonAsync(Products, new ProductAddRequest("Keyboard", null, 1m, 0, 1));
        (await AssertProblemAsync(response, HttpStatusCode.Conflict)).Title.ShouldBe("Duplicate resource");
        var page = await _client.GetFromJsonAsync<PagedList<ProductResponse>>($"{Products}?PageNumber=1&PageSize=10");
        page!.TotalCount.ShouldBe(1);
    }

    [Theory]
    [InlineData("POST")]
    [InlineData("PUT")]
    public async Task Unknown_category_returns_409(string method)
    {
        var created = await CreateAsync();
        using var response = method == "POST"
            ? await _client.PostAsJsonAsync(Products, new ProductAddRequest("Orphan", null, 1m, 0, 999))
            : await _client.PutAsJsonAsync($"{Products}/{created.Id}", new ProductUpdateRequest("Orphan", null, 1m, 999));
        (await AssertProblemAsync(response, HttpStatusCode.Conflict)).Title.ShouldBe("Related resource missing");
        (await GetAsync(created.Id)).Category.ShouldBe("Electronics");
    }

    [Theory]
    [InlineData("Name")]
    [InlineData("Description")]
    [InlineData("Price")]
    [InlineData("Stock")]
    [InlineData("CategoryId")]
    public async Task Invalid_create_returns_validation_problem_and_does_not_insert(string field)
    {
        var request = new ProductAddRequest(
            field == "Name" ? "" : "Valid", field == "Description" ? new string('x', 257) : null,
            field == "Price" ? 0m : 1m, field == "Stock" ? -1 : 0, field == "CategoryId" ? 0 : 1);
        using var response = await _client.PostAsJsonAsync(Products, request);
        await AssertValidationAsync(response, field);
        (await _client.GetFromJsonAsync<PagedList<ProductResponse>>($"{Products}?PageNumber=1&PageSize=10"))!.TotalCount.ShouldBe(0);
    }

    [Theory]
    [InlineData("Name")]
    [InlineData("Description")]
    [InlineData("Price")]
    [InlineData("CategoryId")]
    public async Task Invalid_update_returns_validation_problem_without_changing_product(string field)
    {
        var created = await CreateAsync();
        var before = await GetAsync(created.Id);
        using var response = await _client.PutAsJsonAsync($"{Products}/{created.Id}", new ProductUpdateRequest(
            field == "Name" ? "" : "Updated", field == "Description" ? new string('x', 257) : null,
            field == "Price" ? -1m : 1m, field == "CategoryId" ? 0 : 1));
        await AssertValidationAsync(response, field);
        (await GetAsync(created.Id)).ShouldBe(before);
    }

    [Theory]
    [InlineData("?PageNumber=0", "PageNumber")]
    [InlineData("?PageSize=0", "PageSize")]
    [InlineData("?PageSize=51", "PageSize")]
    [InlineData("/search?Name=", "Name")]
    [InlineData("/search", "Name")]
    [InlineData("/search?Name=key&PageSize=51", "PageSize")]
    [InlineData("/stock-level?MinStock=-1&MaxStock=10", "MinStock")]
    [InlineData("/stock-level?MinStock=0&MaxStock=-1", "MaxStock")]
    [InlineData("/stock-level?MinStock=10&MaxStock=5", "")]
    [InlineData("/stock-level?MaxStock=10", "MinStock")]
    [InlineData("/stock-level?MinStock=0", "MaxStock")]
    public async Task Invalid_query_returns_validation_problem(string query, string field)
    {
        using var response = await _client.GetAsync(Products + query);
        await AssertValidationAsync(response, field);
    }

    [Theory]
    [InlineData("{invalid", "application/json", 400)]
    [InlineData("", "application/json", 400)]
    [InlineData("plain text", "text/plain", 415)]
    public async Task Invalid_body_returns_client_error(string body, string contentType, int expected)
    {
        using var content = new StringContent(body, Encoding.UTF8, contentType);
        using var response = await _client.PostAsync(Products, content);
        response.StatusCode.ShouldBe((HttpStatusCode)expected);
    }

    [Theory]
    [InlineData("POST", "79228162514264337593543950335")]
    [InlineData("PUT", "10000000000000000")]
    [InlineData("POST", "1.001")]
    [InlineData("PUT", "0.001")]
    public async Task Invalid_price_returns_400_without_changing_data(string method, string value)
    {
        var price = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
        var created = await CreateAsync();
        var before = await GetAsync(created.Id);
        using var response = method == "POST"
            ? await _client.PostAsJsonAsync(Products, new ProductAddRequest("Invalid price", null, price, 0, 1))
            : await _client.PutAsJsonAsync($"{Products}/{created.Id}", new ProductUpdateRequest("Invalid price", null, price, 1));
        await AssertValidationAsync(response, "Price");
        (await GetAsync(created.Id)).ShouldBe(before);
        (await _client.GetFromJsonAsync<PagedList<ProductResponse>>(Products))!.TotalCount.ShouldBe(1);
    }

    [Fact]
    public async Task Unsupported_api_version_returns_404()
    {
        using var response = await _client.GetAsync("/api/v2/products");
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
