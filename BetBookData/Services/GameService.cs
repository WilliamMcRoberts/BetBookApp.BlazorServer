
using BetBookData.Dto;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using MediatR;

using BetBookData.Interfaces;

namespace BetBookData.Services;

#nullable enable

public class GameService : IGameService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;
    private readonly ILogger<GameService> _logger;
    private readonly IMediator _mediator;

    public GameService(IConfiguration config,
                       IHttpClientFactory httpClientFactory,
                       ILogger<GameService> logger,
                       IMediator mediator)
    {
        _config = config;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<GameByScoreIdDto> GetGameByScoreId(int scoreId)
    {
        GameByScoreIdDto? score = new();

        try
        {
            _logger.LogInformation("Http Get / Get Game By Score ID");

            var client = _httpClientFactory.CreateClient("sportsdata");

            score = await client.GetFromJsonAsync<GameByScoreIdDto>(
                    $"stats/json/BoxScoreByScoreIDV3/{scoreId}?key={_config.GetSection("SportsDataIO").GetSection("Key4").Value}");
        }

        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Http Get Failed...GetGameByScoreId()");
        }

        return score!;
    }

    public async Task<GameDto[]> GetGameDtoArrayByWeekAndSeason(int _week, Season _season)
    {
        GameDto[]? gameDtoArray = new GameDto[16];

        try
        {
            _logger.LogInformation("Http Get / Get Games By Week");
            var client = _httpClientFactory.CreateClient("sportsdata");

            gameDtoArray = await client.GetFromJsonAsync<GameDto[]>(
                    $"scores/json/ScoresByWeek/2022{_season}/{_week}?key={_config.GetSection("SportsDataIO").GetSection("Key4").Value}");
        }

        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Http Get Failed...GetGamesByWeek()");
        }

        return gameDtoArray!;
    }
}

#nullable restore
