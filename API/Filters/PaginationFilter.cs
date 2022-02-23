namespace API.Filters
{
    public class PaginationFilter
    {
        private const int _maxSizePage = 10;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PaginationFilter()
        {
            PageNumber = 1;
            PageSize = _maxSizePage;
        }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize > _maxSizePage ? _maxSizePage : pageSize;
        }
    }
}