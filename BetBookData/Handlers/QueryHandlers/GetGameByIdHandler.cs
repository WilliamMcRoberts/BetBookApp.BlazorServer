using BetBookData.Interfaces;
using BetBookData.Models;
using BetBookData.Queries;
using MediatR;

namespace BetBookData.Handlers.QueryHandlers;
public class GetGameByIdHandler : IRequestHandler<GetGameByIdQuery, GameModel>
{
    private readonly IGameData _gameData;

    public GetGameByIdHandler(IGameData gameData)
    {
        _gameData = gameData;
    }

    public async Task<GameModel> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
    {
        return await _gameData.GetGame(request.id);
    }
}
