using AutoMapper;
using EBazar.API.Models.Common;
using EBazar.API.Models.Domain;
using EBazar.API.Models.DTOs;
using EBazar.API.Repositories.Interfaces;
using EBazar.API.Services.Interfaces;

namespace EBazar.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IProductRepository productRepository,
            IImageService imageService,
            IMapper mapper,
            ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _imageService = imageService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<ProductDto>> CreateProductAsync(CreateProductDto createProductDto)
        {
            try
            {
                var product = _mapper.Map<Product>(createProductDto);

                if (createProductDto.ImageFile != null)
                {
                    var allowedTypes = new[] { "image/jpeg", "image/png", "image/jpg", "image/webp" };
                    if (!allowedTypes.Contains(createProductDto.ImageFile.ContentType.ToLower()))
                    {
                        return ApiResponse<ProductDto>.ErrorResponse("Invalid image format. Only JPEG, PNG, JPG, and WEBP are allowed.");
                    }
                    product.Image = await _imageService.SaveImageAsync(createProductDto.ImageFile);
                }

                var createdProduct = await _productRepository.CreateAsync(product);
                var productDto = _mapper.Map<ProductDto>(createdProduct);

                _logger.LogInformation("Product created successfully with ID: {ProductId}", createdProduct.Id);
                return ApiResponse<ProductDto>.SuccessResponse(productDto, "Product created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return ApiResponse<ProductDto>.ErrorResponse("Failed to create product");
            }
        }

        public async Task<ApiResponse<PaginatedResponse<ProductDto>>> GetProductsAsync(ProductQueryDto query)
        {
            try
            {
                if (query.Page <= 0) query.Page = 1;
                if (query.PageSize <= 0 || query.PageSize > 100) query.PageSize = 10;

                var (products, totalCount) = await _productRepository.GetAllAsync(query);
                var productDtos = _mapper.Map<List<ProductDto>>(products);

                var response = new PaginatedResponse<ProductDto>
                {
                    Data = productDtos,
                    TotalRecords = totalCount,
                    Page = query.Page,
                    PageSize = query.PageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / query.PageSize)
                };

                return ApiResponse<PaginatedResponse<ProductDto>>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products");
                return ApiResponse<PaginatedResponse<ProductDto>>.ErrorResponse("Failed to retrieve products");
            }
        }
    }
}
