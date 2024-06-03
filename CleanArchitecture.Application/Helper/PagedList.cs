using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Helper
{
    public class PagedList<T>
    {
        private PagedList(List<T> items, int page, int pageSize, int totalCount)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public PagedList(List<T> items)
        {
            Items = items;
        }
        public List<T> Items { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public bool HasNextPage => Page * PageSize < TotalCount;
        public bool HasPreviousPage => Page > 1;

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int page, int pageSize)
        {
            int totalCount = await query.CountAsync();

            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new(items, page, pageSize, totalCount);
        }
        public static PagedList<T> CreateDto(List<T> items, int page, int pageSize, int totalCount)
        {
            
            return new(items, page, pageSize, totalCount);
        }
    }
}
