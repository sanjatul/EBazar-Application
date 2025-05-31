using EBazar.API.Data;
using EBazar.API.Models.Domain;
using EBazar.API.Models.DTOs;
using EBazar.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EBazar.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _baseUrl;

        public ProductRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            var httpContext = httpContextAccessor.HttpContext;
            _baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
        }

        public async Task<Product> CreateAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product;
        }
        public async Task<Product?> GetProductQuantityAsync(int id)
        {
            var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return null; 
            }
            int totalInCarts = await _context.CartItems
                .Where(ci => ci.ProductId == id)
                .SumAsync(ci => ci.Quantity);
            int availableQuantity = product.Quantity - totalInCarts;
            product.Quantity = availableQuantity;
            return product;
        }


        public async Task<(List<ProductDto> products, int totalCount)> GetAllAsync(ProductQueryDto query)
        {
            var queryable = _context.Products
                   .GroupJoin(
                       _context.CartItems,
                       p => p.Id,
                       ci => ci.ProductId,
                       (p, cartItems) => new
                       {
                           Product = p,
                           CartQuantity = cartItems.Sum(ci => ci.Quantity)
                       })
                   .Select(x => new ProductDto
                   {
                       Id = x.Product.Id,
                       Name = x.Product.Name,
                       Slug = x.Product.Slug,
                       Image = x.Product.Image,
                       Price = x.Product.Price,
                       Discount = x.Product.Discount,
                       StartDate = x.Product.StartDate,
                       EndDate = x.Product.EndDate,
                       Quantity = x.Product.Quantity,
                       RemainingQuantity = x.Product.Quantity - x.CartQuantity
                   });

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                queryable = queryable.Where(p => p.Name.Contains(query.Search) || p.Slug.Contains(query.Search));
            }
            var totalCount = await queryable.CountAsync();

            queryable = query.SortBy?.ToLower() switch
            {
                "price" => query.SortOrder?.ToLower() == "desc"
                    ? queryable.OrderByDescending(p => p.Price)
                    : queryable.OrderBy(p => p.Price),
                "quantity" => query.SortOrder?.ToLower() == "desc"
                    ? queryable.OrderByDescending(p => p.Quantity)
                    : queryable.OrderBy(p => p.Quantity),
                _ => query.SortOrder?.ToLower() == "desc"
                    ? queryable.OrderByDescending(p => p.Id)
                    : queryable.OrderBy(p => p.Id)
            };

            var products = await queryable
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();
            foreach (var product in products)
            {
                product.Image = $"{_baseUrl}{product.Image}";
            }
            return (products, totalCount);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(p => p.Id == id);
        }
    }
}
