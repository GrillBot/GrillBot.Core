namespace GrillBot.Core.Infrastructure;

public interface IRequestFilterAction
{
    Task BeforeExecutionAsync();
    Task AfterExecutionAsync();
}
