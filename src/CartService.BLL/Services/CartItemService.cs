using CartService.BLL.Interfaces;
using CartService.DAL.Entities;
using CartService.DAL.Interfaces;

namespace CartService.BLL.Services;

public class CartItemService(ICartRepository repository, IProductCatalogClient productCatalogClient) : ICartItemService
{
    private readonly ICartRepository _repository = repository;
    private readonly IProductCatalogClient _productCatalogClient = productCatalogClient;

    public async Task<IReadOnlyList<CartItem>> GetCartItemsAsync(int cartId, CancellationToken cancellationToken)
    {
        var cart = await _repository.GetCartByIdAsync(cartId, cancellationToken);
        return cart?.Items ?? [];
    }

    public async Task AddCartItemAsync(int cartId, CartItem item, CancellationToken cancellationToken)
    {
        var exists = await _productCatalogClient.IsProductExistsAsync(item.Id, cancellationToken);
        if (!exists)
            throw new InvalidOperationException($"Product with Id {item.Id} does not exist in catalog.");

        var cart = await _repository.GetCartByIdAsync(cartId, cancellationToken) ?? new Cart { Id = cartId };

        var existing = cart.Items.FirstOrDefault(i => i.Id == item.Id);
        if (existing != null)
            existing.Quantity = item.Quantity;
        else
            cart.Items.Add(item);

        await _repository.SaveAsync(cart, cancellationToken);
    }

    public async Task RemoveCartItemAsync(int cartId, int itemId, CancellationToken cancellationToken)
    {
        var cart = await _repository.GetCartByIdAsync(cartId, cancellationToken);
        if (cart == null) return;

        var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            cart.Items.Remove(item);
            await _repository.SaveAsync(cart, cancellationToken);
        }
    }
}
