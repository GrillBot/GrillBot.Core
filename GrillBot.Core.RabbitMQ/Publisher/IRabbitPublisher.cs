using GrillBot.Core.RabbitMQ.Models;

namespace GrillBot.Core.RabbitMQ.Publisher;

public interface IRabbitPublisher
{
    Task PublishAsync<TPayload>(RabbitMessage<TPayload> message);
    Task PublishAsync<TPayload>(IEnumerable<RabbitMessage<TPayload>> messages);
}
