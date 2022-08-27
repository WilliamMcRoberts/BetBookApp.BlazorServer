using BetBookData.Commands.UpdateCommands;
using BetBookData.Models;
using BetBookData.Queries;
using MediatR;
using Serilog;

namespace BetBookMinApi.Api;

public static class HouseAccountApi
{
    public static void ConfigureHouseAccountApi(this WebApplication app)
    {
        // Endpoint mappings
        app.MapGet("/HouseAccount", GetHouseAccount).WithName("GetHouseAccount").AllowAnonymous();
        app.MapPut("/HouseAccount", UpdateHouseAccount).WithName("UpdateHouseAccount");
    }

    public static async Task<IResult> GetHouseAccount(IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(new GetHouseAccountQuery()));
        }
        catch (Exception ex)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger(typeof(HouseAccountApi));
            logger.LogInformation(ex, "Exception On Get House Account");

            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> UpdateHouseAccount(
        HouseAccountModel houseAccount, IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(
                new UpdateHouseAccountCommand(houseAccount)));
        }
        catch (Exception ex)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger(typeof(HouseAccountApi));
            logger.LogInformation(ex, "Exception On Update House Account");

            return Results.Problem(ex.Message);
        }
    }
}
