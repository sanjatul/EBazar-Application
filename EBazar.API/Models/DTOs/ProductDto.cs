namespace EBazar.API.Models.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Quantity { get; set; }
        
    }
}
