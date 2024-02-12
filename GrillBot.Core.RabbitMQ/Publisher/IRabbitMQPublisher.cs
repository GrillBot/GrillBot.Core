namespace GrillBot.Core.RabbitMQ.Publisher;

public interface IRabbitMQPublisher
{
    Task PublishAsync<TModel>(string queueName, TModel model);
}
