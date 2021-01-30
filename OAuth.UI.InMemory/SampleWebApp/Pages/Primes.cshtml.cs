using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SampleWebApp.Services;

namespace MyApp.Namespace
{
    [Authorize]
    public class PrimesModel : PageModel
    {
        private readonly IPrimesService primesService;

        public int Number { get; set; }
        public int[] Factors { get; set; }

        public PrimesModel(IPrimesService primesService)
        {
            this.primesService = primesService;
        }

        public async Task OnGet(int number)
        {
            this.Number = number;
            this.Factors = await primesService.GetFactorsAsync(HttpContext, number);
        }
    }
}
