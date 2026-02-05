using Basket.API.Endpoints.Basket;
using Basket.Application.CQRS.Basket.Commands.StoreBasket; // Import namespace StoreBasket
using BuildingBlocks.Core.Enums;
using BuildingBlocks.Core.Messaging;
using FluentAssertions;
using IntegrationTests.Common;
using IntegrationTests.Helpers;
using MassTransit.Testing; // Import namespace TestHarness
using Microsoft.Extensions.DependencyInjection; // Để dùng GetTestHarness
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.BasketTests.CheckoutBasketIntegrationTests
{
    public class CheckoutBasketIntegrationTests : BaseIntegrationTest
    {
        private readonly ITestHarness _harness;

        public CheckoutBasketIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
        {
            _harness = factory.Services.GetTestHarness();
        }

        [Fact]
        public async Task Checkout_ShouldReturnAccepted_And_PublishEvent()
        {
            // Arrange
            // 1. Tạo sẵn giỏ hàng (Seeding Data) để checkout không bị lỗi "Empty Basket"
            var productId = Guid.NewGuid().ToString();
            var seedBasketRequest = new StoreBasketRequest(new List<StoreBasketItemRequest>
            {
                new StoreBasketItemRequest(2, 100, productId, "Test Product", "url")
            });

            // Gọi API StoreBasket để lưu giỏ hàng vào Memory Cache
            var storeResponse = await Client.PostAsJsonAsync("/basket", seedBasketRequest);
            storeResponse.EnsureSuccessStatusCode();

            // 2. Chuẩn bị request checkout
            var checkoutRequest = new CheckoutBasketRequest(
                ReceiverName: "User A",
                PhoneNumber: "0909999999",
                Street: "Street 1",
                Ward: Wards.PhuongAnNhon,
                Note: "Fast pls");

            // Act
            var response = await Client.PostAsJsonAsync("/basket/checkout", checkoutRequest);

            // Assert
            // 1. Kiểm tra API trả về 202 Accepted
            response.StatusCode.Should().Be(HttpStatusCode.Accepted);

            // 2. Kiểm tra Event được bắn vào Bus
            // Lưu ý: Chờ một chút để event kịp publish (TestHarness xử lý bất đồng bộ)
            var published = await _harness.Published.Any<BasketCheckoutEvent>();
            published.Should().BeTrue("Event BasketCheckoutEvent phải được bắn ra sau khi checkout thành công.");
        }
    }
}