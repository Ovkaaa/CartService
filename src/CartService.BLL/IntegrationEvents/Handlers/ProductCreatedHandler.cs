using CartService.BLL.Interfaces.IntegrationEvents;
using CartService.DAL.Entities;
using CartService.DAL.Interfaces;

namespace CartService.BLL.IntegrationEvents.Handlers;

public class ProductCreatedHandler(IProductRepository productRepository) : IProductCreatedHandler
{
    public async Task HandleAsync(ProductCreatedIntegrationEvent productCreatedEvent, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(productCreatedEvent);
        var product = new Product
        {
            Id = productCreatedEvent.ProductId,
            Name = productCreatedEvent.Name,
            Price = productCreatedEvent.Price,
            Amount = productCreatedEvent.Amount
        };

        await productRepository.AddAsync(product, cancellationToken);
    }
}
