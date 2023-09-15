using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Diagnostics.Models;
using GrillBot.Core.Services.PointsService.Enums;
using GrillBot.Core.Services.PointsService.Models;
using GrillBot.Core.Services.PointsService.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace GrillBot.Core.Services.PointsService;

public interface IPointsServiceClient : IClient
{
    Task<RestResponse<PaginatedResponse<TransactionItem>>> GetTransactionListAsync(AdminListRequest request);
    Task<RestResponse<List<PointsChartItem>>> GetChartDataAsync(AdminListRequest request);
    Task<DiagnosticInfo> GetDiagAsync();
    Task<RestResponse<List<BoardItem>>> GetLeaderboardAsync(string guildId, int skip, int count, LeaderboardColumnFlag columns, LeaderboardSortOptions sortOptions);
    Task<int> GetLeaderboardCountAsync(string guildId);
    Task<MergeResult?> MergeTransctionsAsync();
    Task<PointsStatus> GetStatusOfPointsAsync(string guildId, string userId);
    Task<PointsStatus> GetStatusOfExpiredPointsAsync(string guildId, string userId);
    Task<ImagePointsStatus?> GetImagePointsStatusAsync(string guildId, string userId);
    Task ProcessSynchronizationAsync(SynchronizationRequest request);
    Task<ValidationProblemDetails?> CreateTransactionAsync(TransactionRequest request);
    Task<ValidationProblemDetails?> CreateTransactionAsync(AdminTransactionRequest request);
    Task DeleteTransactionAsync(string guildId, string messageId);
    Task DeleteTransactionAsync(string guildId, string messageId, string reactionId);
    Task<ValidationProblemDetails?> TransferPointsAsync(TransferPointsRequest request);
    Task<bool> ExistsAnyTransactionAsync(string guildId, string userId);
    Task<StatusInfo> GetStatusInfoAsync();
    Task<PaginatedResponse<UserListItem>> GetUserListAsync(UserListRequest request);
}
