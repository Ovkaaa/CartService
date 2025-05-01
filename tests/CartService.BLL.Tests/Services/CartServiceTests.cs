using CartService.DAL.Entities;
using CartService.DAL.Interfaces;
using Moq;

namespace CartService.BLL.Tests.Services;

public class CartServiceTests
{
    private readonly Mock<ICartRepository> _cartRepositoryMock = new();
    private readonly Mock<IProductCatalogClient> _productCatalogClientMock = new();

    private BLL.Services.CartService _cartService;

    public CartServiceTests()
    {
        _cartService = new BLL.Services.CartService(
            _cartRepositoryMock.Object,
            _productCatalogClientMock.Object);
    }

    [Fact]
    public async Task GetItemsAsync_ReturnsItems_WhenCartExists()
    {
        // Arrange
        var cartId = 1;
        var cart = new Cart { Id = cartId, Items = [new CartItem { Id = 1, Name = "Test", Price = 10, Quantity = 1 }] };
        _cartRepositoryMock.Setup(r => r.GetByIdAsync(cartId)).ReturnsAsync(cart);

        // Act
        var result = await _cartService.GetItemsAsync(cartId);

        // Assert
        Assert.Single(result);
        Assert.Equal(1, result[0].Id);
    }

    [Fact]
    public async Task GetItemsAsync_ReturnsEmptyList_WhenCartDoesNotExist()
    {
        // Arrange
        _cartRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Cart)null!);

        // Act
        var result = await _cartService.GetItemsAsync(0);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task AddItemAsync_AddsNewItem_WhenItemNotInCart()
    {
        // Arrange
        var cartId = 1;
        var newItem = new CartItem { Id = 1, Name = "Product", Price = 5, Quantity = 2 };

        _productCatalogClientMock.Setup(p => p.ProductExistsAsync(newItem.Id)).ReturnsAsync(true);
        _cartRepositoryMock.Setup(r => r.GetByIdAsync(cartId)).ReturnsAsync((Cart)null!);

        Cart? savedCart = null;
        _cartRepositoryMock.Setup(r => r.SaveAsync(It.IsAny<Cart>()))
                           .Callback<Cart>(c => savedCart = c)
                           .Returns(Task.CompletedTask);

        // Act
        await _cartService.AddItemAsync(cartId, newItem);

        // Assert
        Assert.NotNull(savedCart);
        Assert.Single(savedCart!.Items);
        Assert.Equal(newItem.Id, savedCart.Items[0].Id);
    }

    [Fact]
    public async Task AddItemAsync_Throws_WhenProductNotExists()
    {
        // Arrange
        var item = new CartItem { Id = 2 };

        _productCatalogClientMock.Setup(p => p.ProductExistsAsync(item.Id)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _cartService.AddItemAsync(1, item));
    }

    [Fact]
    public async Task AddItemAsync_UpdatesQuantity_IfItemAlreadyExists()
    {
        // Arrange
        var cartId = 1;
        var existingItem = new CartItem { Id = 1, Quantity = 1 };
        var updatedItem = new CartItem { Id = 1, Quantity = 5 };

        var cart = new Cart { Id = cartId, Items = [existingItem] };

        _productCatalogClientMock.Setup(p => p.ProductExistsAsync(updatedItem.Id)).ReturnsAsync(true);
        _cartRepositoryMock.Setup(r => r.GetByIdAsync(cartId)).ReturnsAsync(cart);

        // Act
        await _cartService.AddItemAsync(cartId, updatedItem);

        // Assert
        Assert.Equal(5, cart.Items.First(i => i.Id == 1).Quantity);
        _cartRepositoryMock.Verify(r => r.SaveAsync(cart), Times.Once);
    }

    [Fact]
    public async Task RemoveItemAsync_RemovesItem_WhenExists()
    {
        // Arrange
        var cartId = 1;
        var cart = new Cart { Id = cartId, Items = [new CartItem { Id = 1 }] };

        _cartRepositoryMock.Setup(r => r.GetByIdAsync(cartId)).ReturnsAsync(cart);

        // Act
        await _cartService.RemoveItemAsync(cartId, 1);

        // Assert
        Assert.Empty(cart.Items);
        _cartRepositoryMock.Verify(r => r.SaveAsync(cart), Times.Once);
    }

    [Fact]
    public async Task RemoveItemAsync_DoesNothing_WhenCartIsNull()
    {
        _cartRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Cart)null!);

        // Act
        await _cartService.RemoveItemAsync(0, 1);

        // Assert
        _cartRepositoryMock.Verify(r => r.SaveAsync(It.IsAny<Cart>()), Times.Never);
    }

    [Fact]
    public async Task RemoveItemAsync_DoesNothing_WhenItemNotFound()
    {
        // Arrange
        var cartId = 1;
        var cart = new Cart { Id = cartId, Items = [new CartItem { Id = 2 }] };

        _cartRepositoryMock.Setup(r => r.GetByIdAsync(cartId)).ReturnsAsync(cart);

        // Act
        await _cartService.RemoveItemAsync(cartId, 1);

        // Assert
        Assert.Single(cart.Items);
        _cartRepositoryMock.Verify(r => r.SaveAsync(It.IsAny<Cart>()), Times.Never);
    }
}
