using BetBookData.Models;

namespace BetBookData.DataLogic.Interfaces;
public interface ITeamData
{
    Task DeleteTeam(int id);
    Task<TeamModel?> GetTeam(int id);
    Task<IEnumerable<TeamModel>> GetTeams();
    Task<int> InsertTeam(TeamModel team);
    Task UpdateTeam(TeamModel team);
}
