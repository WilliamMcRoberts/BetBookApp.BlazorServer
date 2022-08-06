using BetBookData.Lookups;

namespace BetBookData.Interfaces;
public interface ITeamService
{
    Task<Team[]> GetAllTeams();
}