using CartService.BLL.IntegrationEvents;
using CartService.BLL.Interfaces.IntegrationEvents;
using CartService.Product.Consumer.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CartService.Product.Consumer.Consumers;

public class UpdateProductConsumerFunction(
    IProductUpdatedHandler handler,
    ILogger<UpdateProductConsumerFunction> logger)
{
    [Function(nameof(UpdateProductConsumerFunction))]
    public async Task Run(
        [RabbitMQTrigger("%RabbitMQ:UpdatedProductQueue%", ConnectionStringSetting = "RabbitMQ:Connection")] string message,
        FunctionContext context)
    {
        ArgumentNullException.ThrowIfNull(message);
        logger.LogProcessingMessage(message);

        var productUpdatedEvent = JsonSerializer.Deserialize<ProductUpdatedIntegrationEvent>(message);

        if (productUpdatedEvent is not null)
        {
            await handler.HandleAsync(productUpdatedEvent, CancellationToken.None);
        }
    }
}
