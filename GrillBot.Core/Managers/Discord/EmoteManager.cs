using Discord;

namespace GrillBot.Core.Managers.Discord;

public class EmoteManager : IEmoteManager
{
    private IDiscordClient DiscordClient { get; }

    public EmoteManager(IDiscordClient discordClient)
    {
        DiscordClient = discordClient;
    }

    public async Task<List<GuildEmote>> GetSupportedEmotesAsync()
    {
        var guilds = await DiscordClient.GetGuildsAsync();
        return guilds.SelectMany(o => o.Emotes).ToList();
    }
}
