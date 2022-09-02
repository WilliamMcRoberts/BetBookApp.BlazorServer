using MediatR;

namespace BetBookData.Commands.UpdateCommands;

public record UpdateAndPayoutUnpaidPushParleyBetsByBettorCommand(decimal totalPendingParleyRefund, int userId) : IRequest<bool>;