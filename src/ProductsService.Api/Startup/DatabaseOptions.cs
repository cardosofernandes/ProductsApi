namespace ProductsService.Api.Startup;

public sealed class DatabaseOptions
{
    public const string SectionName = "Database";

    public bool MigrateOnStartup { get; init; } = false;
}
