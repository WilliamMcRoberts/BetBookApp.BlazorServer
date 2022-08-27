
using BetBookData.Models;
using MediatR;

namespace BetBookData.Queries;

public record GetTeamsQuery() : IRequest<IEnumerable<TeamModel>>;

