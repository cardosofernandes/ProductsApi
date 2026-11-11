using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProductsService.Data.Configurations;

public static class SeedDataExtensions
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);
    
    public static void SeedFromJsonFile<T>(this EntityTypeBuilder<T> modelBuilder, string folder = "SeedData") 
        where T : class
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, folder, $"{typeof(T).Name}.json");
        if (!File.Exists(filePath))
            throw new FileNotFoundException("The seed file was not found.", filePath);
        
        var productsJson = File.ReadAllText(filePath);
        var products = JsonSerializer.Deserialize<T[]>(productsJson, Options);
        modelBuilder.HasData(products ?? []);
    }
}