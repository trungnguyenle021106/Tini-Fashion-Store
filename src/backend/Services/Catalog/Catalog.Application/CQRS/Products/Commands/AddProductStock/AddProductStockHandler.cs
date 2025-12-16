using Catalog.Application.Common.Interfaces;
using MediatR;

namespace Catalog.Application.CQRS.Products.Commands.AddProductStock
{
    public class AddProductStockHandler : IRequestHandler<AddProductStockCommand, AddProductStockResult>
    {
        private readonly ICatalogDbContext _dbContext;

        public AddProductStockHandler(ICatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AddProductStockResult> Handle(AddProductStockCommand command, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products.FindAsync(command.Id, cancellationToken);

            if (product == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy sản phẩm với Id: {command.Id}");
            }

            product.AddStock(command.Quantity);

            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new AddProductStockResult(true);
        }
    }
}