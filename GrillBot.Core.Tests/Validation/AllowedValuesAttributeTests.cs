using GrillBot.Core.Validation;

namespace GrillBot.Core.Tests.Validation;

public class AllowedValuesAttributeTests : ValidationAttributeTestBase<AllowedValuesAttribute>
{
    protected override AllowedValuesAttribute CreateAttribute() => new();

    [TestMethod]
    public void NullValue()
        => Assert.IsNull(Attribute.GetValidationResult(null, Context));

    [TestMethod]
    public void NoValues()
        => Assert.IsNotNull(Attribute.GetValidationResult("Value", Context));

    [TestMethod]
    public void AllowedValue()
    {
        Attribute = new AllowedValuesAttribute("Value");
        Assert.IsNull(Attribute.GetValidationResult("Value", Context));
    }
}
