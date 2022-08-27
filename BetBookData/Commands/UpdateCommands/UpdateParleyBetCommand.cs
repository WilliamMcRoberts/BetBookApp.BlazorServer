using BetBookData.Models;
using MediatR;

namespace BetBookData.Commands.UpdateCommands;

public record UpdateParleyBetCommand(ParleyBetModel parleyBet) : IRequest<ParleyBetModel>;
