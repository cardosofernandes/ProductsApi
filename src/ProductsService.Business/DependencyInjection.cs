using Microsoft.Extensions.DependencyInjection;
using ProductsService.Business.Interfaces;
using ProductsService.Business.Services;
using ProductsService.Business.Validators;

namespace ProductsService.Business;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddBusiness()
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(DependencyInjection).Assembly);
            services.AddMapster();

            services.AddScoped<IProductService, ProductService>();
            services.AddValidatorsFromAssemblyContaining<PagedRequestValidator>();
            return services;
        }
    }
    
}