namespace CartService.BLL.IntegrationEvents;

public record ProductUpdatedIntegrationEvent(int ProductId, string Name, decimal Price, int Amount);
