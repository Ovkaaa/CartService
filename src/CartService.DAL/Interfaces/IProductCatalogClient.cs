namespace CartService.DAL.Interfaces;

public interface IProductCatalogClient
{
    Task<bool> IsProductExistsAsync(int productId, CancellationToken cancellationToken);
}
