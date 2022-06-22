using BetBookData.Models;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using BetBookData.DbAccess;
using BetBookData.DataLogic.Interfaces;

namespace BetBookData.DataLogic;

public class TeamData : ITeamData
{

    private readonly ISqlConnection _db;
    private readonly IConfiguration _config;

    /// <summary>
    ///     TeamData Constructor
    /// </summary>
    /// <param name="db">
    ///     ISqlConnection represents access to the database
    /// </param>
    /// <param name="config">
    ///     IConfiguration represents access to the configuration
    /// </param>
    public TeamData(ISqlConnection db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    /// <summary>
    ///     Async method calls the spTeams_GetAll stored procedure to retrieve 
    ///     all teams in the database
    /// </summary>
    /// <returns>
    ///     IEnumerable of TeamModel representing all teams in the database
    /// </returns>
    public async Task<IEnumerable<TeamModel>> GetTeams()
    {
        return await _db.LoadData<TeamModel, dynamic>(
        "dbo.spTeams_GetAll", new { });
    }

    /// <summary>
    ///     Async method calls spTeams_Get stored procedure which retrieves one 
    ///     team by team id
    /// </summary>
    /// <param name="id">
    ///     int represents the id of the team being retrieved from the database
    /// </param>
    /// <returns>
    ///     TeamModel represents the team being retrieved from the database
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
    ///     Async method calls the spTeams_Insert stored procedure to insert one team 
    ///     entry into the database
    /// </summary>
    /// <param name="team">
    ///     TeamModel represents the team being inserted into the database
    /// </param>
    /// <returns>
    ///     int represents the id of the team that was inserted into the database
    /// </returns>
    public async Task<int> InsertTeam(TeamModel team)
    {
        using IDbConnection connection = new System.Data.SqlClient
            .SqlConnection(_config.GetConnectionString("BetBookDB"));

        var p = new DynamicParameters();

        p.Add("@TeamName", team.TeamName);
        p.Add("@City", team.City);
        p.Add("@Stadium", team.Stadium);
        p.Add("@WinCount", team.WinCount);
        p.Add("@LossCount", team.LossCount);
        p.Add("@DrawCount", team.Drawcount);
        p.Add("@Id", 0, dbType: DbType.Int32,
            direction: ParameterDirection.Output);

        await connection.ExecuteAsync("dbo.spTeams_Insert", p, 
            commandType: CommandType.StoredProcedure);

        team.Id = p.Get<int>("@Id");

        return team.Id;
    }

    /// <summary>
    ///     Async method calls the spTeams_Update stored procedure to update a team
    /// </summary>
    /// <param name="team">
    ///     TeamModel represents the team being updated in the database
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
            team.WinCount,
            team.LossCount,
            team.Drawcount
        });
    }

    /// <summary>
    ///     Async method calls the spTeams_Delete stored procedure which deletes one team
    ///     entry in the database
    /// </summary>
    /// <param name="id">
    ///     int represents the id of the team being deleted from the database
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
