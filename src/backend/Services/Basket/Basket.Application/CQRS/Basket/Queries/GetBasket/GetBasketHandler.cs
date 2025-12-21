using Basket.Application.Common.Interfaces;
using Basket.Domain.Entities;
using MediatR;

namespace Basket.Application.CQRS.Basket.Queries.GetBasket
{
    public class GetBasketHandler : IRequestHandler<GetBasketQuery, GetBasketResult>
    {
        private readonly IBasketRepository _repository;

        public GetBasketHandler(IBasketRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
        {
            var basket = await _repository.GetBasketAsync(query.UserName, cancellationToken);
            if (basket == null)
            {
                basket = new ShoppingCart(query.UserName);
            }

            return new GetBasketResult(basket);
        }
    }
}