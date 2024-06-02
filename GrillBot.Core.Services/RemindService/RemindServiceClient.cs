using GrillBot.Core.Managers.Performance;
using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Common.Extensions;
using GrillBot.Core.Services.RemindService.Models.Request;
using GrillBot.Core.Services.RemindService.Models.Response;

namespace GrillBot.Core.Services.RemindService;

public class RemindServiceClient : RestServiceBase, IRemindServiceClient
{
    private static readonly TimeSpan _infiniteTimeout = Timeout.InfiniteTimeSpan;
    private static readonly TimeSpan _defaultTimeout = TimeSpan.FromMinutes(1);

    public override string ServiceName => "RemindService";

    public RemindServiceClient(ICounterManager counterManager, IHttpClientFactory httpClientFactory) : base(counterManager, httpClientFactory)
    {
    }

    public async Task<ProcessPendingRemindersResult> ProcessPendingRemindersAsync()
        => (await ProcessRequestAsync<ProcessPendingRemindersResult>(() => HttpMethod.Post.ToRequest("api/remind/process-pending"), _infiniteTimeout))!;

    public Task CancelReminderAsync(CancelReminderRequest request)
        => ProcessRequestAsync(() => HttpMethod.Put.ToRequest("api/remind/cancel", request), _defaultTimeout);

    public async Task<PaginatedResponse<RemindMessageItem>> GetReminderListAsync(ReminderListRequest request)
        => (await ProcessRequestAsync<PaginatedResponse<RemindMessageItem>>(() => HttpMethod.Post.ToRequest("api/remind/list", request), _defaultTimeout))!;

    public async Task<CreateReminderResult> CreateReminderAsync(CreateReminderRequest request)
        => (await ProcessRequestAsync<CreateReminderResult>(() => HttpMethod.Post.ToRequest("api/remind/create", request), _defaultTimeout))!;

    public async Task<CreateReminderResult> CopyReminderAsync(CopyReminderRequest request)
        => (await ProcessRequestAsync<CreateReminderResult>(() => HttpMethod.Post.ToRequest("api/remind/copy", request), _defaultTimeout))!;

    public async Task<List<ReminderSuggestionItem>> GetSuggestionsAsync(string userId)
        => (await ProcessRequestAsync<List<ReminderSuggestionItem>>(() => HttpMethod.Get.ToRequest($"api/remind/suggestions/{userId}"), _defaultTimeout))!;
}
