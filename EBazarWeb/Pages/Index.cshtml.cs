using EBazar.API.Models.Common;
using EBazarWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace EBazarWeb.Pages
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public Product product { get; set; } = new Product();
        public ApiResponse<Cart> cart { get; set; } = new();

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            await GetCartItemsAsync();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                await GetCartItemsAsync();
                return Page();
            }

            return RedirectToPage("/Index");
        }

        private async Task GetCartItemsAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://localhost:7213/carts");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<Cart>>(
                        json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    ) ?? new();
                    cart = apiResponse;
                }
                else
                {
                    _logger.LogWarning("Failed to fetch cart items. Status Code: {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while fetching cart items.");
            }
        }

    }
    }