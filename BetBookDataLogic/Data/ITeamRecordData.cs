using BetBookDataLogic.Models;

namespace BetBookDataLogic.Data;
public interface ITeamRecordData
{
    Task DeleteTeamRecord(int teamId);
    Task<TeamRecordModel?> GetTeamRecord(int teamId);
    Task InsertTeamRecord(int teamId);
    Task UpdateTeamRecord(TeamRecordModel teamRecord);
}
