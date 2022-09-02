using BetBookData.Commands.UpdateCommands;
using BetBookData.Interfaces;
using MediatR;

namespace BetBookData.Handlers.CommandHandlers.UpdateHandlers;

public class UpdateAndPayoutUnpaidWinningSingleBetsByBettorHandler : IRequestHandler<UpdateAndPayoutUnpaidWinningBetsByBettorCommand, bool>
{
    private readonly IBetData _betData;

    public UpdateAndPayoutUnpaidWinningSingleBetsByBettorHandler(IBetData betData)
    {
        _betData = betData;
    }

    public async Task<bool> Handle(UpdateAndPayoutUnpaidWinningBetsByBettorCommand request, CancellationToken cancellationToken)
    {
        return await _betData.PayoutUnpaidWinningBets(request.totalPendingPayout, request.userId);
    }
}