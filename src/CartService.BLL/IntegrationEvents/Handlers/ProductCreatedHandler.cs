using CartService.BLL.Interfaces.IntegrationEvents;
using CartService.DAL.Entities;
using CartService.DAL.Interfaces;

namespace CartService.BLL.IntegrationEvents.Handlers;

public class ProductCreatedHandler(IProductRepository productRepository) : IProductCreatedHandler
{
    public async Task HandleAsync(ProductCreatedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Id = @event.ProductId,
            Name = @event.Name,
            Price = @event.Price,
            Amount = @event.Amount
        };

        await productRepository.AddAsync(product, cancellationToken);
    }
}
