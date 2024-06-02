using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.RemindService.Models.Request;
using GrillBot.Core.Services.RemindService.Models.Response;

namespace GrillBot.Core.Services.RemindService;

public interface IRemindServiceClient : IClient
{
    Task<ProcessPendingRemindersResult> ProcessPendingRemindersAsync();
    Task CancelReminderAsync(CancelReminderRequest request);
    Task<PaginatedResponse<RemindMessageItem>> GetReminderListAsync(ReminderListRequest request);
    Task<CreateReminderResult> CreateReminderAsync(CreateReminderRequest request);
    Task<CreateReminderResult> CopyReminderAsync(CopyReminderRequest request);
    Task<List<ReminderSuggestionItem>> GetSuggestionsAsync(string userId);
}
