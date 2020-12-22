using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SampleAPI.Fixtures
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services => { });
        }
    }

    [TestClass]
    public class BaseFixtures
    {
        public static HttpClient httpclient;

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            var webApplicationFactory = new CustomWebApplicationFactory<Startup>();
            httpclient = webApplicationFactory.CreateClient();
        }

        [DataTestMethod]
        [DataRow(0, new int[0])]
        [DataRow(1, new int[0])]
        [DataRow(2, new[] { 2 })]
        [DataRow(3, new[] { 3 })]
        [DataRow(25, new[] { 5, 5 })]
        [DataRow(42, new[] { 2, 3, 7 })]
        public async Task TestPrimesControllerFactorsActionReturnsOkForGoodInput(int n, int[] expectedFactors)
        {
            var responseObject = await httpclient.GetAsync($"primes/factors/{n}");

            Assert.AreEqual(System.Net.HttpStatusCode.OK, responseObject.StatusCode);

            var actualFactors = JsonSerializer.Deserialize<int[]>(await responseObject.Content.ReadAsStringAsync());

            CollectionAssert.AreEqual(expectedFactors, actualFactors);
        }

        [TestMethod]
        public async Task TestPrimesControllerFactorsActionReturnsBadRequestForBadInput()
        {
            var responseObject = await httpclient.GetAsync("primes/factors/-3");

            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, responseObject.StatusCode);
        }
    }
}
