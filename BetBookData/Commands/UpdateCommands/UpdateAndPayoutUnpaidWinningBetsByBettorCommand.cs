using MediatR;

namespace BetBookData.Commands.UpdateCommands;

public record UpdateAndPayoutUnpaidWinningBetsByBettorCommand(decimal totalPendingPayout, int userId) : IRequest<bool>;