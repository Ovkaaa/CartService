using System.Net;
using System.Net.Http.Json;
using CartService.BLL.Interfaces;
using CartService.DAL.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CartService.API.Tests.Endpoints;

public class CartItemsEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Mock<ICartService> _cartServiceMock = new();

    private readonly HttpClient _client;

    public CartItemsEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddScoped(_ => _cartServiceMock.Object);
            });
        }).CreateClient();
    }

    [Fact]
    public async Task GetCartItems_ReturnsOk()
    {
        // Arrange
        _cartServiceMock
            .Setup(m => m.GetItemsAsync(1))
            .ReturnsAsync([]);

        // Act
        var response = await _client.GetAsync("/api/cart/1/items");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var items = await response.Content.ReadFromJsonAsync<List<CartItem>>();
        Assert.NotNull(items);
    }

    [Fact]
    public async Task AddCartItem_ReturnsOk()
    {
        // Arrange
        var newItem = new CartItem { Id = 2, Name = "NewItem", Price = 20, Quantity = 2 };

        // Act
        var response = await _client.PostAsJsonAsync("/api/cart/1/items", newItem);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        _cartServiceMock.Verify(m =>
            m.AddItemAsync(1, It.Is<CartItem>(item => item.Id == newItem.Id
                                                      && item.Name == newItem.Name
                                                      && item.Price == newItem.Price
                                                      && item.Quantity == newItem.Quantity)),
            Times.Once);
    }

    [Fact]
    public async Task DeleteCartItem_ReturnsOk()
    {
        // Act
        var response = await _client.DeleteAsync("/api/cart/1/items/2");
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        _cartServiceMock.Verify(m => m.RemoveItemAsync(1, 2), Times.Once);
    }
}

