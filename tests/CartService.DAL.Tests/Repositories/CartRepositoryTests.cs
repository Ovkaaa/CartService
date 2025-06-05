using CartService.DAL.Entities;
using CartService.DAL.Repositories;
using LiteDB;
using Moq;

namespace CartService.DAL.Tests.Repositories;

public class CartRepositoryTests
{
    private readonly Mock<ILiteDatabase> _liteDatabaseMock = new();

    private readonly CartRepository _cartRepository;

    public CartRepositoryTests()
    {
        _cartRepository = new CartRepository(_liteDatabaseMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_Returns_Cart_When_Exists()
    {
        // Arrange
        var expectedCart = new Cart { Id = 123 };

        var collectionMock = new Mock<ILiteCollection<Cart>>();
        collectionMock.Setup(c => c.FindById(123)).Returns(expectedCart);

        _liteDatabaseMock.Setup(db => db.GetCollection<Cart>("carts", It.IsAny<BsonAutoId>())).Returns(collectionMock.Object);

        // Act
        var result = await _cartRepository.GetCartByIdAsync(123, CancellationToken.None);

        // Assert
        Assert.Equal(expectedCart, result);
    }

    [Fact]
    public async Task SaveAsync_Upserts_Cart()
    {
        // Arrange
        var cart = new Cart { Id = 123 };

        var collectionMock = new Mock<ILiteCollection<Cart>>();
        collectionMock.Setup(c => c.Upsert(cart.Id, cart));

        _liteDatabaseMock.Setup(db => db.GetCollection<Cart>("carts", It.IsAny<BsonAutoId>())).Returns(collectionMock.Object);

        // Act
        await _cartRepository.SaveAsync(cart, CancellationToken.None);

        // Assert
        collectionMock.Verify(c => c.Upsert(cart.Id, cart), Times.Once);
    }
}
