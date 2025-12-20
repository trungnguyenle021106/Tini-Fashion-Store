using Catalog.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.CQRS.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, GetCategoryByIdResult>
    {
        private readonly ICatalogDbContext _dbContext;

        public GetCategoryByIdHandler(ICatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetCategoryByIdResult> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == query.Id, cancellationToken);

            if (category == null)
            {
                throw new KeyNotFoundException($"Category with Id {query.Id} not found.");
            }
            var categoryDto = new CategoryDto(category.Id, category.Name);

            return new GetCategoryByIdResult(categoryDto);
        }
    }
}