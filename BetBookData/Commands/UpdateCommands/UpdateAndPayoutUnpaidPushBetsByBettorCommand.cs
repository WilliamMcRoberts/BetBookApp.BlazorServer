using MediatR;

namespace BetBookData.Commands.UpdateCommands;

public record UpdateAndPayoutUnpaidPushBetsByBettorCommand(decimal totalPendingRefund, int userId) : IRequest<bool>;