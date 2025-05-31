using System.ComponentModel.DataAnnotations;

namespace EBazarWeb.ViewModels
{
    public class ProductVM
    {
        public string Name { get; set; }

        public string Slug { get; set; }

        public decimal Price { get; set; }

        public decimal? Discount { get; set; } = 0;

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int Quantity { get; set; }

        public string? Image { get; set; }
    }
}
