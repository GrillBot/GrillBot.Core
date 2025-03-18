using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace GrillBot.Core.RabbitMQ.V2.Serialization.Json;

public class JsonRabbitMessageSerializer : BaseRabbitMessageSerializer, IJsonRabbitMessageSerializer
{
    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public async Task<JsonNode?> DeserializeToJsonObjectAsync(byte[] bytes, Encoding? encoding = null)
    {
        var rawMessage = await DeserializeToStringAsync(bytes, encoding);
        return JsonNode.Parse(rawMessage);
    }

    public override Task<byte[]> SerializeMessageAsync<T>(T data, Encoding? encoding = null)
    {
        var json = JsonSerializer.Serialize(data, _serializerOptions);
        var bytes = (encoding ?? Encoding.UTF8).GetBytes(json);

        return Task.FromResult(bytes);
    }
}
