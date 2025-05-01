using CartService.DAL.Interfaces;

namespace CartService.API.Tests.Integration.FakeServices;

public class FakeProductCatalogClient : IProductCatalogClient
{
    public Task<bool> ProductExistsAsync(int productId)
    {
        return Task.FromResult(true);
    }
}