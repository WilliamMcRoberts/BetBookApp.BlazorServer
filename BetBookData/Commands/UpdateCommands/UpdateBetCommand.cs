using BetBookData.Models;
using MediatR;

namespace BetBookData.Commands.UpdateCommands;

public record UpdateBetCommand(BetModel bet) : IRequest<BetModel>;

