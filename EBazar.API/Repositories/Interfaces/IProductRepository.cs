using EBazar.API.Models.Domain;
using EBazar.API.Models.DTOs;

namespace EBazar.API.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> CreateAsync(Product product);
        Task<Product?> GetByIdAsync(int id);
        Task<Product?> GetProductQuantityAsync(int id);
        Task<(List<ProductDto> products, int totalCount)> GetAllAsync(ProductQueryDto query);
        Task<bool> ExistsAsync(int id);
    }
}
