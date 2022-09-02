using BetBookData.Models;
using MediatR;

namespace BetBookData.Queries;

public record GetInProgressParleyBetsQuery() : IRequest<IEnumerable<ParleyBetModel>>;