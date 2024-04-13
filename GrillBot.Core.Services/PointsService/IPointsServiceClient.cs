using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.PointsService.Enums;
using GrillBot.Core.Services.PointsService.Models;
using GrillBot.Core.Services.PointsService.Models.Channels;
using GrillBot.Core.Services.PointsService.Models.Users;

namespace GrillBot.Core.Services.PointsService;

public interface IPointsServiceClient : IClient
{
    Task<PaginatedResponse<TransactionItem>> GetTransactionListAsync(AdminListRequest request);
    Task<List<PointsChartItem>> GetChartDataAsync(AdminListRequest request);
    Task<List<BoardItem>> GetLeaderboardAsync(string guildId, int skip, int count, LeaderboardColumnFlag columns, LeaderboardSortOptions sortOptions);
    Task<int> GetLeaderboardCountAsync(string guildId);
    Task<MergeResult?> MergeValidTransctionsAsync();
    Task<PointsStatus> GetStatusOfPointsAsync(string guildId, string userId);
    Task<ImagePointsStatus?> GetImagePointsStatusAsync(string guildId, string userId);
    Task TransferPointsAsync(TransferPointsRequest request);
    Task<bool> ExistsAnyTransactionAsync(string guildId, string userId);
    Task<StatusInfo> GetStatusInfoAsync();
    Task<PaginatedResponse<UserListItem>> GetUserListAsync(UserListRequest request);
    Task<int> GetTransactionsCountForGuildAsync(string guildId);
    Task<UserInfo> GetUserInfoAsync(string guildId, string userId);
    Task<ChannelInfo> GetChannelInfoAsync(string guildId, string channelId);
}
