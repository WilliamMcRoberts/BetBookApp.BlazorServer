using BetBookData.Commands.InsertCommands;
using BetBookData.Commands.UpdateCommands;
using BetBookData.Models;
using BetBookData.Queries;
using MediatR;
using Serilog;

namespace BetBookMinApi.Api;

public static class GamesApi
{
    public static void ConfigureGamesApi(this WebApplication app)
    {
        // Endpoint mappings
        app.MapGet("/Games", GetGames).WithName("GetAllGames").AllowAnonymous();
        app.MapGet("/Games/{id}", GetGame).WithName("GetGameById").AllowAnonymous();
        app.MapPost("/Games", InsertGame).WithName("InsertGame").AllowAnonymous();
        app.MapPut("/Games", UpdateGame).WithName("UpdateGame").AllowAnonymous();

    }

    public static async Task<IResult> GetGames(IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(new GetGamesQuery()));
        }
        catch (Exception ex)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });
            var _logger = loggerFactory.CreateLogger(typeof(GamesApi));

            _logger.LogInformation(ex, "Exception On Get Games");
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> GetGame(int id, IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(new GetGameByIdQuery(id)));
        }
        catch (Exception ex)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger(typeof(GamesApi));
            logger.LogInformation(ex, "Exception On Get Game");

            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> InsertGame(GameModel game, IMediator mediator)
    {
        try
        {            
            return Results.Ok(await mediator.Send(
                new InsertGameCommand(game)));
        }
        catch (Exception ex)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger(typeof(GamesApi));
            logger.LogInformation(ex, "Exception On Insert Game");

            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> UpdateGame(GameModel game, IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(
                new UpdateGameCommand(game)));
        }
        catch (Exception ex)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger(typeof(GamesApi));
            logger.LogInformation(ex, "Exception On Update Game");

            return Results.Problem(ex.Message);
        }
    }
}
