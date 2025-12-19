using Carter;
using Identity.Application.CQRS.Auth.Commands.Logout;
using MediatR;

namespace Identity.API.Endpoints.Auth
{
    public class LogoutEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/logout", async (ISender sender, HttpContext context) =>
            {
                var refreshToken = context.Request.Cookies["refresh_token"];
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    var command = new LogoutCommand(refreshToken);
                    try
                    {
                        await sender.Send(command);
                    }
                    catch
                    {

                    }
                }

                context.Response.Cookies.Delete("access_token");
                context.Response.Cookies.Delete("refresh_token");

                return Results.Ok();
            })
            .WithName("Logout")
            .WithSummary("Revoke Refresh Token & Clear Cookies")
            .WithDescription("Logout user by revoking the refresh token and clearing HttpOnly cookies.");
        }
    }
}