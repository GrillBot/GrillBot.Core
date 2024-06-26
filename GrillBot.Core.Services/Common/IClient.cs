using GrillBot.Core.Services.Diagnostics.Models;

namespace GrillBot.Core.Services.Common;

public interface IClient
{
    string ServiceName { get; }
    string Url { get; }

    Task<bool> IsHealthyAsync();
    Task<DiagnosticInfo> GetDiagnosticAsync();
    Task<long> GetUptimeAsync();
}
