namespace ProductsService.Business.DTO;

public record ProductResponse(int Id, string Name, string? Description, decimal Price, int Stock, string Category);