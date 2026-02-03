using Basket.API.Endpoints.Basket;
using FluentAssertions;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Encodings.Web; 
using ApiProgram = Basket.API.Program;

namespace IntegrationTests.BasketTests.StoreBasketTests
{
    public class StoreBasketEndpointTests : IClassFixture<WebApplicationFactory<ApiProgram>>
    {
        private readonly HttpClient _client;

        private const string TEST_USER_ID = "9d365287-3932-4752-9f88-15967073286f";

        public StoreBasketEndpointTests(WebApplicationFactory<ApiProgram> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {

                builder.ConfigureTestServices(services =>
                {
                    services.RemoveAll(typeof(Microsoft.Extensions.Caching.Distributed.IDistributedCache));
                    services.AddDistributedMemoryCache();

                    services.AddMassTransitTestHarness();

                    services.Configure<AuthenticationOptions>(options =>
                    {
                        options.DefaultAuthenticateScheme = "TestScheme";
                        options.DefaultChallengeScheme = "TestScheme";
                    });

                    services.AddAuthentication("TestScheme")
                            .AddScheme<AuthenticationSchemeOptions, LocalTestAuthHandler>(
                                "TestScheme", options => { });
                });
            }).CreateClient();

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("TestScheme");
        }

        [Fact]
        public async Task StoreBasket_ShouldReturnOk_WhenRequestIsValid()
        {
            // Arrange
            var request = new StoreBasketRequest(new List<StoreBasketItemRequest>
            {
                new StoreBasketItemRequest(2, 150, "p-101", "Laptop", "url")
            });

            // Act
            var response = await _client.PostAsJsonAsync("/basket", request);

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK, because: content);
        }
    }

    public class LocalTestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public LocalTestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[] {
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.NameIdentifier, "9d365287-3932-4752-9f88-15967073286f"),
                new Claim("sub", "9d365287-3932-4752-9f88-15967073286f"),
                new Claim("id", "9d365287-3932-4752-9f88-15967073286f")
            };

            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "TestScheme");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}