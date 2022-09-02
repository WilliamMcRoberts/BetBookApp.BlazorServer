using BetBookData.Commands.UpdateCommands;
using BetBookData.Interfaces;
using MediatR;

namespace BetBookData.Handlers.CommandHandlers.UpdateHandlers;

public class UpdateAndPayoutUnpaidPushParleyBetsByBettorHandler : IRequestHandler<UpdateAndPayoutUnpaidPushParleyBetsByBettorCommand, bool>
{
    private readonly IParleyBetData _parleyData;

    public UpdateAndPayoutUnpaidPushParleyBetsByBettorHandler(IParleyBetData parleyData)
    {
        _parleyData = parleyData;
    }

    public async Task<bool> Handle(
        UpdateAndPayoutUnpaidPushParleyBetsByBettorCommand request, CancellationToken cancellationToken)
    {
        return await _parleyData.PayoutUnpaidPushParleyBets(request.totalPendingParleyRefund, request.userId);
    }
}