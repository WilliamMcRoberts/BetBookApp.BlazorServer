using BetBookData.Models;
using MediatR;

namespace BetBookData.Queries;

public record GetBettorParleyBetsUnpaidQuery(int userId) : IRequest<IEnumerable<ParleyBetModel>>;