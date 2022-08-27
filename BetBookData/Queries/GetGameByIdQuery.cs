

using BetBookData.Models;
using MediatR;

namespace BetBookData.Queries;

public record GetGameByIdQuery(int id) : IRequest<GameModel>;
