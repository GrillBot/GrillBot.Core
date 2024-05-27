using Discord;

namespace GrillBot.Core.Services.GrillBot.Models.Events;

public class DiscordMessageEmbedFooter
{
    public string? Text { get; set; }
    public string? IconUrl { get; set; }

    public DiscordMessageEmbedFooter()
    {
    }

    public DiscordMessageEmbedFooter(string? text, string? iconUrl)
    {
        Text = text;
        IconUrl = iconUrl;
    }

    public EmbedFooterBuilder ToBuilder()
        => new EmbedFooterBuilder().WithText(Text).WithIconUrl(IconUrl);

    public static DiscordMessageEmbedFooter? FromEmbed(EmbedFooter? footer)
        => footer is null ? null : new(footer.Value.Text, footer.Value.IconUrl);

    public static DiscordMessageEmbedFooter? FromEmbed(EmbedFooterBuilder? builder)
        => builder is null ? null : new(builder.Text, builder.IconUrl);
}
