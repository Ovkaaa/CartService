using CartService.DAL.Entities;
using CartService.DAL.Interfaces;
using LiteDB;

namespace CartService.DAL.Repositories;

public class CartRepository(ILiteDatabase liteDb) : ICartRepository
{
    private const string CartCollectionName = "carts";
    
    private readonly ILiteDatabase _liteDb = liteDb;

    public Task<Cart?> GetByIdAsync(int cartId)
    {
        var carts = _liteDb.GetCollection<Cart?>(CartCollectionName);
        return Task.FromResult(carts.FindById(cartId));
    }

    public Task SaveAsync(Cart cart)
    {
        var carts = _liteDb.GetCollection<Cart>(CartCollectionName);
        carts.Upsert(cart.Id, cart);
        return Task.CompletedTask;
    }
}
