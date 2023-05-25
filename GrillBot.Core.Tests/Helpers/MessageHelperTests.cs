using GrillBot.Core.Helpers;

namespace GrillBot.Core.Tests.Helpers;

[TestClass]
public class MessageHelperTests
{
    [TestMethod]
    public void DiscordMessageUriRegex()
    {
        var result = MessageHelper.DiscordMessageUriRegex();

        Assert.IsNotNull(result);
        Assert.IsTrue(result.ToString().Contains("discord\\.com"));
        Assert.IsTrue(result.ToString().Contains("channels"));
    }
}
