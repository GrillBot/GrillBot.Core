﻿using System.Net;

namespace GrillBot.Core.Services.Common;

public class ClientException : HttpRequestException
{
    public ClientException()
    {
    }

    public ClientException(string? message) : base(message)
    {
    }

    public ClientException(string? message, Exception? inner) : base(message, inner)
    {
    }

    public ClientException(string? message, Exception? inner, HttpStatusCode? statusCode) : base(message, inner, statusCode)
    {
    }

    public ClientException(HttpStatusCode statusCode) : base(
        $"API returned status code {statusCode}"
    )
    {
    }

    public ClientException(HttpStatusCode statusCode, string content) : base(
        $"API returned status code {statusCode}\n{content}"
    )
    {
    }
}
