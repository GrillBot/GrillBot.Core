using GrillBot.Core.Infrastructure.Actions;
using Microsoft.AspNetCore.Http;

namespace GrillBot.Core.Tests.Infrastructure.Actions;

[TestClass]
public class ApiResultTests
{
    [TestMethod]
    public void FromSuccess()
    {
        var result = ApiResult.FromSuccess();

        Assert.IsNotNull(result);
        Assert.IsNull(result.Data);
        Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
    }
}
