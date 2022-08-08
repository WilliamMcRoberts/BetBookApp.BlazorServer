using BetBookData.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using BetBookDbAccess;
using BetBookData.Interfaces;
using Dapper;

namespace BetBookData.Data;

public class TeamData : ITeamData
{
    private readonly ISqlConnection _db;

    /// <summary>
    /// TeamData Constructor
    /// </summary>
    /// <param name="db">ISqlConnection represents SqlConnection class interface</param>
    /// <param name="config">IConfiguration represents Configuration class interface</param>
    public TeamData(ISqlConnection db)
    {
        _db = db;
    }

    /// <summary>
    /// Async method calls the spTeams_GetAll stored procedure to retrieve 
    /// all teams in the database
    /// </summary>
    /// <returns>
    /// IEnumerable of TeamModel representing all teams in the database
    /// </returns>
    public async Task<IEnumerable<TeamModel>> GetTeams() =>
        await _db.LoadData<TeamModel, dynamic>( "dbo.spTeams_GetAll", new { });

    /// <summary>
    /// Async method calls spTeams_Get stored procedure which retrieves one 
    /// team by team id
    /// </summary>
    /// <param name="id">
    /// int represents the id of the team being retrieved from the database
    /// </param>
    /// <returns>
    /// TeamModel represents the team being retrieved from the database
    /// </returns>
    public async Task<TeamModel?> GetTeam(int id)
    {
        var results = await _db.LoadData<TeamModel, dynamic>(
            "dbo.spTeams_Get", new
            {
                Id = id
            });

        return results.FirstOrDefault();
    }

    /// <summary>
    /// Async method calls the spTeams_Insert stored procedure to insert one team 
    /// entry into the database
    /// </summary>
    /// <param name="team">
    /// TeamModel represents the team being inserted into the database
    /// </param>
    /// <returns>
    /// int represents the id of the team that was inserted into the database
    /// </returns>
    public async Task<int> InsertTeam(TeamModel team)
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

        return team.Id;
    }

    /// <summary>
    /// Async method calls the spTeams_Update stored procedure to update a team
    /// </summary>
    /// <param name="team">
    /// TeamModel represents the team being updated in the database
    /// </param>
    /// <returns></returns>
    public async Task UpdateTeam(TeamModel team)
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

    /// <summary>
    /// Async method calls the spTeams_Delete stored procedure which deletes one team
    /// entry in the database
    /// </summary>
    /// <param name="id">
    /// int represents the id of the team being deleted from the database
    /// </param>
    /// <returns></returns>
    public async Task DeleteTeam(int id)
    {
        await _db.SaveData(
        "dbo.spTeams_Delete", new
        {
            Id = id
        });
    }
}
