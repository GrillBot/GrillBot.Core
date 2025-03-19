using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Common.Attributes;
using GrillBot.Core.Services.UserMeasures.Models.Dashboard;
using GrillBot.Core.Services.UserMeasures.Models.Measures;
using GrillBot.Core.Services.UserMeasures.Models.User;
using Refit;

namespace GrillBot.Core.Services.UserMeasures;

[Service("UserMeasures")]
public interface IUserMeasuresServiceClient : IServiceClient
{
    [Get("/api/dashboard")]
    Task<List<DashboardRow>> GetDashboardDataAsync();

    [Get("/api/info/count/{guildId}")]
    Task<int> GetItemsCountOfGuildAsync(string guildId);

    [Post("/api/measures/list")]
    Task<PaginatedResponse<MeasuresItem>> GetMeasuresListAsync(MeasuresListParams parameters);

    [Get("/api/user/{guildId}/{userId}")]
    Task<UserInfo> GetUserInfoAsync(string guildId, string userId);

    [Delete("/api/measures")]
    Task DeleteMeasureAsync([Query] DeleteMeasuresRequest request);
}
