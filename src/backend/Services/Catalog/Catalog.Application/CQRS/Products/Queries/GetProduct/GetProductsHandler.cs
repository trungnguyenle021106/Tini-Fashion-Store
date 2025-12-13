using Catalog.Application.Common.Interfaces;
using Catalog.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.CQRS.Products.Queries.GetProduct
{
    public class GetProductsHandler
         : IRequestHandler<GetProductsQuery, GetProductsResult>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetProductsHandler(IApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            var products = await _dbContext.Products.AsNoTracking().ToListAsync(cancellationToken);
            return new GetProductsResult(products);
        }
    }
}
