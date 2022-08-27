using BetBookData.Interfaces;
using BetBookData.Models;
using BetBookData.Queries;
using MediatR;

namespace BetBookData.Handlers.QueryHandlers;

public class GetTeamsHandler : IRequestHandler<GetTeamsQuery, IEnumerable<TeamModel>>
{
    private readonly ITeamData _teamData;

    public GetTeamsHandler(ITeamData teamData)
    {
        _teamData = teamData;
    }

    public async Task<IEnumerable<TeamModel>> Handle(GetTeamsQuery request, CancellationToken cancellationToken)
    {
        return await _teamData.GetTeams();
    }
}
