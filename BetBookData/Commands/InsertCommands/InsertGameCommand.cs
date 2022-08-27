using BetBookData.Models;
using MediatR;

namespace BetBookData.Commands.InsertCommands;

public record InsertGameCommand(GameModel game) : IRequest<GameModel>;
