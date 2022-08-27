

using BetBookData.Interfaces;
using BetBookData.Models;
using BetBookData.Queries;
using MediatR;

namespace BetBookData.Handlers.QueryHandlers;

public class GetGamesForThisWeekHandler : IRequestHandler<GetGamesForThisWeekQuery, HashSet<GameModel>>
{
    private readonly IGameService _gameService;

    public GetGamesForThisWeekHandler(IGameService gameService)
    {
        _gameService = gameService;
    }

    public async Task<HashSet<GameModel>> Handle(
        GetGamesForThisWeekQuery request, CancellationToken cancellationToken)
    {
        return await _gameService.GetGamesForThisWeek(request.season, request.week);
    }
}
