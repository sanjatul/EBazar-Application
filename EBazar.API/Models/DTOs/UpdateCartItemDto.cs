using System.ComponentModel.DataAnnotations;

namespace EBazar.API.Models.DTOs
{
    public class UpdateCartItemDto
    {
        [Required(ErrorMessage = "Product Id is required.")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be greater than or equal zero.")]
        public int Quantity { get; set; }
    }
}
