using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProductsService.Data;

namespace ProductsService.Api.Startup;

public static class DatabaseInitializer
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        if (!app.Services.GetRequiredService<IOptions<DatabaseOptions>>().Value.MigrateOnStartup)
            return;

        await using var scope = app.Services.CreateAsyncScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(DatabaseInitializer));

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        logger.LogInformation("Applying database migrations");
        await db.Database.MigrateAsync();
    }
}