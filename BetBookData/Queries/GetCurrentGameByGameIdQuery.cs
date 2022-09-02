using BetBookData.Models;
using MediatR;

namespace BetBookData.Queries;

public record GetCurrentGameByGameIdQuery(int gameId) : IRequest<GameModel>;