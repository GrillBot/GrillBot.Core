#pragma warning disable IDE0290 // Use primary constructor
namespace GrillBot.Core.IO;

public sealed class TemporaryFile : IDisposable
{
    public string Path { get; private set; }

    public string Filename => System.IO.Path.GetFileName(Path);

    public TemporaryFile(string extension)
    {
        Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"{System.IO.Path.GetRandomFileName()}.{extension}");
    }

    public void ChangeExtension(string newExtension)
        => Path = System.IO.Path.ChangeExtension(Path, newExtension);

    public Task<byte[]> ReadAllBytesAsync() => File.ReadAllBytesAsync(Path);
    public Task<string[]> ReadAllLinesAsync() => File.ReadAllLinesAsync(Path);
    public Task<string> ReadAllTextAsync() => File.ReadAllTextAsync(Path);
    public Task WriteAllBytesAsync(byte[] bytes) => File.WriteAllBytesAsync(Path, bytes);
    public Task WriteAllLinesAsync(IEnumerable<string> lines) => File.WriteAllLinesAsync(Path, lines);
    public Task WriteAllTextAsync(string content) => File.WriteAllTextAsync(Path, content);

    public async Task WriteStreamAsync(Stream stream)
    {
        await using var fileStream = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite);

        await stream.CopyToAsync(fileStream);
        await fileStream.FlushAsync();
    }

    public void Dispose()
    {
        if (File.Exists(Path))
            File.Delete(Path);
    }

    public override string ToString() => Path;
}
