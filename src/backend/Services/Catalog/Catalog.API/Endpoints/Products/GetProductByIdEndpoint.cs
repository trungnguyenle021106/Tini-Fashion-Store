using Carter;
using Catalog.Application.CQRS.Products.Queries.GetProductById;
using Catalog.Domain.Enums;
using Mapster;
using MediatR;

namespace Catalog.API.Endpoints.Products
{
    public record GetProductByIdResponse(
        Guid Id,
        string Name,
        decimal Price,
        string Description,
        string ImageUrl,
        ProductStatus Status,
        int Quantity,
        Guid CategoryId
    );

    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByIdQuery(id));

                var response = result.Product.Adapt<GetProductByIdResponse>();

                return Results.Ok(response);
            })
            .WithName("GetProductById")
            .WithSummary("Get product by Id")
            .WithDescription("Get detailed information of a specific product");
        }
    }
}