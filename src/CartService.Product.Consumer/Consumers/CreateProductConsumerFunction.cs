using Azure.Messaging.ServiceBus;
using CartService.BLL.IntegrationEvents;
using CartService.BLL.Interfaces.IntegrationEvents;
using CartService.Product.Consumer.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CartService.Product.Consumer.Consumers;

public class CreateProductConsumerFunction(
    IProductCreatedHandler handler,
    ILogger<CreateProductConsumerFunction> logger)
{
    [Function(nameof(CreateProductConsumerFunction))]
    public async Task Run(
        [ServiceBusTrigger("%ProductCreatedQueue%", Connection = "ServiceBus:ConnectionString")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(messageActions);

        logger.LogProcessingMessage(message.MessageId);

        var productCreatedEvent = message.Body.ToObjectFromJson<ProductCreatedIntegrationEvent>();

        if (productCreatedEvent is not null)
        {
            await handler.HandleAsync(productCreatedEvent, CancellationToken.None);
        }

        await messageActions.CompleteMessageAsync(message);
    }
}
