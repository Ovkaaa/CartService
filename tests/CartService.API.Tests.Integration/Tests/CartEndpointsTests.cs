using CartService.API.Tests.Integration.Factories;
using CartService.DAL.Entities;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace CartService.API.Tests.Integration.Tests;

public class CartEndpointsTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task AddItem_ShouldAddToCart()
    {
        // Arrange
        var cartId = 1;
        var item = new CartItem
        {
            Id = 100,
            Name = "Test Item",
            Price = 9.99m,
            Quantity = 3
        };

        var liteDatabase = factory.Services.GetRequiredService<ILiteDatabase>();
        var product = new Product
        {
            Id = item.Id,
            Name = item.Name,
            Price = item.Price,
            Amount = 100
        };
        var products = liteDatabase.GetCollection<Product>("products");
        products.Upsert(product.Id, product);

        // Act
        var response = await _client.PostAsJsonAsync($"api/v1/carts/{cartId}/items", item);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var items = await _client.GetFromJsonAsync<List<CartItem>>($"api/v1/carts/{cartId}/items");
        Assert.NotNull(items);
        Assert.Contains(items, i => i.Id == item.Id && i.Quantity == item.Quantity);
    }

    [Fact]
    public async Task GetItems_ShouldReturnEmpty_WhenCartIsNew()
    {
        // Arrange
        var cartId = 2;

        // Act
        var items = await _client.GetFromJsonAsync<List<CartItem>>($"api/v1/carts/{cartId}/items");

        // Assert
        Assert.NotNull(items);
        Assert.Empty(items);
    }

    [Fact]
    public async Task UpdateItemQuantity_ShouldReplaceItemQuantity()
    {
        // Arrange
        var cartId = 3;
        var item = new CartItem { Id = 200, Name = "Item", Price = 5.55m, Quantity = 1 };
        var updatedItem = new CartItem { Id = 200, Name = "Item", Price = 5.55m, Quantity = 5 };

        // Add initial item
        await _client.PostAsJsonAsync($"api/v1/cart/{cartId}/items", item);

        // Act - update with new quantity
        await _client.PostAsJsonAsync($"api/v1/cart/{cartId}/items", updatedItem);

        var items = await _client.GetFromJsonAsync<List<CartItem>>($"api/v1/carts/{cartId}/items");

        // Assert
        Assert.NotNull(items);
        var found = items.Find(i => i.Id == item.Id);
        Assert.NotNull(found);
        Assert.Equal(5, found.Quantity);
    }

    [Fact]
    public async Task DeleteItem_ShouldRemoveFromCart()
    {
        // Arrange
        var cartId = 4;
        var item = new CartItem { Id = 300, Name = "DeleteMe", Price = 12.34m, Quantity = 2 };

        await _client.PostAsJsonAsync($"api/v1/carts/{cartId}/items", item);

        // Act
        var deleteResponse = await _client.DeleteAsync($"api/v1/carts/{cartId}/items/{item.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

        var items = await _client.GetFromJsonAsync<List<CartItem>>($"api/v1/carts/{cartId}/items");
        Assert.NotNull(items);
        Assert.DoesNotContain(items, i => i.Id == item.Id);
    }

    [Fact]
    public async Task DeleteNonExistingItem_ShouldNotFail()
    {
        // Arrange
        var cartId = 5;
        var nonExistingItemId = 999;

        // Act
        var deleteResponse = await _client.DeleteAsync($"api/v1/carts/{cartId}/items/{nonExistingItemId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

        var items = await _client.GetFromJsonAsync<List<CartItem>>($"api/v1/carts/{cartId}/items");
        Assert.NotNull(items);
        Assert.Empty(items);
    }
}
