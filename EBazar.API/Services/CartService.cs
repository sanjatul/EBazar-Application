using AutoMapper;
using EBazar.API.Models.Common;
using EBazar.API.Models.Domain;
using EBazar.API.Models.DTOs;
using EBazar.API.Repositories.Interfaces;
using EBazar.API.Services.Interfaces;

namespace EBazar.API.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CartService> _logger;

        public CartService(
            ICartRepository cartRepository,
            IProductRepository productRepository,
            IMapper mapper,
            ILogger<CartService> logger)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<CartDto>> AddToCartAsync(AddToCartDto addToCartDto)
        {
            try
            {
                
                if (addToCartDto.Quantity < 1)
                {
                    return ApiResponse<CartDto>.ErrorResponse("Quantity must be greater than zero");
                }

                Product? product = await _productRepository.GetByIdAsync(addToCartDto.ProductId);
                if (product == null)
                {
                    return ApiResponse<CartDto>.ErrorResponse("Product not found");
                }

                Product? productQuantity =await _productRepository.GetProductQuantityAsync(addToCartDto.ProductId);
                if (productQuantity is null)
                {
                    return ApiResponse<CartDto>.ErrorResponse("Product not found");
                }

                if (productQuantity?.Quantity == 0)
                {
                    return ApiResponse<CartDto>.ErrorResponse("Product is out of stock");
                }

                if (productQuantity?.Quantity < addToCartDto.Quantity)
                {
                    return ApiResponse<CartDto>.ErrorResponse($"Only {productQuantity?.Quantity} items available in stock");
                }

                var cart = await _cartRepository.GetCartAsync();
                if (cart == null)
                {
                    cart= await _cartRepository.CreateAsync();
                }
                var existingCartItem = await _cartRepository.GetCartItemAsync(cart.Id, addToCartDto.ProductId);

                if (existingCartItem != null)
                {
                    var newQuantity = existingCartItem.Quantity + addToCartDto.Quantity;
                    if (newQuantity > productQuantity?.Quantity)
                    {
                        return ApiResponse<CartDto>.ErrorResponse($"Cannot add {addToCartDto.Quantity} items. Only {productQuantity?.Quantity - existingCartItem.Quantity} more items available");
                    }

                    existingCartItem.Quantity = newQuantity;
                    await _cartRepository.UpdateCartItemAsync(existingCartItem);
                }
                else
                {
                    var cartItem = new CartItem
                    {
                        CartId = cart.Id,
                        ProductId = addToCartDto.ProductId,
                        Quantity = addToCartDto.Quantity
                    };
                    await _cartRepository.AddCartItemAsync(cartItem);
                }

                var updatedCart = await _cartRepository.GetCartAsync();
                var cartDto = _mapper.Map<CartDto>(updatedCart);

                _logger.LogInformation("Product {ProductId} added to cart {CartId}", addToCartDto.ProductId, cart.Id);
                return ApiResponse<CartDto>.SuccessResponse(cartDto, "Product added to cart successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product to cart");
                return ApiResponse<CartDto>.ErrorResponse("Failed to add product to cart");
            }
        }

        public async Task<ApiResponse<CartDto>> UpdateCartItemAsync(UpdateCartItemDto updateCartItemDto)
        {
            try
            {
                var cart = await _cartRepository.GetCartAsync();
                if (cart == null)
                {
                    return ApiResponse<CartDto>.ErrorResponse("Cart not found");
                }

                var cartItem = await _cartRepository.GetCartItemAsync(cart.Id, updateCartItemDto.ProductId);
                if (cartItem == null)
                {
                    return ApiResponse<CartDto>.ErrorResponse("Product not found in cart");
                }

                if (updateCartItemDto.Quantity == 0)
                {
                    await _cartRepository.DeleteCartItemAsync(cartItem);
                    _logger.LogInformation("Product {ProductId} removed from cart {CartId}", updateCartItemDto.ProductId, cart.Id);
                }
                else if (updateCartItemDto.Quantity > 0)
                {
                    var product = await _productRepository.GetByIdAsync(updateCartItemDto.ProductId);
                    if (product != null && updateCartItemDto.Quantity > product.Quantity)
                    {
                        return ApiResponse<CartDto>.ErrorResponse($"Only {product.Quantity} items available in stock");
                    }

                    cartItem.Quantity = updateCartItemDto.Quantity;
                    await _cartRepository.UpdateCartItemAsync(cartItem);
                    _logger.LogInformation("Cart item quantity updated for product {ProductId} in cart {CartId}", updateCartItemDto.ProductId, cart.Id);
                }
                else
                {
                    return ApiResponse<CartDto>.ErrorResponse("Quantity cannot be negative");
                }

                var updatedCart = await _cartRepository.GetCartAsync();
                var cartDto = _mapper.Map<CartDto>(updatedCart);

                return ApiResponse<CartDto>.SuccessResponse(cartDto, "Cart updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart item");
                return ApiResponse<CartDto>.ErrorResponse("Failed to update cart item");
            }
        }

        public async Task<ApiResponse<CartDto>> GetCartAsync()
        {
            try
            {
                var cart = await _cartRepository.GetCartAsync();
                if (cart == null)
                {
                    return ApiResponse<CartDto>.ErrorResponse("No Items Available");
                    //return ApiResponse<CartDto>.SuccessResponse(new { });
                }

                var cartDto = _mapper.Map<CartDto>(cart);
                return ApiResponse<CartDto>.SuccessResponse(cartDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cart");
                return ApiResponse<CartDto>.ErrorResponse("Failed to retrieve cart");
            }
        }
    }
}
