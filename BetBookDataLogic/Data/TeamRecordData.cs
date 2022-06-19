using BetBookDataLogic.Data.Interfaces;
using BetBookDataLogic.DbAccess;
using BetBookDataLogic.Models;
using Microsoft.Extensions.Configuration;

namespace BetBookDataLogic.Data;
public class TeamRecordData : ITeamRecordData
{

    private readonly ISqlConnection _db;

    /// <summary>
    /// Constructor for UserData
    /// </summary>
    /// <param name="db"></param>
    public TeamRecordData(ISqlConnection db)
    {
        _db = db;
    }

    /// <summary>
    /// Method calls spTeams_Get stored procedure which retrieves the
    /// record of a team
    /// </summary>
    /// <param name="id"></param>
    /// <returns>TeamRecordModel</returns>
    public async Task<TeamRecordModel?> GetTeamRecord(int teamId)
    {
        var results = await _db.LoadData<TeamRecordModel, dynamic>(
            "dbo.spTeamRecords_Get", new
            {
                TeamId = teamId
            });

        return results.FirstOrDefault();
    }

    /// <summary>
    /// Method calls the spTeamRecords_Insert stored procedure to insert one 
    /// team record into the database
    /// </summary>
    /// <param name="teamRecord"></param>
    /// <returns></returns>
    public async Task InsertTeamRecord(int teamId)
    {
        var wins = "";
        var losses = "";
        var draws = "";
        await _db.SaveData("dbo.spTeamRecords_Insert", new
        {
            teamId,
            wins,
            losses,
            draws
        });
    }

    /// <summary>
    /// Method calls the spTeamRecords_Update stored procedure to update
    /// a team record
    /// </summary>
    /// <param name="teamRecord"></param>
    /// <returns></returns>
    public async Task UpdateTeamRecord(TeamRecordModel teamRecord)
    {
        await _db.SaveData("dbo.spTeamRecords_Update", new
        {
            teamRecord.TeamId,
            teamRecord.Wins,
            teamRecord.Losses,
            teamRecord.Draws
        });
    }

    /// <summary>
    /// Method calls the spTeamRecords_Delete stored procedure which deletes one team
    /// record from the database
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteTeamRecord(int teamId)
    {
        await _db.SaveData(
        "dbo.spTeamRecords_Delete", new
        {
            TeamId = teamId
        });
    }
}
