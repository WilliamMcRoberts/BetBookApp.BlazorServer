

using BetBookData.Models;
using MediatR;

namespace BetBookData.Queries;

public record GetGamesNotStartedQuery() : IRequest<IEnumerable<GameModel>>;
