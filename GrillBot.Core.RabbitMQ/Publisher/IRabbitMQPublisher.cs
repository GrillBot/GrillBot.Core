namespace GrillBot.Core.RabbitMQ.Publisher;

[Obsolete("Use IRabbitPublisher")]
public interface IRabbitMQPublisher
{
    Task PublishAsync<TModel>(string queueName, TModel model);
    Task PublishAsync<TModel>(TModel model) where TModel : IPayload;
    Task PublishBatchAsync<TModel>(IEnumerable<TModel> models) where TModel : IPayload;
}
