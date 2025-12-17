using Catalog.Application.Common.Interfaces;
using MediatR;

namespace Catalog.Application.CQRS.Products.Commands.DiscontinueProduct
{
    public class DiscontinueProductHandler : IRequestHandler<DiscontinueProductCommand, DiscontinueProductResult>
    {
        private readonly ICatalogDbContext _dbContext;

        public DiscontinueProductHandler(ICatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DiscontinueProductResult> Handle(DiscontinueProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products.FindAsync(command.Id, cancellationToken);

            if (product == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy sản phẩm với Id: {command.Id}");
            }

            product.Discontinue();

            _dbContext.Products.Update(product);

            return new DiscontinueProductResult(true);
        }
    }
}