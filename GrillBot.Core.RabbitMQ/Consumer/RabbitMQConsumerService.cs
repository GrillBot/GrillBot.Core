using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GrillBot.Core.RabbitMQ.Consumer;

public class RabbitMQConsumerService : IHostedService
{
    private IServiceProvider ServiceProvider { get; }
    private ILoggerFactory LoggerFactory { get; }

    private readonly Dictionary<string, AsyncDefaultBasicConsumer> _consumers = new();
    public static readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public RabbitMQConsumerService(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        LoggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var handler in FindHandlers())
            InitializeConsumer(handler.QueueName);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var consumer in _consumers.Where(o => o.Value.Model.IsOpen))
            consumer.Value.Model.Close();

        return Task.CompletedTask;
    }

    private IEnumerable<IRabbitMQHandler> FindHandlers()
    {
        using var scope = ServiceProvider.CreateScope();
        return scope.ServiceProvider.GetServices<IRabbitMQHandler>();
    }

    private void InitializeConsumer(string queueName)
    {
        var connection = ServiceProvider.GetRequiredService<IConnection>();
        var queueModel = connection.CreateModel();
        queueModel.QueueDeclare(queueName, true, exclusive: false, autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(queueModel);
        consumer.Received += (_, ev) => OnQueueMessage(ev, queueName, queueModel);
        queueModel.BasicConsume(queueName, false, consumer);

        _consumers.Add(queueName, consumer);
    }

    private async Task OnQueueMessage(BasicDeliverEventArgs @event, string queueName, IModel queueModel)
    {
        var logger = LoggerFactory.CreateLogger($"RabbitMQ/{queueName}");

        using var scope = ServiceProvider.CreateScope();

        var handler = scope.ServiceProvider
            .GetServices<IRabbitMQHandler>()
            .First(o => o.QueueName == queueName);

        var body = @event.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);

        logger.LogInformation("Received new message. Length: {Length}. Handler: {Name}", body.Length, handler.GetType().Name);

        try
        {
            var payload = JsonSerializer.Deserialize(message, handler.PayloadType, _serializerOptions);

            await handler.HandleAsync(payload);
            queueModel.BasicAck(@event.DeliveryTag, false);
        }
        catch (Exception ex)
        {
            if (ex is JsonException)
            {
                logger.LogWarning("Payload deserialization of type {Name} failed.", handler.PayloadType.Name);

                await handler.HandleUnknownMessageAsync(message);
                queueModel.BasicAck(@event.DeliveryTag, false);

                return;
            }

            logger.LogError(ex, "An error occured while processing message.");
            queueModel.BasicNack(@event.DeliveryTag, false, true);
        }
    }
}
