namespace ProductsService.Business.Mapping;

public sealed class ProductMappingConfig : IRegister
{
    public  void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, ProductResponse>()
            .Map(dest => dest.Category, src => src.Category.Name);
        
        config.NewConfig<ProductAddRequest, Product>();
        config.NewConfig<ProductUpdateRequest, Product>()
            .Ignore(dest => dest.Stock);

        config.NewConfig<Product, ProductCreationResponse>();
        config.NewConfig<PagedList<Product>, PagedList<ProductResponse>>();
    }
}
