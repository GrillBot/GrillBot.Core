namespace GrillBot.Core.Infrastructure.Actions;

public interface IApiAction
{
    Task<ApiResult> ProcessAsync(object?[] parameters);
}
