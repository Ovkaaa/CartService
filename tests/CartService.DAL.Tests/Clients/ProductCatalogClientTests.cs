using System.Net;
using System.Net.Http.Json;
using CartService.DAL.Clients;
using Moq;
using Moq.Protected;

namespace CartService.DAL.Tests.Clients;

public class ProductCatalogClientTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock = new();

    private readonly ProductCatalogClient _productCatalogClient;

    public ProductCatalogClientTests()
    {
        HttpClient httpClient = new(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://fake-catalog/")
        };

        _productCatalogClient = new ProductCatalogClient(httpClient);
    }

    [Fact]
    public async Task ProductExistsAsync_ReturnsTrue_WhenProductExists()
    {
        // Arrange
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(m => m.RequestUri!.ToString().Contains("/api/v1/products/1")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(true)
            });

        // Act
        var exists = await _productCatalogClient.IsProductExistsAsync(1, CancellationToken.None);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task ProductExistsAsync_ReturnsFalse_WhenProductDoesNotExist()
    {
        // Arrange
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(false)
            });

        // Act
        var exists = await _productCatalogClient.IsProductExistsAsync(999, CancellationToken.None);

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task ProductExistsAsync_ThrowsException_OnNonSuccessStatusCode()
    {
        // Arrange
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            });

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _productCatalogClient.IsProductExistsAsync(123, CancellationToken.None));
    }
}
