﻿using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Common.Attributes;
using GrillBot.Core.Services.RemindService.Models.Request;
using GrillBot.Core.Services.RemindService.Models.Response;
using Refit;

namespace GrillBot.Core.Services.RemindService;

[Service("Remind")]
public interface IRemindServiceClient : IServiceClient
{
    [Post("/api/remind/process-pending")]
    Task<ProcessPendingRemindersResult> ProcessPendingRemindersAsync(CancellationToken cancellationToken = default);

    [Put("/api/remind/cancel")]
    Task CancelReminderAsync(CancelReminderRequest request, CancellationToken cancellationToken = default);

    [Post("/api/remind/list")]
    Task<PaginatedResponse<RemindMessageItem>> GetReminderListAsync(ReminderListRequest request, CancellationToken cancellationToken = default);

    [Post("/api/remind/create")]
    Task<CreateReminderResult> CreateReminderAsync(CreateReminderRequest request, CancellationToken cancellationToken = default);

    [Post("/api/remind/copy")]
    Task<CreateReminderResult> CopyReminderAsync(CopyReminderRequest request, CancellationToken cancellationToken = default);

    [Post("/api/remind/suggestions/{userId}")]
    Task<List<ReminderSuggestionItem>> GetSuggestionsAsync(string userId, CancellationToken cancellationToken = default);

    [Post("/api/remind/postpone/{notificationMessageId}/{hours}")]
    Task PostponeRemindAsync(string notificationMessageId, int hours, CancellationToken cancellationToken = default);
}
