namespace EBazar.API.Models.Common
{
    public class PaginatedResponse<T>
    {
        public List<T> Data { get; set; }
        public int TotalRecords { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
