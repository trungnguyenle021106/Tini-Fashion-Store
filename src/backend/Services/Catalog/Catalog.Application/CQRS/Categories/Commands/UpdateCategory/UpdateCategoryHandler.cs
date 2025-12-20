using Catalog.Application.Common.Interfaces;
using MediatR;

namespace Catalog.Application.CQRS.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, UpdateCategoryResult>
    {
        private readonly ICatalogDbContext _dbContext;

        public UpdateCategoryHandler(ICatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UpdateCategoryResult> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories
                .FindAsync(new object[] { command.Id }, cancellationToken);

            if (category == null)
            {
                throw new KeyNotFoundException($"Category with Id {command.Id} not found.");
            }

            category.UpdateName(command.Name);

            _dbContext.Categories.Update(category);

            return new UpdateCategoryResult(true);
        }
    }
}