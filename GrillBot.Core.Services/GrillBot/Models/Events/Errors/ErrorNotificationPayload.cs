using GrillBot.Core.RabbitMQ;

namespace GrillBot.Core.Services.GrillBot.Models.Events.Errors;

public class ErrorNotificationPayload : IPayload
{
    public string QueueName => "grillbot:error_notifications";

    public string? Title { get; set; }
    public List<ErrorNotificationField> Fields { get; set; } = new();
    public ulong? UserId { get; set; }

    public ErrorNotificationPayload()
    {
    }

    public ErrorNotificationPayload(string? title, IEnumerable<ErrorNotificationField> fields, ulong? userId)
    {
        Title = title;
        Fields = fields.Where(o => o is not null).ToList();
        UserId = userId;
    }
}
