using Basket.Application.CQRS.Basket.Commands.DeleteBasket;
using BuildingBlocks.Infrastructure.Extensions;
using Carter;
using Mapster;
using MediatR;
using System.Security.Claims;

namespace Basket.API.Endpoints.Basket
{
    public record DeleteBasketResponse(bool IsSuccess);

    public class DeleteBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket", async (ClaimsPrincipal user, ISender sender) =>
            {
                var userId = user.GetUserId();

                var command = new DeleteBasketCommand(userId);

                var result = await sender.Send(command);

                var response = result.Adapt<DeleteBasketResponse>();

                return Results.Ok(response);
            })
            .WithName("DeleteBasket")
            .WithSummary("Delete user basket")
            .WithDescription("Removes the shopping cart from Redis.")
            .RequireAuthorization();
        }
    }
}