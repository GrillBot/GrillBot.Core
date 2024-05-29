using Discord;

namespace GrillBot.Core.Services.GrillBot.Models.Events.Messages;

public class DiscordMessageEmbedAuthor
{
    public string? Name { get; set; }
    public string? Url { get; set; }
    public string? IconUrl { get; set; }

    public DiscordMessageEmbedAuthor()
    {
    }

    public DiscordMessageEmbedAuthor(string? name, string? url, string? iconUrl)
    {
        Name = name;
        Url = url;
        IconUrl = iconUrl;
    }

    public EmbedAuthorBuilder ToBuilder()
        => new EmbedAuthorBuilder().WithName(Name).WithUrl(Url).WithIconUrl(IconUrl);

    public static DiscordMessageEmbedAuthor? FromEmbed(EmbedAuthor? author)
        => author is null ? null : new(author.Value.Name, author.Value.Url, author.Value.IconUrl);

    public static DiscordMessageEmbedAuthor? FromEmbed(EmbedAuthorBuilder? builder)
        => builder is null ? null : new(builder.Name, builder.Url, builder.IconUrl);
}
