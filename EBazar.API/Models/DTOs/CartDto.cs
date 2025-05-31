namespace EBazar.API.Models.DTOs
{
    public class CartDto
    {
        public int Id { get; set; }
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
        public decimal TotalAmount
        {
            get
            {
                return Items?.Sum(item => item.SubTotal) ?? 0;
            }
        }
    }

}
