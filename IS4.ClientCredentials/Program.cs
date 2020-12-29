using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace IS4.ClientCredentials
{
    class Program
    {
        private static async Task<string> GetAccessToken(string clientId, string clientSecret)
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5002");
            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "ClientCredentialsFlow",
                ClientSecret = "ClientCredentialsFlowSecret"
            });

            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error);
            }

            return tokenResponse.AccessToken;
        }

        private static async Task<int[]> GetAPIResponse(string accessToken, int n)
        {
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(accessToken);

            var response = await apiClient.GetAsync($"https://localhost:5001/primes/factors/{n}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error in HTTP Response: {response.StatusCode}");
            }
            else
            {
                return JsonSerializer.Deserialize<int[]>(await response.Content.ReadAsStringAsync());
            }
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
