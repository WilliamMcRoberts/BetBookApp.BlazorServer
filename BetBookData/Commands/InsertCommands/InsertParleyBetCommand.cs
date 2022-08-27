using BetBookData.Models;
using MediatR;

namespace BetBookData.Commands.InsertCommands;

public record InsertParleyBetCommand(ParleyBetModel parleyBet) : IRequest<ParleyBetModel>;
