using EBazar.API.Models.Common;
using EBazar.API.Models.DTOs;

namespace EBazar.API.Services.Interfaces
{
    public interface ICartService
    {
        Task<ApiResponse<CartDto>> AddToCartAsync(AddToCartDto addToCartDto);
        Task<ApiResponse<CartDto>> UpdateCartItemAsync(UpdateCartItemDto updateCartItemDto);
        Task<ApiResponse<CartDto>> GetCartAsync();
    }
}
