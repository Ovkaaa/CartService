using CartService.BLL.IntegrationEvents;
using CartService.BLL.Interfaces.IntegrationEvents;
using CartService.Product.Consumer.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CartService.Product.Consumer.Consumers;

public class CreateProductConsumerFunction(
    IProductCreatedHandler handler,
    ILogger<CreateProductConsumerFunction> logger)
{
    [Function(nameof(CreateProductConsumerFunction))]
    public async Task Run(
        [RabbitMQTrigger("%RabbitMQ:CreatedProductQueue%", ConnectionStringSetting = "RabbitMQ:Connection")] string message,
        FunctionContext context)
    {
        ArgumentNullException.ThrowIfNull(message);
        logger.LogProcessingMessage(message);

        var productCreatedEvent = JsonSerializer.Deserialize<ProductCreatedIntegrationEvent>(message);

        if (productCreatedEvent is not null)
        {
            await handler.HandleAsync(productCreatedEvent, CancellationToken.None);
        }
    }
}
