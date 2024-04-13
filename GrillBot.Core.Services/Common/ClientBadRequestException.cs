using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GrillBot.Core.Services.Common;

public class ClientBadRequestException : ClientException
{
    public Dictionary<string, string[]> ValidationErrors { get; } = new();

    public ClientBadRequestException()
    {
    }

    public ClientBadRequestException(string? message) : base(message)
    {
    }

    public ClientBadRequestException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public ClientBadRequestException(ValidationProblemDetails problemDetails) : base(HttpStatusCode.BadRequest)
    {
        ValidationErrors = problemDetails.Errors.ToDictionary(o => o.Key, o => o.Value);
    }

    public ClientBadRequestException(string? message, Exception? inner, HttpStatusCode? statusCode) : base(message, inner, statusCode)
    {
    }

    public ClientBadRequestException(HttpStatusCode statusCode) : base(statusCode)
    {
    }

    public ClientBadRequestException(HttpStatusCode statusCode, string content) : base(statusCode, content)
    {
    }
}
