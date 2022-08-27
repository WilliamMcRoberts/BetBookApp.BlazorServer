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

    public async Task<TeamModel?> GetTeam(int id)
    {
        _logger.LogInformation("Get Team Call");

        var results = await _db.LoadData<TeamModel, dynamic>(
            "dbo.spTeams_Get", new
            {
                Id = id
            });

        return results.FirstOrDefault();
    }

    public async Task<int> InsertTeam(TeamModel team)
    {
        _logger.LogInformation("Insert Team Call");

        try
        {
            await _db.SaveData("dbo.spTeams_Insert", new
            {
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
            _logger.LogInformation(ex, "Failed To Insert Team"); 
        }

        return team.Id;
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

    public async Task DeleteTeam(int id)
    {
        _logger.LogInformation( "Delete Team Call");

        try
        {
            await _db.SaveData( "dbo.spTeams_Delete", new
            {
                Id = id
            });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex,"Failed To Delete Team");
        }
    }
}

#nullable restore
