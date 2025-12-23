using Basket.Application.CQRS.Basket.Commands.StoreBasket;
using BuildingBlocks.Infrastructure.Extensions;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Basket.API.Endpoints.Basket
{
    public record StoreBasketRequest(List<StoreBasketItemRequest> Items);

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
            app.MapPost("/basket", async (ClaimsPrincipal user, [FromBody] StoreBasketRequest request, ISender sender) =>
            {
                var userID = user.GetUserId();
                var command = new StoreBasketCommand(userID, request.Items.Adapt<List<CartItemDto>>());

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