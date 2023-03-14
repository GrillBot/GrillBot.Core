using Discord;

namespace GrillBot.Core.Managers.Discord;

public interface IEmoteManager
{
    Task<List<GuildEmote>> GetSupportedEmotesAsync();
}
