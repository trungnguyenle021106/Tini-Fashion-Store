using Basket.Application.CQRS.Basket.Commands.CheckoutBasket;
using BuildingBlocks.Core.Enums;
using BuildingBlocks.Infrastructure.Extensions;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Basket.API.Endpoints.Basket
{
    public record CheckoutBasketRequest(
        string ReceiverName,
        string PhoneNumber,
        string Street,
        Wards Ward,
        string? Note
    );

    public record CheckoutBasketResponse(bool IsSuccess);

    public class CheckoutBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket/checkout", async (ClaimsPrincipal user, [FromBody] CheckoutBasketRequest request, ISender sender) =>
            {
                var userId = user.GetUserId();
                var email = user.GetEmail();

                var command = new CheckoutBasketCommand(
                    userId,
                    email,
                    request.ReceiverName,
                    request.PhoneNumber,
                    request.Street,
                    request.Ward,
                    request.Note
                );

                var result = await sender.Send(command);

                var response = result.Adapt<CheckoutBasketResponse>();

                return Results.Accepted("/basket/checkout", response);
            })
            .WithName("CheckoutBasket")
            .WithSummary("Checkout basket (COD)")
            .WithDescription("Checkout using strict UserAddress format.")
            .RequireAuthorization();
        }
    }
}