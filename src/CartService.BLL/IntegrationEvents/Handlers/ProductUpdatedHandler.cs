using CartService.BLL.Interfaces.IntegrationEvents;
using CartService.DAL.Entities;
using CartService.DAL.Interfaces;

namespace CartService.BLL.IntegrationEvents.Handlers;

public class ProductUpdatedHandler(IProductRepository productRepository) : IProductUpdatedHandler
{
    public async Task HandleAsync(ProductUpdatedIntegrationEvent productUpdatedEvent, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(productUpdatedEvent);
        var product = await productRepository.GetByIdAsync(productUpdatedEvent.ProductId, cancellationToken);

        if (product is null)
        {
            product = new Product
            {
                Id = productUpdatedEvent.ProductId,
                Name = productUpdatedEvent.Name,
                Price = productUpdatedEvent.Price,
                Amount = productUpdatedEvent.Amount
            };
            await productRepository.AddAsync(product, cancellationToken);
        }

        product.Name = productUpdatedEvent.Name;
        product.Price = productUpdatedEvent.Price;
        product.Amount = productUpdatedEvent.Amount;

        await productRepository.UpdateAsync(product, cancellationToken);
    }
}
