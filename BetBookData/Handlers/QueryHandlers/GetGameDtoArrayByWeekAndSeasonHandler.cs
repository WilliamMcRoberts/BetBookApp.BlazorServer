using BetBookData.Dto;
using BetBookData.Interfaces;
using BetBookData.Queries;
using MediatR;

namespace BetBookData.Handlers.QueryHandlers;

public class GetGameDtoArrayByWeekAndSeasonHandler : IRequestHandler<GetGameDtoArrayByWeekAndSeasonQuery, GameDto[]>
{
    private readonly IGameService _gameService;

    public GetGameDtoArrayByWeekAndSeasonHandler(IGameService gameService)
    {
        _gameService = gameService;
    }

    public async Task<GameDto[]> Handle(GetGameDtoArrayByWeekAndSeasonQuery request, CancellationToken cancellationToken)
    {
        return await _gameService.GetGameDtoArrayByWeekAndSeason(request.week, request.season);
    }
}