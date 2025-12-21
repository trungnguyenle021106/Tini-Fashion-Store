using Basket.Application.CQRS.Basket.Commands.StoreBasket;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Endpoints.Basket
{
    public record StoreBasketRequest(string UserName, List<StoreBasketItemRequest> Items);

    public record StoreBasketItemRequest(
        int Quantity,
        decimal Price,
        string ProductId,
        string ProductName,
        string PictureUrl
    );

    public record StoreBasketResponse(string UserName);

    public class StoreBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async ([FromBody] StoreBasketRequest request, ISender sender) =>
            {
                var command = request.Adapt<StoreBasketCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<StoreBasketResponse>();

                return Results.Ok(response);
            })
            .WithName("StoreBasket")
            .WithSummary("Create or Update basket")
            .WithDescription("Stores the user's shopping cart in Redis. If it exists, it updates it.")
            .RequireAuthorization();
        }
    }
}