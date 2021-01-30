using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace SampleWebApp.Services
{
    public class PrimesService : IPrimesService
    {
        private readonly HttpClient httpClient;

        public PrimesService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<int[]> GetFactorsAsync(HttpContext httpContext, int number)
        {
            var accessToken = await httpContext.GetTokenAsync("access_token");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.GetAsync($"primes/factors/{number}");

            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<int[]>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return Array.Empty<int>();
            }
        }
    }
}