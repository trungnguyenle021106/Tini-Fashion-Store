using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.CQRS.Categories.Commands.CreateCategory
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryResult>
    {
        private readonly ICatalogDbContext _dbContext;

        public CreateCategoryHandler(ICatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateCategoryResult> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = new Category(command.Name);

            await _dbContext.Categories.AddAsync(category, cancellationToken);

            return new CreateCategoryResult(category.Id);
        }
    }
}