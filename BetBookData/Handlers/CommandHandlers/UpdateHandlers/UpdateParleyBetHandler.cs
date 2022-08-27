using BetBookData.Commands.UpdateCommands;
using BetBookData.Interfaces;
using BetBookData.Models;
using MediatR;

namespace BetBookData.Handlers.UpdateHandlers;

public class UpdateParleyBetHandler : IRequestHandler<UpdateParleyBetCommand, ParleyBetModel>
{
    private readonly IParleyBetData _parleyBetData;

    public UpdateParleyBetHandler(IParleyBetData parleyBetData)
    {
        _parleyBetData = parleyBetData;
    }

    public async Task<ParleyBetModel> Handle(
        UpdateParleyBetCommand request, CancellationToken cancellationToken)
    {
        await _parleyBetData.UpdateParleyBet(request.parleyBet);

        return request.parleyBet;
    }
}
