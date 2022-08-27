

using BetBookData.Models;
using MediatR;

namespace BetBookData.Queries;

public record GetGamesQuery() : IRequest<IEnumerable<GameModel>>;

