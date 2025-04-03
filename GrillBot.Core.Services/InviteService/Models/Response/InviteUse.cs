namespace GrillBot.Core.Services.InviteService.Models.Response;

public record InviteUse(
    string UserId,
    DateTime UsedAtUtc
);
