using GrillBot.Core.Infrastructure.Auth;

namespace GrillBot.Core.RabbitMQ.Publisher;

public class RabbitPublisher : IRabbitPublisher
{
    private readonly IRabbitMQPublisher _publisher;
    private readonly ICurrentUserProvider _currentUser;

    public RabbitPublisher(IRabbitMQPublisher publisher, ICurrentUserProvider currentUser)
    {
        _publisher = publisher;
        _currentUser = currentUser;
    }

    public Task PublishAsync<TModel>(string queueName, TModel model) where TModel : IPayload
    {
        var headers = CreateHeaders();
        return _publisher.PublishAsync(queueName, model, headers);
    }

    public Task PublishAsync<TModel>(string queueName, TModel model, Dictionary<string, string> headers)
    {
        headers = CreateHeaders(headers);
        return _publisher.PublishAsync(queueName, model, headers);
    }

    public Task PublishAsync<TModel>(TModel model) where TModel : IPayload
    {
        var headers = CreateHeaders();
        return _publisher.PublishAsync(model.QueueName, model, headers);
    }

    public Task PublishAsync<TModel>(TModel model, Dictionary<string, string> headers) where TModel : IPayload
    {
        headers = CreateHeaders(headers);
        return _publisher.PublishAsync(model.QueueName, model, headers);
    }

    public Task PublishBatchAsync<TModel>(IEnumerable<TModel> models) where TModel : IPayload
    {
        var headers = CreateHeaders();
        return _publisher.PublishBatchAsync(models, headers);
    }

    public Task PublishBatchAsync<TModel>(IEnumerable<TModel> models, Dictionary<string, string> headers) where TModel : IPayload
    {
        headers = CreateHeaders(headers);
        return _publisher.PublishBatchAsync(models, headers);
    }

    private Dictionary<string, string> CreateHeaders(Dictionary<string, string>? headers = null)
    {
        headers ??= new Dictionary<string, string>();
        var internalHeaders = headers.ToDictionary(o => o.Key, o => o.Value);

        if (_currentUser.IsLogged)
            internalHeaders.Add("Authorization", _currentUser.EncodedJwtToken!);

        return internalHeaders;
    }
}
