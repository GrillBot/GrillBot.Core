using GrillBot.Core.RabbitMQ.Consumer;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace GrillBot.Core.RabbitMQ.Publisher;

public class RabbitMQPublisher : IRabbitMQPublisher
{
    private IConnection Connection { get; }

    public RabbitMQPublisher(IConnection connection)
    {
        Connection = connection;
    }

    public Task PublishAsync<TModel>(string queueName, TModel model)
    {
        var queue = CreateQueueModel(queueName);

        var json = JsonSerializer.Serialize(model, RabbitMQConsumerService._serializerOptions);
        var message = Encoding.UTF8.GetBytes(json);

        queue.BasicPublish("", queueName, true, null, message);
        return Task.CompletedTask;
    }

    private IModel CreateQueueModel(string queueName)
    {
        var model = Connection.CreateModel();
        model.QueueDeclare(queueName, true, false, false);

        return model;
    }
}
