using Catalog.Application.Data;
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

        public Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {

            throw new NotImplementedException();
        }
    }
}
