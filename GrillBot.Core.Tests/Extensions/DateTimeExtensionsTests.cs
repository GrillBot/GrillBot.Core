using GrillBot.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillBot.Core.Tests.Extensions;

[TestClass]
public class DateTimeExtensionsTests
{
    [TestMethod]
    public void WithKind_LocalToUtc()
    {
        var localTime = new DateTime(2023, 10, 9, 10, 23, 42, DateTimeKind.Local);
        var result = localTime.WithKind(DateTimeKind.Utc);

        Assert.AreEqual(localTime.Year, result.Year);
        Assert.AreEqual(localTime.Month, result.Month);
        Assert.AreEqual(localTime.Day, result.Day); ;
        Assert.AreEqual(localTime.Hour, result.Hour);
        Assert.AreEqual(localTime.Minute, result.Minute);
        Assert.AreEqual(localTime.Second, result.Second);
        Assert.AreEqual(DateTimeKind.Utc, result.Kind);
    }

    [TestMethod]
    public void WithKind_UtcToLocal()
    {
        var localTime = new DateTime(2023, 10, 9, 10, 23, 42, DateTimeKind.Utc);
        var result = localTime.WithKind(DateTimeKind.Local);

        Assert.AreEqual(localTime.Year, result.Year);
        Assert.AreEqual(localTime.Month, result.Month);
        Assert.AreEqual(localTime.Day, result.Day); ;
        Assert.AreEqual(localTime.Hour, result.Hour);
        Assert.AreEqual(localTime.Minute, result.Minute);
        Assert.AreEqual(localTime.Second, result.Second);
        Assert.AreEqual(DateTimeKind.Local, result.Kind);
    }
}
