﻿using Discord;

namespace GrillBot.Core.Extensions.Discord;

public static class UserExtensions
{
    public static bool IsUser(this IUser user)
        => user is { IsBot: false, IsWebhook: false };

    public static string GetFullName(this IUser user)
    {
        if (user is IGuildUser guildUser && !string.IsNullOrEmpty(guildUser.Nickname))
        {
            return !string.IsNullOrEmpty(user.GlobalName) && user.GlobalName != user.Username ?
                $"{guildUser.Nickname} ({user.GlobalName} / {user.Username})" :
                $"{guildUser.Nickname} ({user.Username})";
        }

        return !string.IsNullOrEmpty(user.GlobalName) && user.GlobalName != user.Username ?
            $"{user.GlobalName} / {user.Username}" :
            user.Username;
    }

    public static string GetDisplayName(this IUser user)
    {
        if (user is IGuildUser guildUser && !string.IsNullOrEmpty(guildUser.Nickname))
            return guildUser.Nickname;
        return string.IsNullOrEmpty(user.GlobalName) ? user.Username : user.GlobalName;
    }
}
