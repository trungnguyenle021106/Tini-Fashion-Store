using Basket.Domain.Entities;
using FluentAssertions;

namespace UnitTests.BasketTests.ShoppingCartTest
{
    public class ShoppingCartTests
    {
        [Fact]
        public void TotalPrice_ShouldBeSumOfAllItems()
        {
            // Arrange
            var cart = new ShoppingCart(Guid.NewGuid());
            cart.Items.Add(new ShoppingCartItem { Price = 100, Quantity = 2 }); // 200
            cart.Items.Add(new ShoppingCartItem { Price = 50, Quantity = 1 });  // 50

            // Act
            var total = cart.TotalPrice;

            // Assert
            total.Should().Be(250);
        }
    }
}