using CartService.DAL.Entities;

namespace CartService.BLL.Interfaces;

public interface ICartItemService
{
    Task<IReadOnlyList<CartItem>> GetCartItemsAsync(int cartId, CancellationToken cancellationToken);
    Task AddCartItemAsync(int cartId, CartItem item, CancellationToken cancellationToken);
    Task RemoveCartItemAsync(int cartId, int itemId, CancellationToken cancellationToken);
}
