using Carter;
using Catalog.API.Commons.Models;
using Catalog.Application.CQRS.Categories.Queries.GetCategoryById;
using Mapster;
using MediatR;

namespace Catalog.API.Endpoints.Categories
{
    public class GetCategoryByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/categories/{id}", async (Guid id, ISender sender) =>
            {
                var query = new GetCategoryByIdQuery(id);

                var result = await sender.Send(query);

                var response = result.Category.Adapt<CategoryResponse>();

                return Results.Ok(response);
            })
            .WithName("GetCategoryById")
            .WithSummary("Get category by Id")
            .WithDescription("Retrieve details of a specific category.");
        }
    }
}