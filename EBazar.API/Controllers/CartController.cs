using EBazar.API.Models.DTOs;
using EBazar.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EBazar.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("/add-cart-item")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto addToCartDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { Success = false, Message = "Validation failed", Errors = errors });
            }

            var result = await _cartService.AddToCartAsync(addToCartDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("/update-cart-items")]
        public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemDto updateCartItemDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { Success = false, Message = "Validation failed", Errors = errors });
            }

            var result = await _cartService.UpdateCartItemAsync(updateCartItemDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("/carts")]
        public async Task<IActionResult> GetCart()
        {
            var result = await _cartService.GetCartAsync();

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
