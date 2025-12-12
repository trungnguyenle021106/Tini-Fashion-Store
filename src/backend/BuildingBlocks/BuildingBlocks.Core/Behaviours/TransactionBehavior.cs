using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Core.Infrastructure.Interfaces;
using MediatR;

namespace Catalog.Application.CQRS.Behaviours
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse> 
    {
        private readonly IUnitOfWork _unitOfWork; 

        public TransactionBehavior(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return response;
        }
    }
}
