using CartService.BLL.IntegrationEvents;

namespace CartService.BLL.Interfaces.IntegrationEvents;

public interface IProductCreatedHandler
{
    Task HandleAsync(ProductCreatedIntegrationEvent @event, CancellationToken cancellationToken);
}
