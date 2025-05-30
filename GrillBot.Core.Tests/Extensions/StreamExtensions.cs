using GrillBot.Core.Extensions;

namespace GrillBot.Core.Tests.Extensions;

[TestClass]
public class StreamExtensions
{
    [TestMethod]
    public void ToByteArray()
    {
        var sourceData = new byte[] { 1, 2, 3 };

        using var sourceStream = new MemoryStream(sourceData);
        var result = sourceStream.ToByteArray();

        Assert.IsNotNull(result);
        Assert.IsTrue(result.SequenceEqual(sourceData));
    }

    [TestMethod]
    public async Task ToByteArrayAsync()
    {
        var sourceData = new byte[] { 1, 2, 3 };

        await using var sourceStream = new MemoryStream(sourceData);
        var result = await sourceStream.ToByteArrayAsync();

        Assert.IsNotNull(result);
        Assert.IsTrue(result.SequenceEqual(sourceData));
    }
}
