﻿namespace GrillBot.Core.Services.SearchingService.Models.Response;

public record SearchListItem(
    long Id,
    string UserId,
    string GuildId,
    string ChannelId,
    string Content,
    DateTime CreatedAtUtc,
    DateTime ValidToUtc,
    bool IsInvalid,
    bool IsDeleted
);
