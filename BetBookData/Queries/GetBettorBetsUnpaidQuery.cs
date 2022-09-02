using BetBookData.Models;
using MediatR;

namespace BetBookData.Queries;

public record GetBettorBetsUnpaidQuery(int userId) : IRequest<IEnumerable<BetModel>>;