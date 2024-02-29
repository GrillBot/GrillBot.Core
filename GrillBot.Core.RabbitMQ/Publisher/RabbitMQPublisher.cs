using GrillBot.Core.Managers.Performance;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace GrillBot.Core.RabbitMQ.Publisher;

public class RabbitMQPublisher : IRabbitMQPublisher
{
    private IConnection Connection { get; }
    private ICounterManager CounterManager { get; }

    public RabbitMQPublisher(IConnection connection, ICounterManager counterManager)
    {
        Connection = connection;
        CounterManager = counterManager;
    }

    public Task PublishAsync<TModel>(TModel model) where TModel : IPayload
        => PublishAsync(model.QueueName, model);

    public async Task PublishAsync<TModel>(string queueName, TModel model)
    {
        using (CounterManager.Create($"RabbitMQ.{queueName}.Producer"))
            await SendWithRetryPolicyAsync(queueName, model, 5);
    }

    public async Task PublishBatchAsync<TModel>(IEnumerable<TModel> models) where TModel : IPayload
    {
        foreach (var group in models.GroupBy(o => o.QueueName))
        {
            foreach (var message in group)
                await PublishAsync(message);
        }
    }

    private async Task SendWithRetryPolicyAsync<TModel>(string queueName, TModel model, int maxRetry, int retryCount = 0)
    {
        try
        {
            using var queue = CreateQueueModel(queueName);

            var message = SerializeModel(model);
            var props = queue.CreateBasicProperties();
            props.Persistent = true;

            queue.BasicPublish("", queueName, true, props, message);
        }
        catch (global::RabbitMQ.Client.Exceptions.AlreadyClosedException) when (retryCount < maxRetry)
        {
            await Task.Delay(1000);
            await SendWithRetryPolicyAsync(queueName, model, maxRetry, retryCount + 1);
        }
    }

    private IModel CreateQueueModel(string queueName)
    {
        var model = Connection.CreateModel();
        model.QueueDeclare(queueName, true, false, false);

        return model;
    }

    private static byte[] SerializeModel<TModel>(TModel model)
        => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(model, RabbitMQSettings._serializerOptions));
}
