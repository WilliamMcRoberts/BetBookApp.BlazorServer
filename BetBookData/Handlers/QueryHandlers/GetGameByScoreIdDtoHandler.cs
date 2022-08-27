

using BetBookData.Dto;
using BetBookData.Interfaces;
using BetBookData.Queries;
using MediatR;

namespace BetBookData.Handlers.QueryHandlers;

public class GetGameByScoreIdDtoHandler : IRequestHandler<GetGameByScoreIdDtoQuery, GameByScoreIdDto>
{
    private readonly IGameService _gameService;

    public GetGameByScoreIdDtoHandler(IGameService gameService)
    {
        _gameService = gameService;
    }

    public async Task<GameByScoreIdDto> Handle(GetGameByScoreIdDtoQuery request, CancellationToken cancellationToken)
    {
        return await _gameService.GetGameByScoreId(request.scoreId);
    }
}
