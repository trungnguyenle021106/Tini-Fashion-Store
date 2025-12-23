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
            var basket = await _repository.GetBasketAsync(query.UserId, cancellationToken);
            if (basket == null)
            {
                basket = new ShoppingCart(query.UserId);
            }

            return new GetBasketResult(basket);
        }
    }
}