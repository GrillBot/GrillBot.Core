using Discord;

namespace GrillBot.Core.Services.GrillBot.Models.Events.Messages.Embeds;

public class DiscordMessageEmbed
{
    public string? Url { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DiscordMessageEmbedAuthor? Author { get; set; }
    public uint? Color { get; set; }
    public DiscordMessageEmbedFooter? Footer { get; set; }
    public string? ImageUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
    public List<DiscordMessageEmbedField> Fields { get; set; } = [];
    public DateTime? Timestamp { get; set; }
    public bool UseCurrentTimestamp { get; set; }

    public DiscordMessageEmbed()
    {
    }

    public DiscordMessageEmbed(
        string? url,
        string? title,
        string? description,
        DiscordMessageEmbedAuthor? author,
        uint? color,
        DiscordMessageEmbedFooter? footer,
        string? imageUrl,
        string? thumbnailUrl,
        IEnumerable<DiscordMessageEmbedField> fields,
        DateTime? timestamp,
        bool useCurrentTimestamp
    )
    {
        Url = url;
        Title = title;
        Description = description;
        Author = author;
        Color = color;
        Footer = footer;
        ImageUrl = imageUrl;
        ThumbnailUrl = thumbnailUrl;
        Fields = [.. fields.Take(EmbedBuilder.MaxFieldCount)];
        Timestamp = timestamp;
        UseCurrentTimestamp = useCurrentTimestamp;
    }

    public EmbedBuilder ToBuilder()
    {
        var builder = new EmbedBuilder();

        if (!string.IsNullOrEmpty(Url))
            builder = builder.WithUrl(Url);
        if (!string.IsNullOrEmpty(Title))
            builder = builder.WithTitle(Title);
        if (!string.IsNullOrEmpty(Description))
            builder = builder.WithDescription(Description);
        if (Author is not null)
            builder = builder.WithAuthor(Author.ToBuilder());
        if (Color is not null)
            builder = builder.WithColor(Color.Value);
        if (Footer is not null)
            builder = builder.WithFooter(Footer.ToBuilder());
        if (!string.IsNullOrEmpty(ImageUrl))
            builder = builder.WithImageUrl(ImageUrl);
        if (!string.IsNullOrEmpty(ThumbnailUrl))
            builder = builder.WithThumbnailUrl(ThumbnailUrl);
        if (Fields.Count > 0)
            builder = builder.WithFields(Fields.Select(f => f.ToBuilder()));
        if (Timestamp is not null)
            builder = builder.WithTimestamp(new DateTimeOffset(Timestamp.Value.ToUniversalTime()));
        if (UseCurrentTimestamp)
            builder = builder.WithCurrentTimestamp();

        return builder;
    }

    public bool IsValidEmbed()
    {
        try
        {
            ToBuilder().Build();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static DiscordMessageEmbed FromEmbed(IEmbed embed)
    {
        return new DiscordMessageEmbed(
            embed.Url,
            embed.Title,
            embed.Description,
            DiscordMessageEmbedAuthor.FromEmbed(embed.Author),
            embed.Color?.RawValue,
            DiscordMessageEmbedFooter.FromEmbed(embed.Footer),
            embed.Image?.Url,
            embed.Thumbnail?.Url,
            embed.Fields.Select(DiscordMessageEmbedField.FromEmbed),
            embed.Timestamp?.DateTime,
            false
        );
    }
}
