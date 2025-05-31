namespace EBazar.API.Models.DTOs
{
    public class ProductQueryDto
    {
        public string? Search { get; set; }
        public string? SortBy { get; set; } = "name";
        public string? SortOrder { get; set; } = "asc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public override string ToString()
        {
            return $"Search: '{Search}', SortBy: '{SortBy}', SortOrder: '{SortOrder}', Page: {Page}, PageSize: {PageSize}";
        }
    }
}
