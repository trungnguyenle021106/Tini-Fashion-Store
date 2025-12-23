using Basket.Application.Common.Interfaces;
using Basket.Domain.Entities;
using MediatR;

namespace Basket.Application.CQRS.Basket.Commands.StoreBasket
{
    public class StoreBasketHandler : IRequestHandler<StoreBasketCommand, StoreBasketResult>
    {
        private readonly IBasketRepository _repository;

        public StoreBasketHandler(IBasketRepository repository)
        {
            _repository = repository;
        }

        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            var shoppingCart = new ShoppingCart(command.UserId);

            foreach (var item in command.Items)
            {
                shoppingCart.Items.Add(new ShoppingCartItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    PictureUrl = item.PictureUrl
                });
            }

           await _repository.UpdateBasketAsync(shoppingCart, cancellationToken);

            return new StoreBasketResult(shoppingCart.UserId);
        }
    }
}