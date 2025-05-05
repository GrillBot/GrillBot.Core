using Discord;
using GrillBot.Core.Services.GrillBot.Models.Events.Messages.Components;
using GrillBot.Core.Services.GrillBot.Models.Events.Messages.Embeds;

namespace GrillBot.Core.Services.GrillBot.Models.Events.Messages;

public class DiscordMessagePayloadData
{
    public string? Content { get; set; }
    public List<DiscordMessageFile> Attachments { get; set; } = [];
    public DiscordMessageEmbed? Embed { get; set; }
    public MessageFlags? Flags { get; set; }
    public DiscordMessageAllowedMentions? AllowedMentions { get; set; }
    public string ServiceId { get; set; } = null!;
    public Dictionary<string, string> ServiceData { get; set; } = [];
    public DiscordMessageComponent? Components { get; set; }

    public bool CanUseLocalizedEmbeds => ServiceData.TryGetValue("UseLocalizedEmbeds", out var _useLocalizedEmbeds) && _useLocalizedEmbeds == "true";
    public bool CanUseLocalizedContent => ServiceData.TryGetValue("UseLocalizedContent", out var _useLocalizedContent) && _useLocalizedContent == "true";
    public string? Locale => ServiceData.TryGetValue("Language", out var _locale) ? _locale : null;

    public DiscordMessagePayloadData()
    {
    }

    public DiscordMessagePayloadData(
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
        Content = content;
        Attachments = [.. attachments.Where(o => o is not null)];
        Embed = embed;
        Flags = flags;
        AllowedMentions = allowedMentions;
        ServiceId = serviceId;
        ServiceData = serviceData ?? [];
        Components = components;
    }

    public DiscordMessagePayloadData WithLocalization(bool useLocalizedEmbeds = true, bool useLocalizedContent = true, string? locale = null)
    {
        if (useLocalizedContent)
            ServiceData.TryAdd("UseLocalizedContent", "true");
        if (useLocalizedEmbeds)
            ServiceData.TryAdd("UseLocalizedEmbeds", "true");
        if (!string.IsNullOrEmpty(locale))
            ServiceData.TryAdd("Language", locale);

        return this;
    }
}
