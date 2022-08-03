﻿using BetBookData.Interfaces;
using BetBookData.Models;

namespace BetBookApi.Api;

public static class ParleyBetsApi
{
    public static void ConfigureParleyBetsApi(this WebApplication app)
    {
        // Endpoint mappings
        app.MapGet("/ParleyBets", GetParleyBets);
        app.MapGet("/ParleyBets/{id}", GetParleyBet);
        app.MapPost("/ParleyBets", InsertParleyBet);
        app.MapPut("/ParleyBets", UpdateParleyBet);
        app.MapDelete("/ParleyBets/{id}", DeleteParleyBet);
    }

    public static async Task<IResult> GetParleyBets(IParleyBetData data)
    {
        try
        {
            return Results.Ok(await data.GetParleyBets());
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> GetParleyBet(int id, IParleyBetData data)
    {
        try
        {
            return Results.Ok(await data.GetParleyBet(id));
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> InsertParleyBet(ParleyBetModel parleyBet, IParleyBetData data)
    {
        try
        {
            await data.InsertParleyBet(parleyBet);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> UpdateParleyBet(ParleyBetModel parleyBet, IParleyBetData data)
    {
        try
        {
            await data.UpdateParleyBet(parleyBet);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> DeleteParleyBet(int id, IParleyBetData data)
    {
        try
        {
            await data.DeleteParleyBet(id);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}