using Basket.Application.CQRS.Basket.Commands.DeleteBasket;
using Carter;
using Mapster;
using MediatR;

namespace Basket.API.Endpoints.Basket
{
    public record DeleteBasketResponse(bool IsSuccess);

    public class DeleteBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var command = new DeleteBasketCommand(userName);

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