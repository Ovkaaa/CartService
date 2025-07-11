using CartService.DAL.Entities;
using CartService.DAL.Interfaces;
using LiteDB;

namespace CartService.DAL.Repositories;

public class ProductRepository(ILiteDatabase liteDb) : IProductRepository
{
    private const string ProductCollectionName = "products";

    public Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(product);
        Upsert(product);
        return Task.CompletedTask;
    }

    public Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken)
    {
        var products = liteDb.GetCollection<Product?>(ProductCollectionName);
        return Task.FromResult(products.FindById(productId));
    }

    public Task UpdateAsync(Product product, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(product);
        Upsert(product);
        return Task.CompletedTask;
    }

    private bool Upsert(Product product)
    {
        var products = liteDb.GetCollection<Product>(ProductCollectionName);
        return products.Upsert(product.Id, product);
    }
}
