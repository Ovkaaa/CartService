using CartService.BLL.Interfaces;
using CartService.DAL.Entities;
using CartService.DAL.Interfaces;

namespace CartService.BLL.Services;

public class CartService(ICartRepository repository, IProductCatalogClient productCatalogClient) : ICartService
{
    private readonly ICartRepository _repository = repository;
    private readonly IProductCatalogClient _productCatalogClient = productCatalogClient;

    public async Task<IReadOnlyList<CartItem>> GetItemsAsync(int cartId)
    {
        var cart = await _repository.GetByIdAsync(cartId);
        return cart?.Items ?? [];
    }

    public async Task AddItemAsync(int cartId, CartItem item)
    {
        var exists = await _productCatalogClient.ProductExistsAsync(item.Id);
        if (!exists)
            throw new InvalidOperationException($"Product with Id {item.Id} does not exist in catalog.");

        var cart = await _repository.GetByIdAsync(cartId) ?? new Cart { Id = cartId };

        var existing = cart.Items.FirstOrDefault(i => i.Id == item.Id);
        if (existing != null)
            existing.Quantity = item.Quantity;
        else
            cart.Items.Add(item);

        await _repository.SaveAsync(cart);
    }

    public async Task RemoveItemAsync(int cartId, int itemId)
    {
        var cart = await _repository.GetByIdAsync(cartId);
        if (cart == null) return;

        var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            cart.Items.Remove(item);
            await _repository.SaveAsync(cart);
        }
    }
}
