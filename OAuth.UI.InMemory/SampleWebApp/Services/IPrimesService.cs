using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SampleWebApp.Services
{
    public interface IPrimesService
    {
        Task<int[]> GetFactorsAsync(HttpContext httpContext, int number);
    }
}