namespace GrillBot.Core.Services.Common;

public interface IClient
{
    string ServiceName { get; }
    string Url { get; }

    Task<bool> IsAvailableAsync();
}
