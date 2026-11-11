namespace ProductsService.Api.Endpoints;

public static class XEndpointRegistration
{
    public static void MapAllEndpoints(this IEndpointRouteBuilder app) 
    { 
        var productsV1 = app
            .NewVersionedApi("Products") 
            .MapGroup("api/v{version:apiVersion}/products")
            .WithTags("Products")
            .HasApiVersion(1.0);
        
        FilterProductsEndpoints.Map(productsV1);
        GetProductsEndpoint.Map(productsV1);
        GetProductByIdEndpoint.Map(productsV1);
        CreateProductEndpoint.Map(productsV1);
        UpdateProductEndpoint.Map(productsV1);
        DeleteProductEndpoint.Map(productsV1);
        IncrementEndpoint.Map(productsV1);
        DecrementEndpoint.Map(productsV1);
    }
    
}