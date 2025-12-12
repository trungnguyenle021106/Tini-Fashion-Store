using Catalog.Application.Data;
using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.CQRS.Products.Commands.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
    {
        private readonly IProductRepository _repository;

        public CreateProductHandler(IProductRepository productRepository)
        {
            this._repository = productRepository;
        }

        public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Product product = new Product(request.name, request.price, request.description, request.imageUrl, request.categoryId);
            await this._repository.AddAsync(product);

            return new CreateProductResult(
                    product.Id, 
                    product.Name,
                    product.Price, 
                    product.Description, 
                    product.ImageUrl, 
                    product.Status, 
                    product.Quantity, 
                    product.CategoryId
                );
        }
    }
}
