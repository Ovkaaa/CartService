using CartService.DAL.Entities;

namespace CartService.DAL.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetCartByIdAsync(int cartId, CancellationToken cancellationToken);
    Task SaveAsync(Cart cart, CancellationToken cancellationToken);
}