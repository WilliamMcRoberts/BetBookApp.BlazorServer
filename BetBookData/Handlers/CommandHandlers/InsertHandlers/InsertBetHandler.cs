using BetBookData.Commands.InsertCommands;
using BetBookData.Interfaces;
using BetBookData.Models;
using MediatR;

namespace BetBookData.Handlers.InsertHandlers;

public class InsertBetHandler : IRequestHandler<InsertBetCommand, BetModel>
{
    private readonly IBetData _betData;

    public InsertBetHandler(IBetData betData)
    {
        _betData = betData;
    }

    public async Task<BetModel> Handle(
        InsertBetCommand request, CancellationToken cancellationToken)
    {
        var betId = await _betData.InsertBet(request.bet);

        request.bet.Id = betId;

        return request.bet;
    }
}
