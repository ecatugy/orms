namespace Orms.Domain.Entities.Pagination
{
    public class PaginationFilter
    {
        public int PageNumber { init; get; }
        public int PageSize { init; get; }
        public PaginationFilter()
        {
            PageNumber = 1;
            PageSize = 10;
        }
        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
