using GrillBot.Core.RabbitMQ;

namespace GrillBot.Core.Services.GrillBot.Models.Events.Messages;

public class CreatedDiscordMessagePayload : IPayload
{
    public string QueueName => "grillbot:created_message";

    public string? GuildId { get; set; }
    public string ChannelId { get; set; } = null!;
    public string MessageId { get; set; } = null!;
    public string ServiceId { get; set; } = null!;
    public Dictionary<string, string> ServiceData { get; set; } = new();

    public CreatedDiscordMessagePayload()
    {
    }

    public CreatedDiscordMessagePayload(string? guildId, string channelId, string messageId, string serviceId, Dictionary<string, string> serviceData)
    {
        GuildId = guildId;
        ChannelId = channelId;
        MessageId = messageId;
        ServiceId = serviceId;
        ServiceData = serviceData;
    }
}
