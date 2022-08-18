
using BetBookData.Interfaces;
using BetBookData.Models;

namespace BetBookMinApi.Api;

public static class TeamsApi
{
    public static void ConfigureTeamsApi(this WebApplication app)
    {
        // Endpoint mappings
        app.MapGet("/Teams", GetTeams).WithName("GetAllTeams").AllowAnonymous();
        app.MapGet("/Teams/{id}", GetTeam).WithName("GetTeamById").AllowAnonymous();
        app.MapPost("/Teams", InsertTeam).WithName("InsertTeam");
        app.MapPut("/Teams", UpdateTeam).WithName("UpdateTeam");
        app.MapDelete("/Teams/{id}", DeleteTeam).WithName("DeleteTeam");
    }

    public static async Task<IResult> GetTeams(ITeamData data)
    {
        try
        {
            return Results.Ok(await data.GetTeams());
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> GetTeam(int id, ITeamData data)
    {
        try
        {
            return Results.Ok(await data.GetTeam(id));
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> InsertTeam(TeamModel team, ITeamData data)
    {
        try
        {
            await data.InsertTeam(team);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> UpdateTeam(TeamModel team, ITeamData data)
    {
        try
        {
            await data.UpdateTeam(team);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> DeleteTeam(int id, ITeamData data)
    {
        try
        {
            await data.DeleteTeam(id);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}
