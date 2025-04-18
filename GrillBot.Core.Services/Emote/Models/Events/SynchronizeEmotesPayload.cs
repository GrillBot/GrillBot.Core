﻿using Discord;
using GrillBot.Core.RabbitMQ.V2.Messages;

namespace GrillBot.Core.Services.Emote.Models.Events;

public class SynchronizeEmotesPayload : IRabbitMessage
{
    public string Topic => "Emote";
    public string Queue => "Synchronization";

    public string GuildId { get; set; } = null!;
    public List<string> Emotes { get; set; } = [];

    public SynchronizeEmotesPayload()
    {
    }

    public SynchronizeEmotesPayload(string guildId, IEnumerable<string> emotes)
    {
        GuildId = guildId;
        Emotes.AddRange(emotes);
    }

    public SynchronizeEmotesPayload(string guildId, IEnumerable<IEmote> emotes) : this(guildId, emotes.Select(o => o.ToString()!))
    {
    }
}
