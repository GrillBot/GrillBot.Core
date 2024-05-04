using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.RubbergodService.Models.Help;
using GrillBot.Core.Services.RubbergodService.Models.Karma;

namespace GrillBot.Core.Services.RubbergodService;

public interface IRubbergodServiceClient : IClient
{
    Task<PaginatedResponse<UserKarma>> GetKarmaPageAsync(PaginatedParams parameters);
    Task<byte[]> GetPinsAsync(ulong guildId, ulong channelId, bool markdown);
    Task<Dictionary<string, Cog>> GetSlashCommandsAsync();
}
