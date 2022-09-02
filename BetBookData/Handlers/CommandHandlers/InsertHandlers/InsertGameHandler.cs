using BetBookData.Commands.InsertCommands;
using BetBookData.Interfaces;
using BetBookData.Models;
using MediatR;

namespace BetBookData.Handlers.InsertHandlers;

public class InsertGameHandler : IRequestHandler<InsertGameCommand, GameModel>
{
    private readonly IGameData _gameData;

    public InsertGameHandler(IGameData gameData)
    {
        _gameData = gameData;
    }

    public async Task<GameModel> Handle(
        InsertGameCommand request, CancellationToken cancellationToken)
    {
        request.game.Id = await _gameData.InsertGame(request.game);

        return request.game;
    }
}
