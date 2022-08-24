using BetBookData.Interfaces;
using BetBookData.Models;
using Serilog;

namespace BetBookMinApi.Api;

public static class BetsApi
{
    public static void ConfigureBetsApi(this WebApplication app)
    {
        // Endpoint mappings
        app.MapGet("/Bets", GetBets).WithName("GetAllBets").AllowAnonymous();
        app.MapGet("/Bets/{id}", GetBet).WithName("GetBetById").AllowAnonymous();
        app.MapPost("/Bets", InsertBet).WithName("InsertBet");
        app.MapPut("/Bets", UpdateBet).WithName("UpdateBet");
        app.MapDelete("/Bets/{id}", DeleteBet).WithName("DeleteBet");
    }


    public static async Task<IResult> GetBets(IBetData data)
    {
        try
        {
            return Results.Ok(await data.GetBets());
        }
        catch (Exception ex)
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger(typeof(GamesApi));
            logger.LogInformation(ex, "Exception On Get Bets");

            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> GetBet(int id, IBetData data)
    {
        try
        {
            return Results.Ok(await data.GetBet(id));
        }
        catch (Exception ex)
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger(typeof(GamesApi));
            logger.LogInformation(ex, "Exception On Get Bet");

            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> InsertBet(BetModel bet, IBetData data)
    {
        try
        {
            await data.InsertBet(bet);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger(typeof(GamesApi));
            logger.LogInformation(ex, "Exception Insert Get Bet");

            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> UpdateBet(BetModel bet, IBetData data)
    {
        try
        {
            await data.UpdateBet(bet);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger(typeof(GamesApi));
            logger.LogInformation(ex, "Exception On Update Bet");

            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> DeleteBet(int id, IBetData data)
    {
        

        try
        {
            await data.DeleteBet(id);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger(typeof(GamesApi));
            logger.LogInformation(ex, "Exception On Delete Bet");

            return Results.Problem(ex.Message);
        }
    }
}
