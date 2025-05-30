namespace GrillBot.Core.Extensions;

public static class StreamExtensions
{
    public static byte[] ToByteArray(this Stream stream)
    {
        using var ms = new MemoryStream();
        stream.CopyTo(ms);

        return ms.ToArray();
    }

    public static async Task<byte[]> ToByteArrayAsync(this Stream stream)
    {
        await using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);

        return ms.ToArray();
    }
}
