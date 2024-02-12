using GrillBot.Core.Models.Pagination;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Diagnostics.Models;
using GrillBot.Core.Services.UserMeasures.Models.Dashboard;
using GrillBot.Core.Services.UserMeasures.Models.MeasuresList;
using GrillBot.Core.Services.UserMeasures.Models.User;

namespace GrillBot.Core.Services.UserMeasures;

public interface IUserMeasuresServiceClient : IClient
{
    Task<DiagnosticInfo> GetDiagAsync();
    Task<List<DashboardRow>> GetDashboardDataAsync();
    Task<int> GetItemsCountOfGuildAsync(string guildId);
    Task<RestResponse<PaginatedResponse<MeasuresItem>>> GetMeasuresListAsync(MeasuresListParams parameters);
    Task<UserInfo> GetUserInfoAsync(string guildId, string userId);
}
