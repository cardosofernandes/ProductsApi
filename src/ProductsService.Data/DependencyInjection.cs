using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductsService.Data.Interfaces;
using ProductsService.Data.Repositories;


namespace ProductsService.Data;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            // services.AddOptions<DatabaseOptions>()
            //     .Bind(configuration.GetSection(DatabaseOptions.SectionName));
            
            // services.AddDbContext<AppDbContext>(options => options
            //     .UseSqlServer(configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string is not configured.")));
            
            services.AddDbContext<AppDbContext>(options => options
                .UseNpgsql(
                    configuration.GetConnectionString("Postgres")
                    ?? throw new InvalidOperationException("Connection string 'Postgres' is not configured."),
                    npgsql => npgsql.EnableRetryOnFailure())
                .UseSnakeCaseNamingConvention());
            
            services.AddScoped<IProductRepository, ProductRepository>();
            return services;
        }
    }
    
}