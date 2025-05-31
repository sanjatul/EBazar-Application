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


        public IndexModel()
        {
        }

        public async Task OnGetAsync()
        {
            
        }
    }
    }