using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace SampleAPI.Fixtures
{
    public class AuthorizedCustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        private class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
        {
            public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
                : base(options, logger, encoder, clock)
            {
            }

            protected override Task<AuthenticateResult> HandleAuthenticateAsync()
            {
                var identity = new ClaimsIdentity(new[] { new Claim("scope", "SampleAPI") }, "Test");
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, "Test");

                var result = AuthenticateResult.Success(ticket);

                return Task.FromResult(result);
            }
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddAuthentication(options => { options.DefaultAuthenticateScheme = nameof(TestAuthHandler); })
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(nameof(TestAuthHandler), options => { });
            });
        }
    }

    [TestClass]
    public class BaseFixtures
    {
        public static HttpClient httpclient;

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            var webApplicationFactory = new AuthorizedCustomWebApplicationFactory<Startup>();
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

    [TestClass]
    public class AuthorizationFixtures
    {
        private class UnauthorizedCustomWebApplicationFactory<TStartup>
            : WebApplicationFactory<TStartup> where TStartup : class
        {
        }

        public static HttpClient httpclient;

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            var webApplicationFactory = new UnauthorizedCustomWebApplicationFactory<Startup>();
            httpclient = webApplicationFactory.CreateClient();
        }


        [TestMethod]
        public async Task TestPrimesControllerFactorsActionReturnsUnauthorized()
        {
            var responseObject = await httpclient.GetAsync("primes/factors/42");

            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, responseObject.StatusCode);
        }
    }
}
