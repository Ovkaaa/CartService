namespace CartService.BLL.IntegrationEvents;

public record ProductCreatedIntegrationEvent(int ProductId, string Name, decimal Price, int Amount);
