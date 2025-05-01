using CartService.DAL.Entities;

namespace CartService.BLL.Interfaces;

public interface ICartService
{
    Task<IReadOnlyList<CartItem>> GetItemsAsync(int cartId);
    Task AddItemAsync(int cartId, CartItem item);
    Task RemoveItemAsync(int cartId, int itemId);
}
