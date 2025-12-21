using Basket.Application.CQRS.Basket.Queries.GetBasket;
using Carter;
using Mapster;
using MediatR;

namespace Basket.API.Endpoints.Basket
{
    public record ShoppingCartResponse(string UserName, List<ShoppingCartItemResponse> Items, decimal TotalPrice);

    public record ShoppingCartItemResponse(
        int Quantity,
        decimal Price,
        string ProductId,
        string ProductName,
        string PictureUrl
    );

    public record GetBasketResponse(ShoppingCartResponse Cart);

    public class GetBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var query = new GetBasketQuery(userName);

                var result = await sender.Send(query);

                var response = result.Adapt<GetBasketResponse>();

                return Results.Ok(response);
            })
            .WithName("GetBasket")
            .WithSummary("Get user basket")
            .WithDescription("Get shopping cart by user name (returns empty cart if not found).")
            .RequireAuthorization();
        }
    }
}