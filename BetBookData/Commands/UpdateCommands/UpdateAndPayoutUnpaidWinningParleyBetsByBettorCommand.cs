using MediatR;

namespace BetBookData.Commands.UpdateCommands;

public record UpdateAndPayoutUnpaidWinningParleyBetsByBettorCommand(decimal totalPendingParleyPayout, int userId) : IRequest<bool>;