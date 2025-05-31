using EBazar.API.Models.DTOs;
using EBazar.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBazar.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("/add-product")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto createProductDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { Success = false, Message = "Validation failed", Errors = errors });
            }

            var result = await _productService.CreateProductAsync(createProductDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return CreatedAtAction(nameof(GetProducts), new { id = result.Data.Id }, result);
        }

        [HttpGet("/get-products")]
        public async Task<IActionResult> GetProducts([FromQuery] ProductQueryDto query)
        {
            var result = await _productService.GetProductsAsync(query);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
