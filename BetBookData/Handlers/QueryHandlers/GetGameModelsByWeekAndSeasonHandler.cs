using BetBookData.Interfaces;
using BetBookData.Models;
using BetBookData.Queries;
using MediatR;

namespace BetBookData.Handlers.QueryHandlers;

public class GetGameModelsByWeekAndSeasonHandler : IRequestHandler<GetGameModelsByWeekAndSeasonQuery, IEnumerable<GameModel>>
{
    private readonly IGameData _gameData;

    public GetGameModelsByWeekAndSeasonHandler(IGameData gameData)
    {
        _gameData = gameData;
    }

    public async Task<IEnumerable<GameModel>> Handle(
        GetGameModelsByWeekAndSeasonQuery request, CancellationToken cancellationToken)
    {
        return await _gameData.GetGamesByWeekAndSeason(request.week, request.season);
    }
}