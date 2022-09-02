using BetBookData.Models;
using MediatR;

namespace BetBookData.Queries;

public record GetBetsOnCurrentGameQuery(int gameId) : IRequest<IEnumerable<BetModel>>;