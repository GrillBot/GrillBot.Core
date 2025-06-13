namespace GrillBot.Core.Services.UserManagementService.Models.Response;

public record GuildUser(
    string GuildId,
    string? CurrentNickname,
    List<string> NicknameHistory
);
