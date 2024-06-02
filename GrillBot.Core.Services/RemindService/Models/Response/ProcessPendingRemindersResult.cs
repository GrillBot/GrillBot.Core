namespace GrillBot.Core.Services.RemindService.Models.Response;

public record ProcessPendingRemindersResult(int RemindersCount, List<string> Messages);
