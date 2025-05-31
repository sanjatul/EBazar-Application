using System.ComponentModel.DataAnnotations;

namespace EBazar.API.Models.Domain
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [MaxLength(255)]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Slug is required.")]
        [MaxLength(255)]
        public string Slug { get; set; }
        
        [Required(ErrorMessage = "Product image is required.")]
        public string Image { get; set; }
        
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Discount must be greater than zero or equal.")]
        public decimal? Discount { get; set; }
        
        public DateTime? StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be greater than or equal 0.")]
        public int Quantity { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Discount.HasValue && Discount > 0)
            {
                if (!StartDate.HasValue)
                {
                    yield return new ValidationResult(
                        "Start date is required when a discount is applied.",
                        new[] { nameof(StartDate) });
                }

                if (!EndDate.HasValue)
                {
                    yield return new ValidationResult(
                        "End date is required when a discount is applied.",
                        new[] { nameof(EndDate) });
                }

                if (StartDate.HasValue && EndDate.HasValue && EndDate < StartDate)
                {
                    yield return new ValidationResult(
                        "End date must be greater than start date.",
                        new[] { nameof(EndDate) });
                }
            }
        }
    }
}
