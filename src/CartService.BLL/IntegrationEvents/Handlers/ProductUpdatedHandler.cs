using CartService.BLL.Interfaces.IntegrationEvents;
using CartService.DAL.Entities;
using CartService.DAL.Interfaces;

namespace CartService.BLL.IntegrationEvents.Handlers;

public class ProductUpdatedHandler(IProductRepository productRepository) : IProductUpdatedHandler
{
    public async Task HandleAsync(ProductUpdatedIntegrationEvent @event, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(@event.ProductId, cancellationToken);

        if (product is null)
        {
            product = new Product
            {
                Id = @event.ProductId,
                Name = @event.Name,
                Price = @event.Price,
                Amount = @event.Amount
            };
            await productRepository.AddAsync(product, cancellationToken);
        }

        product.Name = @event.Name;
        product.Price = @event.Price;
        product.Amount = @event.Amount;

        await productRepository.UpdateAsync(product, cancellationToken);
    }
}
