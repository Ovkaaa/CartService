using CartService.DAL.Entities;

namespace CartService.BLL.Interfaces;

public interface IProductService
{
    Task AddAsync(Product product, CancellationToken cancellationToken);
    Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken);
    Task UpdateAsync(Product product, CancellationToken cancellationToken);
}
