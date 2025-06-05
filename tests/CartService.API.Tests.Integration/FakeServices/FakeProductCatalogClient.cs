using CartService.DAL.Interfaces;

namespace CartService.API.Tests.Integration.FakeServices;

public class FakeProductCatalogClient : IProductCatalogClient
{
    public Task<bool> IsProductExistsAsync(int productId, CancellationToken _)
    {
        return Task.FromResult(true);
    }
}