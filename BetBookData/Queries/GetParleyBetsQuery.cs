

using BetBookData.Models;
using MediatR;

namespace BetBookData.Queries;

public record GetParleyBetsQuery() : IRequest<IEnumerable<ParleyBetModel>>;
