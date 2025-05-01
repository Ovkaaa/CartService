using System.Net.Http.Json;
using CartService.DAL.Interfaces;

namespace CartService.DAL.Clients;

public class ProductCatalogClient(HttpClient httpClient) : IProductCatalogClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<bool> ProductExistsAsync(int productId)
    {
        var response = await _httpClient.GetAsync($"/api/catalog/products/{productId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<bool>();
    }
}
