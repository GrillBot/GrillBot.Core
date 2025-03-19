namespace GrillBot.Core.RabbitMQ.V2.Publisher;

public interface IRabbitPublisher
{
    Task PublishAsync<T>(string topicName, T data, string queueName, Dictionary<string, string>? headers = null);
    Task PublishAsync<T>(string topicName, List<T> data, string queueName, Dictionary<string, string>? headers = null);
}
