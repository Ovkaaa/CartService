using CartService.BLL.Interfaces;
using CartService.DAL.Entities;
using CartService.DAL.Interfaces;

namespace CartService.BLL.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public Task AddAsync(Product product, CancellationToken cancellationToken) => productRepository.AddAsync(product, cancellationToken);
    public Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken) => productRepository.GetByIdAsync(productId, cancellationToken);
    public Task UpdateAsync(Product product, CancellationToken cancellationToken) => productRepository.UpdateAsync(product, cancellationToken);
}
