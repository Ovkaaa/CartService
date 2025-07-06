using Azure.Messaging.ServiceBus;
using CartService.BLL.IntegrationEvents;
using CartService.BLL.Interfaces.IntegrationEvents;
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
        logger.LogInformation("Message ID: {id}", message.MessageId);
        logger.LogInformation("Message Body: {body}", message.Body);
        logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        var @event = message.Body.ToObjectFromJson<ProductUpdatedIntegrationEvent>();

        if (@event is not null)
        {
            await handler.HandleAsync(@event, CancellationToken.None);
        }

        await messageActions.CompleteMessageAsync(message);
    }
}
