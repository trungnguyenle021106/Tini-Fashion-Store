using Catalog.Application.Common.Interfaces;
using MediatR;

namespace Catalog.Application.CQRS.Products.Commands.RemoveProductStock
{
    public class RemoveProductStockHandler : IRequestHandler<RemoveProductStockCommand, RemoveProductStockResult>
    {
        private readonly ICatalogDbContext _dbContext;

        public RemoveProductStockHandler(ICatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RemoveProductStockResult> Handle(RemoveProductStockCommand command, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products.FindAsync(command.Id, cancellationToken);

            if (product == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy sản phẩm với Id: {command.Id}");
            }

            product.RemoveStock(command.Quantity);

            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new RemoveProductStockResult(true);
        }
    }
}