using CartService.BLL.IntegrationEvents;

namespace CartService.BLL.Interfaces.IntegrationEvents;

public interface IProductUpdatedHandler
{
    Task HandleAsync(ProductUpdatedIntegrationEvent @event, CancellationToken cancellationToken);
}
