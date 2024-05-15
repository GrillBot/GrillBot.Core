namespace GrillBot.Core.RabbitMQ.Publisher;

public interface IRabbitMQPublisher
{
    Task PublishAsync<TModel>(string queueName, TModel model) where TModel : IPayload;
    Task PublishAsync<TModel>(string queueName, TModel model, Dictionary<string, string> headers);
    Task PublishAsync<TModel>(TModel model) where TModel : IPayload;
    Task PublishAsync<TModel>(TModel model, Dictionary<string, string> headers) where TModel : IPayload;
    Task PublishBatchAsync<TModel>(IEnumerable<TModel> models) where TModel : IPayload;
    Task PublishBatchAsync<TModel>(IEnumerable<TModel> models, Dictionary<string, string> headers) where TModel : IPayload;
}
