﻿namespace GrillBot.Core.Services.AuditLog.Models.Events.Create;

public class ApiRequestRequest
{
    public string ControllerName { get; set; } = null!;
    public string ActionName { get; set; } = null!;
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public string Method { get; set; } = null!;
    public string TemplatePath { get; set; } = null!;
    public string Path { get; set; } = null!;
    public Dictionary<string, string> Parameters { get; set; } = new();
    public string Language { get; set; } = null!;
    public string ApiGroupName { get; set; } = null!;
    public Dictionary<string, string> Headers { get; set; } = new();
    public string Identification { get; set; } = null!;
    public string Ip { get; set; } = null!;
    public string Result { get; set; } = null!;
    public string? Role { get; set; }

    public ApiRequestRequest()
    {
    }

    public ApiRequestRequest(
        string controllerName,
        string actionName,
        DateTime startAt,
        DateTime endAt,
        string method,
        string templatePath,
        string path,
        Dictionary<string, string> parameters,
        string language,
        string apiGroupName,
        Dictionary<string, string> headers,
        string identification,
        string ip,
        string result,
        string? role
    )
    {
        ControllerName = controllerName;
        ActionName = actionName;
        StartAt = startAt;
        EndAt = endAt;
        Method = method;
        TemplatePath = templatePath;
        Path = path;
        Parameters = parameters;
        Language = language;
        ApiGroupName = apiGroupName;
        Headers = headers;
        Identification = identification;
        Ip = ip;
        Result = result;
        Role = role;
    }
}
