namespace CartService.DAL.Interfaces;

public interface IProductCatalogClient
{
    Task<bool> ProductExistsAsync(int productId);
}
