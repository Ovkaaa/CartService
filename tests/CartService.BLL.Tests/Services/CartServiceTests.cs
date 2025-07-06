using CartService.BLL.Interfaces;
using CartService.DAL.Entities;
using CartService.DAL.Interfaces;
using Moq;

namespace CartService.BLL.Tests.Services;

public class CartServiceTests
{
    private readonly Mock<ICartRepository> _cartRepositoryMock = new();
    private readonly Mock<IProductService> _productServiceMock = new();

    private readonly BLL.Services.CartItemService _cartService;

    public CartServiceTests()
    {
        _cartService = new BLL.Services.CartItemService(
            _cartRepositoryMock.Object,
            _productServiceMock.Object);
    }

    [Fact]
    public async Task GetItemsAsync_ReturnsItems_WhenCartExists()
    {
        // Arrange
        var cartId = 1;
        var cart = new Cart { Id = cartId, Items = [new CartItem { Id = 1, Name = "Test", Price = 10, Quantity = 1 }] };
        _cartRepositoryMock.Setup(r => r.GetCartByIdAsync(cartId, It.IsAny<CancellationToken>())).ReturnsAsync(cart);

        // Act
        var result = await _cartService.GetCartItemsAsync(cartId, CancellationToken.None);

        // Assert
        Assert.Single(result);
        Assert.Equal(1, result[0].Id);
    }

    [Fact]
    public async Task GetItemsAsync_ReturnsEmptyList_WhenCartDoesNotExist()
    {
        // Arrange
        _cartRepositoryMock.Setup(r => r.GetCartByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Cart)null!);

        // Act
        var result = await _cartService.GetCartItemsAsync(0, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task AddItemAsync_AddsNewItem_WhenItemNotInCart()
    {
        // Arrange
        var cartId = 1;
        var newItem = new CartItem { Id = 1, Name = "Product", Price = 5, Quantity = 2 };

        _productServiceMock.Setup(p => p.GetByIdAsync(newItem.Id, It.IsAny<CancellationToken>())).ReturnsAsync(new Product());
        _cartRepositoryMock.Setup(r => r.GetCartByIdAsync(cartId, It.IsAny<CancellationToken>())).ReturnsAsync((Cart)null!);

        Cart? savedCart = null;
        _cartRepositoryMock.Setup(r => r.SaveAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
                           .Callback<Cart, CancellationToken>((cart, _) => savedCart = cart)
                           .Returns(Task.CompletedTask);

        // Act
        await _cartService.AddCartItemAsync(cartId, newItem, CancellationToken.None);

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

        _productServiceMock.Setup(p => p.GetByIdAsync(item.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Product?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _cartService.AddCartItemAsync(1, item, CancellationToken.None));
    }

    [Fact]
    public async Task AddItemAsync_UpdatesQuantity_IfItemAlreadyExists()
    {
        // Arrange
        var cartId = 1;
        var existingItem = new CartItem { Id = 1, Quantity = 1 };
        var updatedItem = new CartItem { Id = 1, Quantity = 5 };

        var cart = new Cart { Id = cartId, Items = [existingItem] };

        _productServiceMock.Setup(p => p.GetByIdAsync(updatedItem.Id, It.IsAny<CancellationToken>())).ReturnsAsync(new Product());
        _cartRepositoryMock.Setup(r => r.GetCartByIdAsync(cartId, It.IsAny<CancellationToken>())).ReturnsAsync(cart);

        // Act
        await _cartService.AddCartItemAsync(cartId, updatedItem, CancellationToken.None);

        // Assert
        Assert.Equal(5, cart.Items.First(i => i.Id == 1).Quantity);
        _cartRepositoryMock.Verify(r => r.SaveAsync(cart, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RemoveItemAsync_RemovesItem_WhenExists()
    {
        // Arrange
        var cartId = 1;
        var cart = new Cart { Id = cartId, Items = [new CartItem { Id = 1 }] };

        _cartRepositoryMock.Setup(r => r.GetCartByIdAsync(cartId, It.IsAny<CancellationToken>())).ReturnsAsync(cart);

        // Act
        await _cartService.RemoveCartItemAsync(cartId, 1, CancellationToken.None);

        // Assert
        Assert.Empty(cart.Items);
        _cartRepositoryMock.Verify(r => r.SaveAsync(cart, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RemoveItemAsync_DoesNothing_WhenCartIsNull()
    {
        _cartRepositoryMock.Setup(r => r.GetCartByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Cart)null!);

        // Act
        await _cartService.RemoveCartItemAsync(0, 1, CancellationToken.None);

        // Assert
        _cartRepositoryMock.Verify(r => r.SaveAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task RemoveItemAsync_DoesNothing_WhenItemNotFound()
    {
        // Arrange
        var cartId = 1;
        var cart = new Cart { Id = cartId, Items = [new CartItem { Id = 2 }] };

        _cartRepositoryMock.Setup(r => r.GetCartByIdAsync(cartId, It.IsAny<CancellationToken>())).ReturnsAsync(cart);

        // Act
        await _cartService.RemoveCartItemAsync(cartId, 1, CancellationToken.None);

        // Assert
        Assert.Single(cart.Items);
        _cartRepositoryMock.Verify(r => r.SaveAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
