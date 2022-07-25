using BetBookData.Interfaces;
using BetBookData.Models;

namespace BetBookApi.Api;

public static class GamesApi
{
    /*********************************************************************
                       TODO - Finish Building All API's                            
    ************************************************************************/

    public static void ConfigureGamesApi(this WebApplication app)
    {
        // Endpoint mappings
        app.MapGet("/Games", GetGames);
        app.MapGet("/Games/{id}", GetGame);
        app.MapPost("/Games", InsertGame);
        app.MapPut("/Games", UpdateGame);
        app.MapDelete("/Games/{id}", DeleteGame);
    }

    public static async Task<IResult> GetGames(IGameData data)
    {
        try
        {
            return Results.Ok(await data.GetGames());
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> GetGame(int id, IGameData data)
    {
        try
        {
            return Results.Ok(await data.GetGame(id));
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> InsertGame(GameModel game, IGameData data)
    {
        try
        {
            await data.InsertGame(game);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> UpdateGame(GameModel game, IGameData data)
    {
        try
        {
            await data.UpdateGame(game);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> DeleteGame(int id, IGameData data)
    {
        try
        {
            await data.DeleteGame(id);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}
