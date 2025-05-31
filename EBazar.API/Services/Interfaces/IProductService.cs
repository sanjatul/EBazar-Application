using EBazar.API.Models.Common;
using EBazar.API.Models.DTOs;

namespace EBazar.API.Services.Interfaces
{
    public interface IProductService
    {
        Task<ApiResponse<ProductDto>> CreateProductAsync(CreateProductDto createProductDto);
        Task<ApiResponse<PaginatedResponse<ProductDto>>> GetProductsAsync(ProductQueryDto query);
    }
}
