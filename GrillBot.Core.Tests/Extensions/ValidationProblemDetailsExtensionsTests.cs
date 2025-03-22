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
    [ExpectedException(typeof(ValidationException))]
    [ExcludeFromCodeCoverage]
    public void AggregateAndThrow()
    {
        var details = new ValidationProblemDetails();
        details.Errors.Add("Err", ["Error"]);
        details.AggregateAndThrow();
    }

    [TestMethod]
    public void ThrowFirstError_Null()
        => ((ValidationProblemDetails?)null).ThrowFirstError();

    [TestMethod]
    public void ThrowFirstError_NoErrors()
        => new ValidationProblemDetails().ThrowFirstError();

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    [ExcludeFromCodeCoverage]
    public void ThrowFirstError()
    {
        var details = new ValidationProblemDetails();
        details.Errors.Add("Err", ["Error"]);
        details.ThrowFirstError();
    }
}
