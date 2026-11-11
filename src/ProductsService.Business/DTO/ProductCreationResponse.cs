namespace ProductsService.Business.DTO;

public record ProductCreationResponse(int Id, string Name, string? Description, decimal Price, int Stock, int CategoryId);