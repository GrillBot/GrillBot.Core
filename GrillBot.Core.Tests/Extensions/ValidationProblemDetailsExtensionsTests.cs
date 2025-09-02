using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using GrillBot.Core.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GrillBot.Core.Tests.Extensions;

[TestClass]
public class ValidationProblemDetailsExtensionsTests
{
    [TestMethod]
    public void AggregateAndThrow_Null()
        => ((ValidationProblemDetails?)null).AggregateAndThrow();

    [TestMethod]
    public void AggregateAndThrow_NoErrors()
        => new ValidationProblemDetails().AggregateAndThrow();

    [TestMethod]
    public void AggregateAndThrow()
    {
        var details = new ValidationProblemDetails();
        details.Errors.Add("Err", ["Error"]);

        Assert.ThrowsExactly<ValidationException>(details.AggregateAndThrow);
    }

    [TestMethod]
    public void ThrowFirstError_Null()
        => ((ValidationProblemDetails?)null).ThrowFirstError();

    [TestMethod]
    public void ThrowFirstError_NoErrors()
        => new ValidationProblemDetails().ThrowFirstError();

    [TestMethod]
    public void ThrowFirstError()
    {
        var details = new ValidationProblemDetails();
        details.Errors.Add("Err", ["Error"]);

        Assert.ThrowsExactly<ValidationException>(details.ThrowFirstError);
    }
}
