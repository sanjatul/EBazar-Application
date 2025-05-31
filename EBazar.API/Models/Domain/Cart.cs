using System.ComponentModel.DataAnnotations;

namespace EBazar.API.Models.Domain
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
