using Basket.Application.Common.Interfaces;
using Basket.Application.CQRS.Basket.Commands.StoreBasket;
using Basket.Domain.Entities;
using Moq;

namespace UnitTests.BasketTests.ShoppingCartTest
{
    public class StoreBasketHandlerTests
    {
        private readonly Mock<IBasketRepository> _repoMock;
        private readonly StoreBasketHandler _handler;

        public StoreBasketHandlerTests()
        {
            _repoMock = new Mock<IBasketRepository>();
            _handler = new StoreBasketHandler(_repoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCallRepository_WhenCommandIsValid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new StoreBasketCommand(userId, new List<CartItemDto>
        {
            new CartItemDto(1, 100, "p1", "Prod 1", "url")
        });

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            // Kiểm tra xem Repo có thực sự gọi hàm Update với đúng UserId không
            _repoMock.Verify(x => x.UpdateBasketAsync(
                It.Is<ShoppingCart>(s => s.UserId == userId),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
