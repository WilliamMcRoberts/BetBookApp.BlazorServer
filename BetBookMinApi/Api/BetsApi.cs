using BetBookData.Interfaces;
using BetBookData.Models;

namespace BetBookMinApi.Api;

public static class BetsApi
{
    public static void ConfigureBetsApi(this WebApplication app)
    {
        // Endpoint mappings
        app.MapGet("/Bets", GetBets);
        app.MapGet("/Bets/{id}", GetBet);
        app.MapPost("/Bets", InsertBet);
        app.MapPut("/Bets", UpdateBet);
        app.MapDelete("/Bets/{id}", DeleteBet);
    }

    public static async Task<IResult> GetBets(IBetData data)
    {
        try
        {
            return Results.Ok(await data.GetBets());
        }
        catch (Exception ex)
        {
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
            return Results.Problem(ex.Message);
        }
    }
}
