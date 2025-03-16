using System.Text;
using System.Text.Json;

namespace GrillBot.Core.Helpers;

public static class JsonHelper
{
    public static async Task<string> SerializeAsync<TData>(TData data)
    {
        using var document = JsonSerializer.SerializeToDocument(data);
        return await SerializeJsonDocumentAsync(document);
    }

    public static async Task<string> SerializeJsonDocumentAsync(JsonDocument document)
    {
        await using var stream = new MemoryStream();
        var writer = new Utf8JsonWriter(stream);

        document.WriteTo(writer);
        await writer.FlushAsync();

        return Encoding.UTF8.GetString(stream.ToArray());
    }
}
