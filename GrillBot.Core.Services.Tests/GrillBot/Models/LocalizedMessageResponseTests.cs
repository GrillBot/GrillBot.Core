﻿using GrillBot.Core.Services.GrillBot.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GrillBot.Core.Services.Tests.GrillBot.Models;

[TestClass]
public class LocalizedMessageResponseTests
{
    [TestMethod]
    public void ImplicitOperator_StringToLocalizedMessageResponse_CreatesWithKeyAndEmptyArgs()
    {
        // Arrange
        string key = "TestKey";

        // Act
        LocalizedMessageContent response = key;

        // Assert
        Assert.AreEqual(key, response.Key);
        Assert.IsNotNull(response.Args);
        Assert.AreEqual(0, response.Args.Length);
    }

    [TestMethod]
    public void ImplicitOperator_LocalizedMessageResponseToString_ReturnsKey()
    {
        // Arrange
        var response = new LocalizedMessageContent("TestKey", ["arg1", "arg2"]);

        // Act
        string key = response;

        // Assert
        Assert.AreEqual("TestKey", key);
    }

    [TestMethod]
    public void RecordEquality_WorksAsExpected()
    {
        // Arrange
        var response1 = new LocalizedMessageContent("Key", ["a", "b"]);
        var response2 = new LocalizedMessageContent("Key", ["a", "b"]);
        var response3 = new LocalizedMessageContent("Key", ["x"]);

        // Assert
        Assert.AreEqual(response1, response2);
        Assert.AreNotEqual(response1, response3);
    }
}