using BetBookData.Commands.UpdateCommands;
using BetBookData.Interfaces;
using BetBookData.Models;
using MediatR;

namespace BetBookData.Handlers.UpdateHandlers;

public class UpdateBetHandler : IRequestHandler<UpdateBetCommand, BetModel>
{
    private readonly IBetData _betData;

    public UpdateBetHandler(IBetData betData)
    {
        _betData = betData;
    }

    public async Task<BetModel> Handle(
        UpdateBetCommand request, CancellationToken cancellationToken)
    {
        await _betData.UpdateBet(request.bet);

        return request.bet;
    }
}
