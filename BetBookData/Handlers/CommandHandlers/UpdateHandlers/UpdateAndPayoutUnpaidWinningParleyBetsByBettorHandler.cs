using BetBookData.Commands.UpdateCommands;
using BetBookData.Interfaces;
using MediatR;

namespace BetBookData.Handlers.CommandHandlers.UpdateHandlers;

public class UpdateAndPayoutUnpaidWinningParleyBetsByBettorHandler : IRequestHandler<UpdateAndPayoutUnpaidWinningParleyBetsByBettorCommand, bool>
{
    private readonly IParleyBetData _parleyData;

    public UpdateAndPayoutUnpaidWinningParleyBetsByBettorHandler(IParleyBetData parleyData)
    {
        _parleyData = parleyData;
    }

    public async Task<bool> Handle(
        UpdateAndPayoutUnpaidWinningParleyBetsByBettorCommand request, CancellationToken cancellationToken)
    {
        return await _parleyData.PayoutUnpaidWinningParleyBets(request.totalPendingParleyPayout, request.userId);
    }
}