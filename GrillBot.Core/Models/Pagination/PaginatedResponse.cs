using Microsoft.EntityFrameworkCore;

namespace GrillBot.Core.Models.Pagination;

public class PaginatedResponse<TModel>
{
    public List<TModel> Data { get; set; } = new();
    public int Page { get; set; }
    public long TotalItemsCount { get; set; }

    public static async Task<PaginatedResponse<TEntity>> CreateWithEntityAsync<TEntity>(IQueryable<TEntity> query, PaginatedParams @params)
    {
        var result = new PaginatedResponse<TEntity>
        {
            Page = Math.Max(@params.Page, 0),
            TotalItemsCount = await query.CountAsync()
        };

        if (result.TotalItemsCount == 0)
            return result;

        query = query.Skip(@params.Skip()).Take(@params.PageSize);
        result.Data.AddRange(await query.ToListAsync());

        return result;
    }

    public static async Task<PaginatedResponse<TModel>> CopyAndMapAsync<TEntity>(PaginatedResponse<TEntity> resultWithEntity,
        Func<TEntity, Task<TModel>> converter)
    {
        var result = new PaginatedResponse<TModel>
        {
            Page = resultWithEntity.Page,
            TotalItemsCount = resultWithEntity.TotalItemsCount
        };

        foreach (var item in resultWithEntity.Data)
            result.Data.Add(await converter(item));

        return result;
    }

    public static PaginatedResponse<TModel> Create(List<TModel> data, PaginatedParams request)
    {
        return new PaginatedResponse<TModel>
        {
            Page = Math.Max(request.Page, 0),
            TotalItemsCount = data.Count,
            Data = data.Skip(request.Skip()).Take(request.PageSize).ToList()
        };
    }
}
