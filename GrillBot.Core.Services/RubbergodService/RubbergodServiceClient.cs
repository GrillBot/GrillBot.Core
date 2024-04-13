using GrillBot.Core.Managers.Performance;
using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Common.Extensions;
using GrillBot.Core.Services.RubbergodService.Models.Help;
using GrillBot.Core.Services.RubbergodService.Models.Karma;

namespace GrillBot.Core.Services.RubbergodService;

public class RubbergodServiceClient : RestServiceBase, IRubbergodServiceClient
{
    private static readonly TimeSpan _defaultTimeout = TimeSpan.FromMinutes(1);

    public override string ServiceName => "RubbergodService";

    public RubbergodServiceClient(ICounterManager counterManager, IHttpClientFactory clientFactory) : base(counterManager, clientFactory)
    {
    }

    public async Task<PaginatedResponse<UserKarma>> GetKarmaPageAsync(PaginatedParams parameters)
    {
        var uri = $"api/karma?Page={parameters.Page}&PageSize={parameters.PageSize}";
        return (await ProcessRequestAsync<PaginatedResponse<UserKarma>>(() => HttpMethod.Get.ToRequest(uri), _defaultTimeout))!;
    }

    public Task StoreKarmaAsync(List<KarmaItem> items)
        => ProcessRequestAsync(() => HttpMethod.Post.ToRequest("api/karma", items), _defaultTimeout);

    public Task InvalidatePinCacheAsync(ulong guildId, ulong channelId)
        => ProcessRequestAsync(() => HttpMethod.Delete.ToRequest($"api/pins/{guildId}/{channelId}"), _defaultTimeout);

    public async Task<byte[]> GetPinsAsync(ulong guildId, ulong channelId, bool markdown)
        => await ProcessRequestWithFileAsync(() => HttpMethod.Get.ToRequest($"api/pins/{guildId}/{channelId}?markdown={markdown}"), TimeSpan.FromMinutes(5));

    public async Task<Dictionary<string, Cog>> GetSlashCommandsAsync()
        => (await ProcessRequestAsync<Dictionary<string, Cog>>(() => HttpMethod.Get.ToRequest("api/help/slashcommands"), _defaultTimeout))!;
}
