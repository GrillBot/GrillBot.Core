using GrillBot.Core.Managers.Performance;
using GrillBot.Core.RabbitMQ.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GrillBot.Core.RabbitMQ.Consumer;

public class RabbitConsumerService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ICounterManager _counterManager;
    private readonly Dictionary<string, AsyncDefaultBasicConsumer> _consumers = new();

    public RabbitConsumerService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _counterManager = serviceProvider.GetRequiredService<ICounterManager>();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        IEnumerable<IRabbitHandler> handlers;
        using (var scope = _serviceProvider.CreateScope())
            handlers = scope.ServiceProvider.GetServices<IRabbitHandler>();

        foreach (var handler in handlers)
            InitializeConsumer(handler.QueueName);
        return Task.CompletedTask;
    }

    private void InitializeConsumer(string queueName)
    {
        var connection = _serviceProvider.GetRequiredService<IConnection>();
        var queue = connection.InitializeQueueModel(queueName, true, false, false);

        var consumer = new AsyncEventingBasicConsumer(queue);
        consumer.Received += (_, @event) => HandleQueueMessageAsync(@event, queueName, queue);
        queue.BasicConsume(queueName, false, consumer);

        _consumers.Add(queueName, consumer);
    }

    private async Task HandleQueueMessageAsync(BasicDeliverEventArgs @event, string queueName, IModel queue)
    {
        using (_counterManager.Create($"RabbitMQ.{queueName}.Consumer"))
        {
            var logger = _loggerFactory.CreateLogger($"RabbitMQ/{queueName}");

            using var scope = _serviceProvider.CreateScope();

            var handler = scope.ServiceProvider.GetServices<IRabbitHandler>().First(o => o.QueueName == queueName);
            var body = Encoding.UTF8.GetString(@event.Body.ToArray());
            var headers = @event.BasicProperties.Headers.ToDictionary(o => o.Key, o => o.ToString());

            logger.LogInformation("Received message. Length: {Length}. Handler: {Name}. Headers: {Count}", body.Length, handler.GetType().Name, headers.Count);

            try
            {
                var payload = JsonSerializer.Deserialize(body, handler.PayloadType, RabbitMQSettings._serializerOptions);

                await handler.HandleEventAsync(payload, headers);
                queue.BasicAck(@event.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                if (ex is JsonException)
                {
                    logger.LogWarning("Unable to deserialize received JSON to type {Name}", handler.PayloadType.Name);

                    await handler.HandleUnknownEventAsync(body, headers);
                    queue.BasicAck(@event.DeliveryTag, false);
                    return;
                }

                logger.LogError(ex, "An error occured while processing received message.");
                queue.BasicNack(@event.DeliveryTag, false, true);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var consumer in _consumers.Where(o => o.Value.Model.IsOpen))
            consumer.Value.Model.Close();

        return Task.CompletedTask;
    }
}
