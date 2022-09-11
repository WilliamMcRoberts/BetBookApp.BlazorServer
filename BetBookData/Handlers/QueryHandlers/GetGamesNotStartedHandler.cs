

using BetBookData.Interfaces;
using BetBookData.Models;
using BetBookData.Queries;
using MediatR;

namespace BetBookData.Handlers.QueryHandlers;

public class GetGamesNotStartedHandler : IRequestHandler<GetGamesNotStartedQuery, IEnumerable<GameModel>>
{
    private readonly IGameData _gameData;

    public GetGamesNotStartedHandler(IGameData gameData)
    {
        _gameData = gameData;
    }

    public async Task<IEnumerable<GameModel>> Handle(GetGamesNotStartedQuery request, CancellationToken cancellationToken)
    {
        return await _gameData.GetAllGamesNotStarted();
    }
}
