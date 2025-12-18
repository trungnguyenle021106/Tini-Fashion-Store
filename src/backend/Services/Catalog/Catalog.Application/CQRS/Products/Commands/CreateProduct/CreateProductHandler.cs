using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;
using Mapster;
using MediatR;

namespace Catalog.Application.CQRS.Products.Commands.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
    {
        private readonly ICatalogDbContext _dbContext;

        public CreateProductHandler(ICatalogDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = request.Adapt<Product>();
            await this._dbContext.Products.AddAsync(product);

            return product.Adapt<CreateProductResult>();
        }
    }
}
