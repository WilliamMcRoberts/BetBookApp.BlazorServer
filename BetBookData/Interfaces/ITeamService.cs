using BetBookData.Dto;

namespace BetBookData.Interfaces;
public interface ITeamService
{
    Task<Team[]> GetAllTeams();
}
