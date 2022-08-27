using BetBookData.Interfaces;
using BetBookData.Models;
using BetBookData.Queries;
using MediatR;

namespace BetBookData.Handlers.QueryHandlers;

public class GetGamesHandler : IRequestHandler<GetGamesQuery, IEnumerable<GameModel>>
{
    private readonly IGameData _gameData;

    public GetGamesHandler(IGameData gameData)
    {
        _gameData = gameData;
    }

    public async Task<IEnumerable<GameModel>> Handle(
        GetGamesQuery request, CancellationToken cancellationToken)
    {
        return await _gameData.GetGames();
    }
}
