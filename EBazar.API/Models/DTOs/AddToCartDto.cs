using System.ComponentModel.DataAnnotations;

namespace EBazar.API.Models.DTOs
{
    public class AddToCartDto
    {
        [Required(ErrorMessage = "Product Id is required.")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int Quantity { get; set; }
    }
}
