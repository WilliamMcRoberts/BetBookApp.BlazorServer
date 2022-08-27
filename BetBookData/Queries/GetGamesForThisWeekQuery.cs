

using BetBookData.Dto;
using BetBookData.Models;
using MediatR;

namespace BetBookData.Queries;

public record GetGamesForThisWeekQuery(SeasonType season, int week) : IRequest<HashSet<GameModel>>;
