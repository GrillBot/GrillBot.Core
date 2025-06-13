namespace GrillBot.Core.Services.UserManagementService.Models.Response;

public record UserInfo(
    string UserId,
    List<GuildUser> Guilds
);
