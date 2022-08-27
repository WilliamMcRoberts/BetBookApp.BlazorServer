using BetBookData.Commands.UpdateCommands;
using BetBookData.Data;
using BetBookData.Interfaces;
using BetBookData.Models;
using MediatR;

namespace BetBookData.Handlers.UpdateHandlers;

public class UpdateTeamHandler : IRequestHandler<UpdateTeamCommand, TeamModel>
{
    private readonly ITeamData _teamData;

    public UpdateTeamHandler(ITeamData teamData)
    {
        _teamData = teamData;
    }

    public async Task<TeamModel> Handle(
        UpdateTeamCommand request, CancellationToken cancellationToken)
    {
        await _teamData.UpdateTeam(request.team);

        return request.team;
    }
}
