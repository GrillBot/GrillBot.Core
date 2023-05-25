using System.ComponentModel.DataAnnotations;

namespace GrillBot.Core.Tests.Validation;

public class ValidationAttributeTestBase<TAttribute> where TAttribute : ValidationAttribute, new()
{
    protected TAttribute Attribute { get; private set; } = null!;

    protected ValidationContext Context { get; private set; } = null!;

    [TestInitialize]
    public void Initialize()
    {
        Attribute = new TAttribute();
        Context = new ValidationContext(this) { MemberName = "Test" };
    }
}
