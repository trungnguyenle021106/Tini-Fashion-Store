using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Core.Infrastructure.Data;
using MediatR;

namespace Catalog.Application.CQRS.Behaviours
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse> 
    {
        private readonly IApplicationDbContext _dbContext; 

        public TransactionBehavior(IApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next();
            await _dbContext.SaveChangesAsync(cancellationToken);
            return response;
        }
    }
}
