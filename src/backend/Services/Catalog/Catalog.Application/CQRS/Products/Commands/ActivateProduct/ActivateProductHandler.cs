using Catalog.Application.Common.Interfaces;
using MediatR;
using System.Collections.Generic;

namespace Catalog.Application.CQRS.Products.Commands.ActivateProduct
{
    public class ActivateProductHandler : IRequestHandler<ActivateProductCommand, ActivateProductResult>
    {
        private readonly ICatalogDbContext _dbContext;

        public ActivateProductHandler(ICatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ActivateProductResult> Handle(ActivateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products.FindAsync(command.Id, cancellationToken);

            if (product == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy sản phẩm với Id: {command.Id}");
            }

            product.Activate();

            _dbContext.Products.Update(product);

            return new ActivateProductResult(true);
        }
    }
}