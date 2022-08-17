using BetBookData.Interfaces;
using BetBookData.Models;
using Microsoft.AspNetCore.Authorization;

namespace BetBookMinApi.Api;

public static class GamesApi
{
    public static void ConfigureGamesApi(this WebApplication app)
    {
        // Endpoint mappings
        app.MapGet("/Games", GetGames).WithName("GetAllGames").AllowAnonymous();
        app.MapGet("/Games/{id}", GetGame).WithName("GetGameById");
        app.MapPost("/Games", InsertGame).WithName("InsertGame");
        app.MapPut("/Games", UpdateGame).WithName("UpdateGame");
        app.MapDelete("/Games/{id}", DeleteGame).WithName("DeleteGame");
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
