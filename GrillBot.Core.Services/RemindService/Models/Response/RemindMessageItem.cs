namespace GrillBot.Core.Services.RemindService.Models.Response;

public record RemindMessageItem(
    long Id,
    string FromUserId,
    string ToUserId,
    DateTime NotifyAtUtc,
    string Message,
    int PostponeCount,
    string? NotificationMessageId,
    string CommandMessageId,
    string Language,
    bool IsSendInProgress
);
