using BetBookData.Models;

namespace BetBookData.Interfaces;
public interface ITeamData
{
    Task DeleteTeam(int id);
    Task<TeamModel?> GetTeam(int id);
    Task<IEnumerable<TeamModel>> GetTeams();
    Task<int> InsertTeam(TeamModel team);
    Task UpdateTeam(TeamModel team);
}