using Basket.Application.CQRS.Basket.Commands.StoreBasket;
using FluentAssertions;

namespace UnitTests.BasketTests.ShoppingCartTest
{
    public class StoreBasketCommandValidatorTests
    {
        private readonly StoreBasketCommandValidator _validator = new();

        [Fact]
        public void Validator_ShouldHaveError_WhenQuantityIsZeroOrLess()
        {
            // Arrange
            var command = new StoreBasketCommand(Guid.NewGuid(), new List<CartItemDto>
        {
            new CartItemDto(0, 100, "prod-1", "Product 1", "url") // Quantity = 0
        });

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName.Contains("Quantity"));
        }
    }
}
