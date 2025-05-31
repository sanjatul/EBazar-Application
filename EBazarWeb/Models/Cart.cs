namespace EBazarWeb.ViewModels
{
    public class Cart
    {

        public int Id { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal TotalAmount
        {
            get
            {
                return Items?.Sum(item => item.SubTotal) ?? 0;
            }
        }
    }
}
