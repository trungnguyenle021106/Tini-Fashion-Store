namespace Basket.Domain.Entities
{
    public class ShoppingCart
    {
        public Guid UserId { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = new();

        public ShoppingCart()
        {
        }

        public ShoppingCart(Guid userId)
        {
            UserId = userId;
        }

        public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);
    }
}