using EBazar.API.Models.Domain;

namespace EBazar.API.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> CreateAsync();
        Task<Cart?> GetCartAsync();
        Task<CartItem?> GetCartItemAsync(int cartId, int productId);
        Task<CartItem> AddCartItemAsync(CartItem cartItem);
        Task<CartItem> UpdateCartItemAsync(CartItem cartItem);
        Task DeleteCartItemAsync(CartItem cartItem);
        Task SaveChangesAsync();
    }
}
