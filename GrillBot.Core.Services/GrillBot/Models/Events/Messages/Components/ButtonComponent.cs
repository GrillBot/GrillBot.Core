using Discord;

namespace GrillBot.Core.Services.GrillBot.Models.Events.Messages.Components;

public class ButtonComponent
{
    public string Label { get; set; } = null!;
    public string CustomId { get; set; } = null!;
    public ButtonStyle Style { get; set; }
    public string? EmoteId { get; set; }
    public string? Url { get; set; }
    public bool IsDisabled { get; set; }

    public ButtonComponent()
    {
    }

    public ButtonComponent(string label, string customId, ButtonStyle style, string? emoteId = null, string? url = null, bool isDisabled = false)
    {
        Label = label;
        CustomId = customId;
        Style = style;
        EmoteId = emoteId;
        Url = url;
        IsDisabled = isDisabled;
    }

    public ButtonBuilder ToBuilder()
    {
        var emote = string.IsNullOrEmpty(EmoteId) ? null : Discord.Emote.Parse(EmoteId);
        return new ButtonBuilder(Label, CustomId, Style, Url, emote, IsDisabled);
    }

    public Discord.ButtonComponent ToDiscordComponent()
        => ToBuilder().Build();
}
