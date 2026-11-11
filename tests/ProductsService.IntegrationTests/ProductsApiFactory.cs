using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductsService.Data;
using Testcontainers.PostgreSql;

namespace ProductsService.IntegrationTests;

public sealed class ProductsApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:17")
        .WithDatabase("products_api_tests")
        .WithUsername("products")
        .WithPassword("integration_tests_password")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration((_, configuration) =>
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Postgres"] = _postgres.GetConnectionString(),
                ["Database:MigrateOnStartup"] = "true",
                ["Serilog:MinimumLevel:Default"] = "Fatal"
            }));
    }

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();
        _ = Services;
    }

    public async Task ResetAsync()
    {
        await using var scope = Services.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database
            .ExecuteSqlRawAsync("TRUNCATE TABLE products RESTART IDENTITY");
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        try
        {
            await DisposeAsync();
        }
        finally
        {
            await _postgres.DisposeAsync();
        }
    }
}
