using BetBookData.Models;
using MediatR;

namespace BetBookData.Commands.InsertCommands;

public record InsertBetCommand(BetModel bet) : IRequest<BetModel>;
