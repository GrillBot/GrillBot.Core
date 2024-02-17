using System.Text.Json;

namespace GrillBot.Core.RabbitMQ;

internal static class RabbitMQSettings
{
    public static readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
