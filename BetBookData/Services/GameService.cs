
using BetBookData.Interfaces;
using BetBookData.Dto;
using BetBookData.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using MediatR;
using BetBookData.Queries;
using BetBookData.Commands.InsertCommands;

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

    public async Task<GameByTeamDto> GetGameByTeam(TeamModel team)
    {
        GameByTeamDto? game = new();

        try
        {
            _logger.LogInformation("Http Get / Get Game By Team");
            var client = _httpClientFactory.CreateClient("sportsdata");

            game = await client.GetFromJsonAsync<GameByTeamDto>(
                    $"odds/json/TeamTrends/{team.Symbol}?key={_config.GetSection("SportsDataIO").GetSection("Key3").Value}");

        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Http Get Failed...GetGameByTeam()");
        }

        return game!;
    }

    public async Task<GameByScoreIdDto> GetGameByScoreId(int scoreId)
    {
        GameByScoreIdDto? game = new();

        try
        {
            _logger.LogInformation("Http Get / Get Game By Score ID");

            var client = _httpClientFactory.CreateClient("sportsdata");

            game = await client.GetFromJsonAsync<GameByScoreIdDto>(
                    $"stats/json/BoxScoreByScoreIDV3/{scoreId}?key={_config.GetSection("SportsDataIO").GetSection("Key3").Value}");
        }

        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Http Get Failed...GetGameByScoreId()");
        }

        return game!;
    }

    public async Task<Game[]> GetGamesByWeek(SeasonType currentSeason, int week)
    {
        Game[]? games = new Game[16];

        try
        {
            _logger.LogInformation("Http Get / Get Games By Week");
            var client = _httpClientFactory.CreateClient("sportsdata");

            games = await client.GetFromJsonAsync<Game[]>(
                    $"scores/json/ScoresByWeek/2022{currentSeason}/{week}?key={_config.GetSection("SportsDataIO").GetSection("Key3").Value}");
        }

        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Http Get Failed...GetGamesByWeek()");
        }

        return games!;
    }

    public async Task<HashSet<GameModel>> GetGamesForThisWeek(
        SeasonType currentSeason, int currentWeek)
    {
        IEnumerable<GameModel> games =
            await _mediator.Send(new GetGamesQuery());
        IEnumerable<TeamModel> teams =
            await _mediator.Send(new GetTeamsQuery());

        HashSet<GameModel> thisWeekGames = games.Where(g =>
            g.GameStatus != GameStatus.FINISHED && g.WeekNumber == currentWeek)
            .ToHashSet<GameModel>();

        Game[] gameArray = new Game[16];

        gameArray = await GetGamesByWeek(
            currentSeason, currentWeek);

        foreach (Game game in gameArray)
        {
            if (game.PointSpread is null)
                continue;

            if (thisWeekGames.Contains(games.Where(g =>
                g.ScoreId == game.ScoreID).FirstOrDefault()!))
                continue;

            TeamModel? homeTeam = teams.Where(t =>
                t.Symbol == game.HomeTeam).FirstOrDefault();
            TeamModel? awayTeam = teams.Where(t =>
                t.Symbol == game.AwayTeam).FirstOrDefault();

            if (homeTeam is null || awayTeam is null)
                continue;

            string dateOfGameOnly = game.DateTime.ToString("MM-dd");
            string timeOfGameOnly = game.DateTime.ToString("hh:mm");

            GameModel gameModel = new()
            {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                HomeTeamId = homeTeam.Id,
                AwayTeamId = awayTeam.Id,
                DateOfGame = game.DateTime,
                DateOfGameOnly = dateOfGameOnly,
                TimeOfGameOnly = timeOfGameOnly,
                GameStatus = GameStatus.NOT_STARTED,
                WeekNumber = game.Week,
                PointSpread = Math.Round(Convert.ToDouble(game.PointSpread), 1),
                Stadium = game.StadiumDetails.Name,
                Season = currentSeason,
                ScoreId = game.ScoreID
            };

            try
            {
                thisWeekGames.Add(gameModel);
                await _mediator.Send(new InsertGameCommand(gameModel));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed On Game Insertion...GetGamesForThisWeek()");
            }
        }

        return thisWeekGames;
    }
}

#nullable restore
