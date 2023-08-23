using GrillBot.Core.Tests.Validation.ModelValidator;
using System.ComponentModel.DataAnnotations;

namespace GrillBot.Core.Tests.Validation;

[TestClass]
public class ModelValidatorTests
{
    [TestMethod]
    public void NoValue()
    {
        var result = ProcessTest(new DataModelClass());
        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public void AllFailed()
    {
        var data = new DataModelClass
        {
            Value2 = DateTime.Now.AddDays(1),
            Value3 = DateTime.Now,
            Value4 = 50,
            Value5 = 5
        };

        var result = ProcessTest(data);
        Assert.AreEqual(5, result.Count);
    }

    [TestMethod]
    public void AllSuccess()
    {
        var data = new DataModelClass
        {
            Value1 = "Test",
            Value2 = DateTime.UtcNow.AddDays(-1),
            Value3 = DateTime.UtcNow,
            Value4 = 5,
            Value5 = 50
        };

        var result = ProcessTest(data);
        Assert.AreEqual(0, result.Count);
    }

    private List<ValidationResult> ProcessTest(DataModelClass data)
    {
        var context = new ValidationContext(this) { MemberName = "Test" };
        var validator = new ClassForModelValidation();
        var result = validator.Validate(data, context);

        Assert.IsNotNull(result);
        return result.ToList();
    }
}
