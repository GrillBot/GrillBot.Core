using GrillBot.Core.Managers.Performance;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace GrillBot.Core.RabbitMQ.Publisher;

public class RabbitMQPublisher : IRabbitMQPublisher
{
    private readonly IConnection _connection;
    private readonly ICounterManager _counterManager;

    public RabbitMQPublisher(IConnection connection, ICounterManager counterManager)
    {
        _connection = connection;
        _counterManager = counterManager;
    }

    public Task PublishAsync<TModel>(string queueName, TModel model) where TModel : IPayload
        => PublishAsync(queueName, model, new Dictionary<string, string>());

    public async Task PublishAsync<TModel>(string queueName, TModel model, Dictionary<string, string> headers)
    {
        using (_counterManager.Create($"RabbitMQ.{queueName}.Producer"))
            await SendWithRetryPolicyAsync(queueName, model, headers, 5);
    }

    public Task PublishAsync<TModel>(TModel model) where TModel : IPayload
        => PublishAsync(model.QueueName, model, new Dictionary<string, string>());

    public Task PublishAsync<TModel>(TModel model, Dictionary<string, string> headers) where TModel : IPayload
        => PublishAsync(model.QueueName, model, headers);

    public Task PublishBatchAsync<TModel>(IEnumerable<TModel> models) where TModel : IPayload
        => PublishBatchAsync(models, new Dictionary<string, string>());

    public async Task PublishBatchAsync<TModel>(IEnumerable<TModel> models, Dictionary<string, string> headers) where TModel : IPayload
    {
        foreach (var group in models.GroupBy(o => o.QueueName))
        {
            foreach (var message in group)
                await PublishAsync(message, headers);
        }
    }

    private async Task SendWithRetryPolicyAsync<TModel>(string queueName, TModel model, Dictionary<string, string> headers, int maxRetry, int retryCount = 0)
    {
        try
        {
            using var queue = CreateQueueModel(queueName);

            var message = SerializeModel(model);
            var props = queue.CreateBasicProperties();
            props.Persistent = true;
            props.Headers = headers.ToDictionary(o => o.Key, o => (object)o.Value);

            queue.BasicPublish("", queueName, true, props, message);
        }
        catch (global::RabbitMQ.Client.Exceptions.AlreadyClosedException) when (retryCount < maxRetry)
        {
            await Task.Delay(1000);
            await SendWithRetryPolicyAsync(queueName, model, headers, maxRetry, retryCount + 1);
        }
    }

    private IModel CreateQueueModel(string queueName)
    {
        var model = _connection.CreateModel();
        model.QueueDeclare(queueName, true, false, false);

        return model;
    }

    private static byte[] SerializeModel<TModel>(TModel model)
        => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(model, RabbitMQSettings._serializerOptions));
}
