using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EBazar.API.Models.Domain
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int Quantity { get; set; }
        [ForeignKey("Cart")]
        public int CartId { get; set; }
        public Cart Cart { get; set; }
    }
}
