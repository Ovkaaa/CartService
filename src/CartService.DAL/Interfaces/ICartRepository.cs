using CartService.DAL.Entities;

namespace CartService.DAL.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetByIdAsync(int cartId);
    Task SaveAsync(Cart cart);
}