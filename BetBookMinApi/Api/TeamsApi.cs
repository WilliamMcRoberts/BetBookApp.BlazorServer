
using BetBookData.Commands.UpdateCommands;
using BetBookData.Models;
using BetBookData.Queries;
using MediatR;
using Serilog;

namespace BetBookMinApi.Api;

public static class TeamsApi
{
    public static void ConfigureTeamsApi(this WebApplication app)
    {
        // Endpoint mappings
        app.MapGet("/Teams", GetTeams).WithName("GetAllTeams").AllowAnonymous();
        app.MapPut("/Teams", UpdateTeam).WithName("UpdateTeam");
    }

    public static async Task<IResult> GetTeams(IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(
                new GetTeamsQuery()));
        }
        catch (Exception ex)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger(typeof(TeamsApi));
            logger.LogInformation(ex, "Exception On Get Teams");

            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> UpdateTeam(TeamModel team, IMediator mediator)
    {
        try
        {
            return Results.Ok(await mediator.Send(
                new UpdateTeamCommand(team)));
        }
        catch (Exception ex)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog();
            });

            var logger = loggerFactory.CreateLogger(typeof(TeamsApi));
            logger.LogInformation(ex, "Exception On Update Team");

            return Results.Problem(ex.Message);
        }
    }
}
