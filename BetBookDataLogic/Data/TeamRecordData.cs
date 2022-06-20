using BetBookDataLogic.Data.Interfaces;
using BetBookDataLogic.DbAccess;
using BetBookDataLogic.Models;

namespace BetBookDataLogic.Data;
public class TeamRecordData : ITeamRecordData
{
    private readonly ISqlConnection _db;
    private readonly ITeamData _teamData;

    /// <summary>
    /// TeamRecordData Constructor
    /// </summary>
    /// <param name="db"></param>
    /// <param name="teamData"></param>
    public TeamRecordData(ISqlConnection db, ITeamData teamData)
    {
        _db = db;
        _teamData = teamData;
    }

    /// <summary>
    /// Method calls the spTeamRecords_GetAll stored procedure to retrieve 
    /// all team records in the database
    /// </summary>
    /// <returns>IEnumerable of UserModel</returns>
    public async Task<IEnumerable<TeamRecordModel>> GetTeamRecords()
    {
        return await _db.LoadData<TeamRecordModel, dynamic>(
        "dbo.spTeamRecords_GetAll", new { });
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
        string wins = "";
        string losses = "";
        string draws = "";
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

        TeamModel? team = await _teamData.GetTeam(teamRecord.TeamId);

        if (team is not null)
        {
            team.WinCount = teamRecord.Wins.ToList().Count;
            team.LossCount = teamRecord.Losses.ToList().Count;
            team.Drawcount = teamRecord.Draws.ToList().Count;

            await _teamData.UpdateTeam(team);
        }
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
