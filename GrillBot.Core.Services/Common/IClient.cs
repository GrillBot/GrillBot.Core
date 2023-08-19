﻿namespace GrillBot.Core.Services.Common;

public interface IClient
{
    string ServiceName { get; }
    string Url { get; }
    int Timeout { get; }

    Task<bool> IsAvailableAsync();
}
