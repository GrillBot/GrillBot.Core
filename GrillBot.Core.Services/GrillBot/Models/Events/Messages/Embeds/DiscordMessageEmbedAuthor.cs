using Discord;

namespace GrillBot.Core.Services.GrillBot.Models.Events.Messages.Embeds;

public class DiscordMessageEmbedAuthor
{
    public LocalizedMessageContent? Name { get; set; }
    public LocalizedMessageContent? Url { get; set; }
    public LocalizedMessageContent? IconUrl { get; set; }

    public DiscordMessageEmbedAuthor()
    {
    }

    public DiscordMessageEmbedAuthor(string? name, string? url, string? iconUrl)
    {
        Name = string.IsNullOrEmpty(name) ? null : new(name, []);
        Url = string.IsNullOrEmpty(url) ? null : new(url, []);
        IconUrl = string.IsNullOrEmpty(iconUrl) ? null : new(iconUrl, []);
    }

    public EmbedAuthorBuilder ToBuilder()
    {
        var builder = new EmbedAuthorBuilder();
        if (!string.IsNullOrEmpty(Name?.Key))
            builder = builder.WithName(Name);
        if (!string.IsNullOrEmpty(Url?.Key))
            builder = builder.WithName(Url);
        if (!string.IsNullOrEmpty(IconUrl?.Key))
            builder = builder.WithIconUrl(IconUrl);

        return builder;
    }

    public static DiscordMessageEmbedAuthor? FromEmbed(EmbedAuthor? author)
        => author is null ? null : new(author.Value.Name, author.Value.Url, author.Value.IconUrl);

    public static DiscordMessageEmbedAuthor? FromEmbed(EmbedAuthorBuilder? builder)
        => builder is null ? null : new(builder.Name, builder.Url, builder.IconUrl);
}
