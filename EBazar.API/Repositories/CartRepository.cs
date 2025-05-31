using EBazar.API.Data;
using EBazar.API.Models.Domain;
using EBazar.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EBazar.API.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _baseUrl;

        public CartRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            var httpContext = httpContextAccessor.HttpContext;
            _baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
        }

        public async Task<Cart> CreateAsync()
        {
            var cart = new Cart();
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            foreach (var item in cart.Items)
            {
                if (!string.IsNullOrEmpty(item.Product?.Image))
                {
                    item.Product.Image = $"{_baseUrl}{item.Product.Image}";
                }
            }
            return cart;
        }

        public async Task<Cart?> GetCartAsync()
        {
            var carts=await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync();

            foreach (var item in carts.Items)
            {
                if (!string.IsNullOrEmpty(item.Product?.Image))
                {
                    item.Product.Image = $"{_baseUrl}{item.Product.Image}";
                }
            }
            return carts;
        }

        public async Task<CartItem?> GetCartItemAsync(int cartId, int productId)
        {
            var cartItem= await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
         
                if (!string.IsNullOrEmpty(cartItem.Product.Image))
                {
                cartItem.Product.Image = $"{_baseUrl}{cartItem.Product.Image}";
                }
            
            return cartItem;
        }

        public async Task<CartItem> AddCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
            cartItem.Product.Image = $"{_baseUrl}{cartItem.Product.Image}";
            return cartItem;
        }

        public async Task<CartItem> UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
            cartItem.Product.Image = $"{_baseUrl}{cartItem.Product.Image}";
            return cartItem;
        }

        public async Task DeleteCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
