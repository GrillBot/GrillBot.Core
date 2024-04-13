﻿using Discord;

namespace GrillBot.Core.Services.GrillBot.Models.Events;

public class DiscordMessageEmbedField
{
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
    public bool IsInline { get; set; }

    public DiscordMessageEmbedField()
    {
    }

    public DiscordMessageEmbedField(string name, string value, bool isInline)
    {
        Name = name;
        Value = value;
        IsInline = isInline;
    }

    public EmbedFieldBuilder ToBuilder()
        => new EmbedFieldBuilder().WithIsInline(IsInline).WithName(Name).WithValue(Value);

    public static DiscordMessageEmbedField FromEmbed(EmbedField field)
        => new(field.Name, field.Value, field.Inline);

    public static DiscordMessageEmbedField FromEmbed(EmbedFieldBuilder builder)
        => new(builder.Name, builder.Value?.ToString() ?? "", builder.IsInline);
}
