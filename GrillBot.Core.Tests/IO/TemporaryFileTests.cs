using GrillBot.Core.IO;
using Microsoft.Testing.Platform.Extensions.Messages;

namespace GrillBot.Core.Tests.IO;

[TestClass]
public class TemporaryFileTests
{
    private static async Task InitFileAsync(TemporaryFile file)
        => await File.WriteAllTextAsync(file.Path, "ASDF");

    [TestMethod]
    public async Task CreateAndDisposeAsync()
    {
        var file = new TemporaryFile("txt");
        await InitFileAsync(file);

        file.Dispose();
        Assert.IsFalse(File.Exists(file.Path));
    }

    [TestMethod]
    public void ChangeExtension()
    {
        const string newExtension = "jpg";

        using var file = new TemporaryFile("txt");
        file.ChangeExtension(newExtension);

        Assert.AreEqual($".{newExtension}", Path.GetExtension(file.Path));
    }

    [TestMethod]
    public async Task ReadAllBytesAsync()
    {
        using var file = new TemporaryFile("txt");
        await InitFileAsync(file);

        var result = await file.ReadAllBytesAsync();
        Assert.AreEqual(4, result.Length);
    }

    [TestMethod]
    public async Task ReadAllLinesAsync()
    {
        using var file = new TemporaryFile("txt");
        await InitFileAsync(file);

        var result = await file.ReadAllLinesAsync();
        Assert.AreEqual(1, result.Length);
    }

    [TestMethod]
    public async Task ReadAllTextAsync()
    {
        using var file = new TemporaryFile("txt");
        await InitFileAsync(file);

        var result = await file.ReadAllTextAsync();
        Assert.AreEqual(4, result.Length);
    }

    [TestMethod]
    public async Task WriteAllBytesAsync()
    {
        using var file = new TemporaryFile("txt");

        await file.WriteAllBytesAsync([1, 2, 3, 4]);
        var result = await file.ReadAllBytesAsync();

        Assert.AreEqual(4, result.Length);
    }

    [TestMethod]
    public async Task WriteAllLinesAsync()
    {
        using var file = new TemporaryFile("txt");

        await file.WriteAllLinesAsync(new[] { "1" });
        var result = await file.ReadAllLinesAsync();

        Assert.AreEqual(1, result.Length);
    }

    [TestMethod]
    public async Task WriteAllTextAsync()
    {
        using var file = new TemporaryFile("txt");

        await file.WriteAllTextAsync("text");
        var result = await file.ReadAllTextAsync();

        Assert.AreEqual("text", result);
    }

    [TestMethod]
    public void FilenameProperty()
    {
        using var file = new TemporaryFile("txt");

        Assert.IsFalse(string.IsNullOrEmpty(file.Filename));
    }

    [TestMethod]
    public async Task WriteStreamAsync()
    {
        byte[] data = [1, 2, 3, 4];

        await using var sourceStream = new MemoryStream(data);
        using var file = new TemporaryFile("txt");

        await file.WriteStreamAsync(sourceStream);
        var result = await file.ReadAllBytesAsync();

        Assert.AreEqual(data.Length, result.Length);
    }
}
