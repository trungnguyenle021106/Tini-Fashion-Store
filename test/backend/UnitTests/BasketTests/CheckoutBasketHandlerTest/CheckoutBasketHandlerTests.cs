using Basket.Application.Common.Interfaces;
using Basket.Application.CQRS.Basket.Commands.CheckoutBasket;
using Basket.Domain.Entities;
using BuildingBlocks.Core.Enums;
using BuildingBlocks.Core.Messaging;
using FluentAssertions;
using MassTransit;
using Moq;


namespace UnitTests.BasketTests.CheckoutBasketHandlerTest
{
    public class CheckoutBasketHandlerTest
    {
        private readonly Mock<IBasketRepository> _repoMock;
        private readonly Mock<IPublishEndpoint> _publishEndpointMock;
        private readonly CheckoutBasketHandler _handler;

        public CheckoutBasketHandlerTest()
        {
            _repoMock = new Mock<IBasketRepository>();
            _publishEndpointMock = new Mock<IPublishEndpoint>();
            _handler = new CheckoutBasketHandler(_repoMock.Object, _publishEndpointMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldPublishEvent_And_DeleteBasket_WhenBasketExists()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // 1. Tạo giỏ hàng (không gán TotalPrice ở đây vì sẽ lỗi)
            var basket = new ShoppingCart(userId);

            // 2. Thêm item vào để TotalPrice tự động nhảy lên 100
            // (1 sản phẩm * giá 100 = Tổng 100)
            basket.Items.Add(new ShoppingCartItem
            {
                ProductId = Guid.NewGuid().ToString(),
                Quantity = 1,
                Price = 100
            });

            // Lúc này basket.TotalPrice đã tự động bằng 100.

            _repoMock.Setup(x => x.GetBasketAsync(userId, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(basket);

            var command = new CheckoutBasketCommand(userId, "test@mail.com", "User", "0909", "Street", Wards.PhuongAnNhon, "Note");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();

            // Kiểm tra logic dựa trên số tiền đã được tính toán từ Item
            _publishEndpointMock.Verify(x => x.Publish(
                It.Is<BasketCheckoutEvent>(e => e.UserId == userId && e.TotalPrice == 100), // Check đúng số 100
                It.IsAny<CancellationToken>()),
                Times.Once);

            _repoMock.Verify(x => x.DeleteBasketAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenBasketIsNull()
        {
            // Arrange
            _repoMock.Setup(x => x.GetBasketAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
         .Returns(Task.FromResult<ShoppingCart?>(null)); // Giả lập giỏ hàng rỗng

            var command = new CheckoutBasketCommand(Guid.NewGuid(), "email", "name", "phone", "st", Wards.PhuongAnNhon, "note");

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
