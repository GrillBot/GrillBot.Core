namespace GrillBot.Core.Services.InviteService.Models.Response;

public record UserInviteUse(
    string GuildId,
    string Code,
    DateTime JoinedAtUtc
);
