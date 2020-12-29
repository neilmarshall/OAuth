using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace SampleClient.ClientCredentials
{
    class Program
    {
        private static async Task<string> GetAccessToken(string clientId, string clientSecret)
        {
            var tokenClient = new HttpClient();

            var tokenParams = new KeyValuePair<string, string>[]
            {
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
            };

            var tokenResponse = await tokenClient.PostAsync(
                "https://localhost:5002/connect/token",
                new FormUrlEncodedContent(tokenParams));

            var json = JsonDocument.Parse(await tokenResponse.Content.ReadAsStringAsync());

            return json.RootElement.GetProperty("access_token").GetString();
        }

        private static async Task<int[]> GetAPIResponse(string accessToken, int n)
        {
            var apiClient = new HttpClient();
            apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var apiResponse = await apiClient.GetAsync($"https://localhost:5001/primes/factors/{n}");

            return JsonSerializer.Deserialize<int[]>(await apiResponse.Content.ReadAsStringAsync());
        }

        static async Task Main(string[] args)
        {
            try
            {
                if (args.Length == 0 || !System.Int32.TryParse(args[0], out int n))
                {
                    throw new ArgumentException("Program requires an integer argument to be provided");
                }

                var token = await GetAccessToken("ClientCredentialsFlow", "ClientCredentialsFlowSecret");

                var factors = await GetAPIResponse(token, n);

                Console.WriteLine($"Prime factors of {n}:");
                foreach (var factor in factors)
                {
                    Console.WriteLine($"\t{factor}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
