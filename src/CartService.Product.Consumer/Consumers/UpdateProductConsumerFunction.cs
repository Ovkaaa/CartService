using Azure.Messaging.ServiceBus;
using CartService.BLL.IntegrationEvents;
using CartService.BLL.Interfaces.IntegrationEvents;
using CartService.Product.Consumer.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CartService.Product.Consumer.Consumers;

public class UpdateProductConsumerFunction(
    IProductUpdatedHandler handler,
    ILogger<UpdateProductConsumerFunction> logger)
{
    [Function(nameof(UpdateProductConsumerFunction))]
    public async Task Run(
        [ServiceBusTrigger("%ProductUpdatedQueue%", Connection = "ServiceBus:ConnectionString")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(messageActions);

        logger.LogProcessingMessage(message.MessageId);

        var productUpdatedEvent = message.Body.ToObjectFromJson<ProductUpdatedIntegrationEvent>();

        if (productUpdatedEvent is not null)
        {
            await handler.HandleAsync(productUpdatedEvent, CancellationToken.None);
        }

        await messageActions.CompleteMessageAsync(message);
    }
}
