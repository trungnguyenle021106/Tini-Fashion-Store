namespace Basket.Domain.Entities
{
    public class ShoppingCart
    {
        public string UserName { get; set; } = default!;
        public List<ShoppingCartItem> Items { get; set; } = new();

        public ShoppingCart()
        {
        }

        public ShoppingCart(string userName)
        {
            UserName = userName;
        }

        public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);
    }
}