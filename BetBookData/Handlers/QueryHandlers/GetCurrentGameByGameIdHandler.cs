using BetBookData.Interfaces;
using BetBookData.Models;
using BetBookData.Queries;
using MediatR;

namespace BetBookData.Handlers.QueryHandlers;

public class GetCurrentGameByGameIdHandler : IRequestHandler<GetCurrentGameByGameIdQuery, GameModel>
{
    private readonly IGameData _gameData;

    public GetCurrentGameByGameIdHandler(IGameData gameData)
    {
        _gameData = gameData;
    }

    public async Task<GameModel> Handle(GetCurrentGameByGameIdQuery request, CancellationToken cancellationToken)
    {
        return await _gameData.GetCurrentGameByGameId(request.gameId);
    }
}