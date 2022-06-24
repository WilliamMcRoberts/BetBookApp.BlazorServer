using BetBookData.DataLogic.Interfaces;
using BetBookData.DbAccess;
using BetBookData.Models;

namespace BetBookData.DataLogic;
public class TeamRecordData : ITeamRecordData
{
    private readonly ISqlConnection _db;
    private readonly ITeamData _teamData;

    /// <summary>
    ///     TeamRecordData Constructor
    /// </summary>
    /// <param name="db">
    ///     ISqlConnection represents access to the database
    /// </param>
    /// <param name="teamData">
    ///     ITeamData represents access to team data logic
    /// </param>
    public TeamRecordData(ISqlConnection db, ITeamData teamData)
    {
        _db = db;
        _teamData = teamData;
    }

    /// <summary>
    ///     Async method calls the spTeamRecords_GetAll stored procedure to retrieve 
    ///     all team records in the database
    /// </summary>
    /// <returns>
    ///     IEnumerable of UserModel represents all team records in the database
    /// </returns>
    public async Task<IEnumerable<TeamRecordModel>> GetTeamRecords()
    {
        return await _db.LoadData<TeamRecordModel, dynamic>(
        "dbo.spTeamRecords_GetAll", new { });
    }

    /// <summary>
    ///     Async method calls spTeams_Get stored procedure which retrieves the
    ///     record of a team
    /// </summary>
    /// <param name="id">
    ///     int represents the id of a team being retrieved from the database
    /// </param>
    /// <returns>
    ///     TeamRecordModel represents the team being retrieved from the database
    /// </returns>
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
    ///     Async method call the spTeamRecords_Insert stored
    ///     procedure to insert a team record into the database
    /// </summary>
    /// <param name="teamId">
    ///     int represents the team id of the team record being inserted
    ///     into the database
    /// </param>
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
    ///     Async method calls the spTeamRecords_Update stored procedure to update
    ///     a team record
    /// </summary>
    /// <param name="teamRecord">
    ///     TeamRecordModel represents the team record being updated in the database
    /// </param>
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
            team.WinCount = teamRecord.Wins.Split('|').Length - 1;
            team.LossCount = teamRecord.Losses.Split('|').Length - 1;
            team.Drawcount = teamRecord.Draws.Split('|').Length - 1;

            await _teamData.UpdateTeam(team);
        }
    }

    /// <summary>
    ///     Async method calls the spTeamRecords_Delete stored procedure which deletes one team
    ///     record from the database
    /// </summary>
    /// <param name="id">
    ///     int represents the team id of the team record being deleted from the database
    /// </param>
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
