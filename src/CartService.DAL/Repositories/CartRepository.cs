﻿using CartService.DAL.Entities;
using CartService.DAL.Interfaces;
using LiteDB;

namespace CartService.DAL.Repositories;

public class CartRepository(ILiteDatabase liteDb) : ICartRepository
{
    private const string CartCollectionName = "carts";

    public Task<Cart?> GetCartByIdAsync(int cartId, CancellationToken cancellationToken)
    {
        var carts = liteDb.GetCollection<Cart?>(CartCollectionName);
        return Task.FromResult(carts.FindById(cartId));
    }

    public Task SaveAsync(Cart cart, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(cart);
        var carts = liteDb.GetCollection<Cart>(CartCollectionName);
        carts.Upsert(cart.Id, cart);
        return Task.CompletedTask;
    }
}
