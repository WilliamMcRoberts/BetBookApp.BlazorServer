using BetBookData.Commands.InsertCommands;
using BetBookData.Commands.UpdateCommands;
using BetBookData.Models;
using BetBookData.Queries;
using MediatR;
using Serilog;

namespace BetBookMinApi.Api;

public static class BetsApi
{
    public static void ConfigureBetsApi(this WebApplication app)
    {
        // Endpoint mappings
        app.MapGet("/Bets", GetBets).WithName("GetAllBets").AllowAnonymous();
        app.MapPost("/Bets", InsertBet).WithName("InsertBet");
        app.MapPut("/Bets", UpdateBet).WithName("UpdateBet");
    }


    public static async Task<IResult> GetBets(IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(new GetBetsQuery()));
        }
        catch (Exception ex)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger(typeof(BetsApi));
            logger.LogInformation(ex, "Exception On Get Bets");

            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> InsertBet(BetModel bet, IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(
                new InsertBetCommand(bet)));
        }
        catch (Exception ex)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger(typeof(BetsApi));
            logger.LogInformation(ex, "Exception Insert Get Bet");

            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> UpdateBet(BetModel bet, IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(
                new UpdateBetCommand(bet)));
        }
        catch (Exception ex)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger(typeof(BetsApi));
            logger.LogInformation(ex, "Exception On Update Bet");

            return Results.Problem(ex.Message);
        }
    }
}
