using System.Net.Http.Json;
using CartService.DAL.Interfaces;

namespace CartService.DAL.Clients;

public class ProductCatalogClient(HttpClient httpClient) : IProductCatalogClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<bool> IsProductExistsAsync(int productId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"/api/v1/products/{productId}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<bool>(cancellationToken);
    }
}
