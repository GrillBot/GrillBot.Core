using Discord;
using GrillBot.Core.RabbitMQ.V2.Messages;

namespace GrillBot.Core.Services.GrillBot.Models.Events.Messages;

public class DiscordMessagePayload : IRabbitMessage
{
    public string Topic => "GrillBot";
    public string Queue => "SendMessage";

    public string? GuildId { get; set; }
    public string ChannelId { get; set; } = null!;
    public string? Content { get; set; }
    public List<DiscordMessageFile> Attachments { get; set; } = [];
    public DiscordMessageEmbed? Embed { get; set; }
    public MessageFlags? Flags { get; set; }
    public DiscordMessageAllowedMentions? AllowedMentions { get; set; }
    public string ServiceId { get; set; } = null!;
    public Dictionary<string, string> ServiceData { get; set; } = [];
    public DiscordMessageComponent? Components { get; set; }

    public DiscordMessagePayload()
    {
    }

    public DiscordMessagePayload(
        string? guildId,
        string channelId,
        string? content,
        IEnumerable<DiscordMessageFile> attachments,
        string serviceId,
        DiscordMessageAllowedMentions? allowedMentions = null,
        MessageFlags? flags = null,
        DiscordMessageEmbed? embed = null,
        Dictionary<string, string>? serviceData = null,
        DiscordMessageComponent? components = null
    )
    {
        GuildId = guildId;
        ChannelId = channelId;
        Content = content;
        Attachments = attachments.Where(o => o is not null).ToList();
        Embed = embed;
        Flags = flags;
        AllowedMentions = allowedMentions;
        ServiceId = serviceId;
        ServiceData = serviceData ?? [];
        Components = components;
    }
}
