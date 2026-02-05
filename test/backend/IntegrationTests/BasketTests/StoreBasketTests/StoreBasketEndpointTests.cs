using Basket.API.Endpoints.Basket;
using FluentAssertions;
using IntegrationTests.Common; 
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace IntegrationTests.BasketTests.StoreBasketTests
{
    public class StoreBasketEndpointTests : BaseIntegrationTest
    {
        public StoreBasketEndpointTests(CustomWebApplicationFactory factory) : base(factory)
        {
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
            // Client đã có sẵn từ Base class và đã có Auth Header
            var response = await Client.PostAsJsonAsync("/basket", request);

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK, because: content);
        }
    }
}