namespace ProductsService.Business.DTO;

public record ProductAddRequest(string Name, string? Description, decimal Price, int Stock, int CategoryId);

public record ProductUpdateRequest(string Name, string? Description, decimal Price, int CategoryId);
