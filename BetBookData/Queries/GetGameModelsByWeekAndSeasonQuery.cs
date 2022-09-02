using BetBookData.Models;
using MediatR;

namespace BetBookData.Queries;

public record GetGameModelsByWeekAndSeasonQuery(int week, Season season) : IRequest<IEnumerable<GameModel>>;