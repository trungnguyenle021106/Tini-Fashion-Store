using BuildingBlocks.Core.Exceptions;
using Catalog.Application.Common.Interfaces;
using MediatR;

namespace Catalog.Application.CQRS.Products.Commands.UpdateProduct
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult>
    {
        private readonly ICatalogDbContext _dbContext;

        public UpdateProductHandler(ICatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products.FindAsync(command.Id, cancellationToken);

            if (product == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy sản phẩm với Id: {command.Id}");
            }

            product.UpdateDetails(
                command.Name,
                command.Price,
                command.Description,
                command.ImageUrl,
                command.CategoryId
            );
            _dbContext.Products.Update(product);

            return new UpdateProductResult(true);
        }
    }
}