using GrillBot.Core.Validation;

namespace GrillBot.Core.Tests.Validation;

[TestClass]
public class EmoteIdAttributeTests : ValidationAttributeTestBase<EmoteIdAttribute>
{
    [TestMethod]
    public void NotString()
        => Assert.IsTrue(Attribute.IsValid(12345));

    [TestMethod]
    public void InvalidEmote()
        => Assert.IsFalse(Attribute.IsValid(":emote:"));

    [TestMethod]
    public void Success()
        => Assert.IsTrue(Attribute.IsValid("<a:test:12345>"));
}
