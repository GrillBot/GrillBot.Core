using Discord;
using GrillBot.Core.RabbitMQ.V2.Messages;
using GrillBot.Core.Services.GrillBot.Models.Events.Messages.Components;
using GrillBot.Core.Services.GrillBot.Models.Events.Messages.Embeds;

namespace GrillBot.Core.Services.GrillBot.Models.Events.Messages;

public class DiscordSendMessagePayload : DiscordMessagePayloadData, IRabbitMessage
{
    public string Topic => "GrillBot";
    public string Queue => "SendMessage";

    public ulong? GuildId { get; set; }
    public ulong ChannelId { get; set; }

    public DiscordSendMessagePayload()
    {
    }

    public DiscordSendMessagePayload(
        ulong guildId,
        ulong channelId,
        string? content,
        IEnumerable<DiscordMessageFile> attachments,
        string serviceId,
        DiscordMessageAllowedMentions? allowedMentions = null,
        MessageFlags? flags = null,
        DiscordMessageEmbed? embed = null,
        Dictionary<string, string>? serviceData = null,
        DiscordMessageComponent? components = null
    ) : base(content, attachments, serviceId, allowedMentions, flags, embed, serviceData, components)
    {
        GuildId = guildId;
        ChannelId = channelId;
    }
}
