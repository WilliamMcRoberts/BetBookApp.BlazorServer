using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetBookData.Commands.UpdateCommands;
using BetBookData.Interfaces;
using BetBookData.Models;
using MediatR;

namespace BetBookData.Handlers.UpdateHandlers;

public class UpdateGameHandler : IRequestHandler<UpdateGameCommand, GameModel>
{
    private readonly IGameData _gameData;

    public UpdateGameHandler(IGameData gameData)
    {
        _gameData = gameData;
    }

    public async Task<GameModel> Handle(
        UpdateGameCommand request, CancellationToken cancellationToken)
    {
        await _gameData.UpdateGame(request.game);

        return request.game;
    }
}
