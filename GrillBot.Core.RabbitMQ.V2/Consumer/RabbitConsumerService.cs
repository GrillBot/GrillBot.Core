using GrillBot.Core.RabbitMQ.V2.Dispatcher;
using GrillBot.Core.RabbitMQ.V2.Factory;
using GrillBot.Core.RabbitMQ.V2.Messages;
using GrillBot.Core.RabbitMQ.V2.Options;
using GrillBot.Core.RabbitMQ.V2.Publisher;
using GrillBot.Core.RabbitMQ.V2.Telemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace GrillBot.Core.RabbitMQ.V2.Consumer;

public class RabbitConsumerService(
    IServiceProvider _serviceProvider,
    ILogger<RabbitConsumerService> _logger,
    IRabbitConnectionFactory _connectionFactory,
    IRabbitMessageDispatcher _dispatcher,
    IRabbitChannelFactory _channelFactory,
    IOptions<RabbitOptions> _options,
    TelemetryCollector _telemetry
) : IHostedService
{
    private readonly Dictionary<string, AsyncDefaultBasicConsumer> _consumers = [];

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var handler in FindHandlers())
            await InitializeConsumerAsync(handler.TopicName, handler.QueueName);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var consumerChannel in _consumers.Values.Where(o => o.IsRunning).Select(o => o.Channel))
        {
            await consumerChannel.CloseAsync(cancellationToken);
            await consumerChannel.DisposeAsync();
        }

        _consumers.Clear();
    }

    private IRabbitMessageHandler[] FindHandlers()
    {
        using var scope = _serviceProvider.CreateScope();
        return [.. scope.ServiceProvider.GetServices<IRabbitMessageHandler>()];
    }

    private async Task InitializeConsumerAsync(string topicName, string queueName)
    {
        var connection = await _connectionFactory.CreateAsync();
        var channel = await _channelFactory.CreateChannelAsync(connection, topicName, queueName);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (_, args) => OnQueueMessageReceivedAsync(args, queueName, channel);

        await channel.BasicConsumeAsync($"{topicName}.{queueName}", false, consumer);
        _consumers.Add($"{topicName}.{queueName}", consumer);
    }

    private async Task OnQueueMessageReceivedAsync(BasicDeliverEventArgs args, string queueName, IChannel channel)
    {
        using var scope = _serviceProvider.CreateScope();

        var handler = scope.ServiceProvider.GetServices<IRabbitMessageHandler>()
            .First(o => o.TopicName == args.Exchange && o.QueueName == queueName);

        var handlerType = handler.GetType();
        var body = args.Body.ToArray();

        var headers = args.BasicProperties?.Headers?
            .Select(o => new
            {
                o.Key,
                Value = o.Value is byte[] bytes ? Encoding.UTF8.GetString(bytes) : (o.Value?.ToString() ?? "")
            })
            .Where(o => !string.IsNullOrEmpty(o.Value))
            .ToDictionary(o => o.Key, o => o.Value) ?? [];

        _logger.LogInformation("Received new message. Length: {Length}, Handler: {Name}", body.Length, handlerType.Name);
        _telemetry.IncrementConsumer(args.Exchange, queueName);

        var handlePolicy = Policy
            .Handle<Exception>(handler.HandleException)
            .WaitAndRetryAsync(
                _options.Value.MaxRetryCount,
                _ => _options.Value.RetryDelay
            );

        try
        {
            var result = await handlePolicy.ExecuteAsync(() => _dispatcher.HandleMessageAsync(handler, body, headers));

            if (result == RabbitConsumptionResult.Success)
                await channel.BasicAckAsync(args.DeliveryTag, false);
            else if (result == RabbitConsumptionResult.Retry)
                await channel.BasicNackAsync(args.DeliveryTag, false, true);
            else // Reject
                await channel.BasicNackAsync(args.DeliveryTag, false, false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured while processing message.");

            var rawMessage = Convert.ToBase64String(body);
            var message = new RabbitErrorMessage(ex, args.Exchange, queueName, headers, handlerType.Name, rawMessage);

            await scope.ServiceProvider
                .GetRequiredService<IRabbitPublisher>()
                .PublishAsync(message);

            await channel.BasicNackAsync(args.DeliveryTag, false, false);
        }
    }
}
