using BetBookData.Commands.UpdateCommands;
using BetBookData.Interfaces;
using MediatR;

namespace BetBookData.Handlers.CommandHandlers.UpdateHandlers;

public class UpdateAndPayoutUnpaidPushSingleBetsByBettorHandler : IRequestHandler<UpdateAndPayoutUnpaidPushBetsByBettorCommand, bool>
{
    private readonly IBetData _betData;

    public UpdateAndPayoutUnpaidPushSingleBetsByBettorHandler(IBetData betData)
    {
        _betData = betData;
    }

    public async Task<bool> Handle(
        UpdateAndPayoutUnpaidPushBetsByBettorCommand request, CancellationToken cancellationToken)
    {
        return await _betData.PayoutUnpaidPushBets(request.totalPendingRefund, request.userId);
    }
}