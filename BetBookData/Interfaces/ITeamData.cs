using BetBookData.Models;

namespace BetBookData.Interfaces;

#nullable enable

public interface ITeamData
{
    Task<IEnumerable<TeamModel>> GetTeams();
    Task UpdateTeam(TeamModel team);
}

#nullable restore