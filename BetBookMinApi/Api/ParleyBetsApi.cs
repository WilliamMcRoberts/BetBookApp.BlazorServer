using BetBookData.Commands.InsertCommands;
using BetBookData.Commands.UpdateCommands;
using BetBookData.Models;
using BetBookData.Queries;
using MediatR;
using Serilog;

namespace BetBookMinApi.Api;

public static class ParleyBetsApi
{
    public static void ConfigureParleyBetsApi(this WebApplication app)
    {
        // Endpoint mappings
        app.MapGet("/ParleyBets", GetParleyBets).WithName("GetAllParleyBets").AllowAnonymous();
        app.MapPost("/ParleyBets", InsertParleyBet).WithName("InsertParleyBet");
        app.MapPut("/ParleyBets", UpdateParleyBet).WithName("UpdateParleyBet");
    }

    public static async Task<IResult> GetParleyBets(IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(new GetParleyBetsQuery()));
        }
        catch (Exception ex)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger(typeof(ParleyBetsApi));
            logger.LogInformation(ex, "Exception On Get Parley Bets");

            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> InsertParleyBet(
        ParleyBetModel parleyBet, IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(
                new InsertParleyBetCommand(parleyBet)));
        }
        catch (Exception ex)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger(typeof(ParleyBetsApi));
            logger.LogInformation(ex, "Exception On Insert Parley Bet");

            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> UpdateParleyBet(
        ParleyBetModel parleyBet, IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(
                new UpdateParleyBetCommand(parleyBet)));
        }
        catch (Exception ex)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger(typeof(ParleyBetsApi));
            logger.LogInformation(ex, "Exception On Update Parley Bet");

            return Results.Problem(ex.Message);
        }
    }
}
