using Discord;
using GrillBot.Core.RabbitMQ.Attributes;

namespace GrillBot.Core.Services.GrillBot.Models.Events;

[RabbitQueueMessage("discord:send_message")]
public class DiscordMessagePayload
{
    public string? GuildId { get; set; }
    public string ChannelId { get; set; } = null!;
    public string? Content { get; set; }
    public List<DiscordMessageFile> Attachments { get; set; } = new();
    public DiscordMessageEmbed? Embed { get; set; }
    public MessageFlags? Flags { get; set; }
    public DiscordMessageAllowedMentions? AllowedMentions { get; set; }

    public DiscordMessagePayload()
    {
    }

    public DiscordMessagePayload(
        string? guildId,
        string channelId,
        string? content,
        IEnumerable<DiscordMessageFile> attachments,
        DiscordMessageAllowedMentions? allowedMentions = null,
        MessageFlags? flags = null,
        DiscordMessageEmbed? embed = null
    )
    {
        GuildId = guildId;
        ChannelId = channelId;
        Content = content;
        Attachments = attachments.Where(o => o is not null).ToList();
        Embed = embed;
        Flags = flags;
        AllowedMentions = allowedMentions;
    }
}
