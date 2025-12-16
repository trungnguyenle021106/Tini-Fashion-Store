using BuildingBlocks.Core.CQRS;
using Catalog.Domain.Entities;

namespace Catalog.Application.CQRS.Products.Queries.GetProduct
{
    public record GetProductsResult(PaginatedResult<Product> Products);

    public record GetProductsQuery(
        int PageNumber = 1,
        int PageSize = 10,
        string? Keyword = null
    ) : IQuery<GetProductsResult>;
}