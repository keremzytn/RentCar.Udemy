using TS.MediatR;
using RentCarServer.Application.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RentCarServer.WebAPI;
using TS.Result;

namespace RentCarServer.WebAPI.Modules;

public static class AuthModule
{
    public static void MapAuthModule(this IEndpointRouteBuilder builder)
    {
        var app = builder.MapGroup("/auth");

        app.MapPost("/login",
        async (LoginCommand request, ISender sender, CancellationToken cancellationToken) =>
        {
            var res = await sender.Send(request, cancellationToken);
            return res.IsSuccessful ? Results.Ok(res) : Results.InternalServerError(res);
        })
        .Produces<Result<string>>();
    }
}