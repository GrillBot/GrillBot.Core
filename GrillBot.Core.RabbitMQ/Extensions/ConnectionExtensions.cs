using RabbitMQ.Client;

namespace GrillBot.Core.RabbitMQ.Extensions;

internal static class ConnectionExtensions
{
    internal static IModel InitializeQueueModel(this IConnection connection, string queueName, bool durable, bool exclusive, bool autoDelete)
    {
        var model = connection.CreateModel();
        model.QueueDeclare(queueName, durable, exclusive, autoDelete);

        return model;
    }
}
