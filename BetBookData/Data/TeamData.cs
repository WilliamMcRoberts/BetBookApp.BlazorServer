using BetBookData.Models;
using BetBookData.Interfaces;
using Microsoft.Extensions.Logging;
using BetBookDbAccess;

namespace BetBookData.Data;

#nullable enable

public class TeamData : ITeamData
{
    private readonly ISqlConnection _db;
    private readonly ILogger<TeamData> _logger;

    public TeamData(ISqlConnection db, ILogger<TeamData> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<TeamModel>> GetTeams()
    {
        _logger.LogInformation( "Get Teams Call");

        return await _db.LoadData<TeamModel, dynamic>(
            "dbo.spTeams_GetAll", new { });
    }

    public async Task UpdateTeam(TeamModel team)
    {
        _logger.LogInformation( "Update Team Call");

        try
        {
            await _db.SaveData("dbo.spTeams_Update", new
            {
                team.Id,
                team.TeamName,
                team.City,
                team.Stadium,
                team.Wins,
                team.Losses,
                team.Draws,
                team.Symbol,
                team.Division,
                team.Conference
            });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex,"Failed To Update Team");
        }
    }
}

#nullable restore
