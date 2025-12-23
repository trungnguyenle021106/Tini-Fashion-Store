using Basket.Application.Common.Interfaces;
using MediatR;

namespace Basket.Application.CQRS.Basket.Commands.DeleteBasket
{
    public class DeleteBasketHandler : IRequestHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        private readonly IBasketRepository _repository;

        public DeleteBasketHandler(IBasketRepository repository)
        {
            _repository = repository;
        }

        public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
        {
            await _repository.DeleteBasketAsync(command.UserId, cancellationToken);

            return new DeleteBasketResult(true);
        }
    }
}