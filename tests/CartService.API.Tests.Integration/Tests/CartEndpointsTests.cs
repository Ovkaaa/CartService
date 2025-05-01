using System.Net;
using System.Net.Http.Json;
using CartService.API.Tests.Integration.Factories;
using CartService.DAL.Entities;

namespace CartService.API.Tests.Integration.Tests;

public class CartEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CartEndpointsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

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

        // Act
        var response = await _client.PostAsJsonAsync($"api/cart/{cartId}/items", item);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var items = await _client.GetFromJsonAsync<List<CartItem>>($"api/cart/{cartId}/items");
        Assert.NotNull(items);
        Assert.Contains(items, i => i.Id == item.Id && i.Quantity == item.Quantity);
    }

    [Fact]
    public async Task GetItems_ShouldReturnEmpty_WhenCartIsNew()
    {
        // Arrange
        var cartId = 2;

        // Act
        var items = await _client.GetFromJsonAsync<List<CartItem>>($"api/cart/{cartId}/items");

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
        await _client.PostAsJsonAsync($"api/cart/{cartId}/items", item);

        // Act - update with new quantity
        await _client.PostAsJsonAsync($"api/cart/{cartId}/items", updatedItem);

        var items = await _client.GetFromJsonAsync<List<CartItem>>($"api/cart/{cartId}/items");

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

        await _client.PostAsJsonAsync($"api/cart/{cartId}/items", item);

        // Act
        var deleteResponse = await _client.DeleteAsync($"api/cart/{cartId}/items/{item.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

        var items = await _client.GetFromJsonAsync<List<CartItem>>($"api/cart/{cartId}/items");
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
        var deleteResponse = await _client.DeleteAsync($"api/cart/{cartId}/items/{nonExistingItemId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

        var items = await _client.GetFromJsonAsync<List<CartItem>>($"api/cart/{cartId}/items");
        Assert.NotNull(items);
        Assert.Empty(items);
    }
}
