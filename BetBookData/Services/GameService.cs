
using BetBookData.Interfaces;
using BetBookData.Dto;
using BetBookData.Models;
using BetBookData.Helpers;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace BetBookData.Services;

#nullable enable

public class GameService : IGameService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IGameData _gameData;
    private readonly ITeamData _teamData;
    private readonly IBetData _betData;
    private readonly IParleyBetData _parleyData;
    private readonly IConfiguration _config;
    private readonly ILogger<GameService> _logger;
    IEnumerable<TeamModel>? teams;
    IEnumerable<GameModel>? games;
    IEnumerable<BetModel>? bets;
    IEnumerable<ParleyBetModel>? parleyBets;

    public GameService(IGameData gameData,
                       ITeamData teamData,
                       IBetData betData,
                       IParleyBetData parleyData,
                       IConfiguration config,
                       IHttpClientFactory httpClientFactory,
                       ILogger<GameService> logger)
    {
        _gameData = gameData;
        _teamData = teamData;
        _betData = betData;
        _parleyData = parleyData;
        _config = config;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<GameByTeamDto> GetGameByTeam(TeamModel team)
    {
        GameByTeamDto? game = new();

        try
        {
            _logger.LogInformation("Http Get / Get Game By Team");
            var client = _httpClientFactory.CreateClient("sportsdata");

            game = await client.GetFromJsonAsync<GameByTeamDto>(
                    $"odds/json/TeamTrends/{team.Symbol}?key={_config.GetSection("SportsDataIO").GetSection("Key2").Value}");

        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Exception On Get Game By Team");
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
                    $"stats/json/BoxScoreByScoreIDV3/{scoreId}?key={_config.GetSection("SportsDataIO").GetSection("Key2").Value}");
        }

        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Exception On Get Game By Score ID");
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
                    $"scores/json/ScoresByWeek/2022{currentSeason}/{week}?key={_config.GetSection("SportsDataIO").GetSection("Key2").Value}");
        }

        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Exception On Get Games By Week");
        }

        return games!;
    }

    public async Task<HashSet<GameModel>> GetGamesForThisWeek(
        SeasonType currentSeason, int currentWeek)
    {
        if (teams is null || !teams.Any())
            teams = await _teamData.GetTeams();

        if (games is null || !teams.Any())
            games = await _gameData.GetGames();

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

            GameModel gameModel = new();

            gameModel.HomeTeam = teams.Where(t =>
                t.Symbol == game.HomeTeam)?.FirstOrDefault();
            gameModel.AwayTeam = teams.Where(t =>
                t.Symbol == game.AwayTeam)?.FirstOrDefault();

            if (gameModel.HomeTeam is null || gameModel.AwayTeam is null)
                continue;

            gameModel.HomeTeamId = gameModel.HomeTeam.Id;
            gameModel.AwayTeamId = gameModel.AwayTeam.Id;
            gameModel.DateOfGame = game.DateTime;
            gameModel.DateOfGameOnly = gameModel.DateOfGame.ToString("MM-dd");
            gameModel.TimeOfGameOnly = gameModel.DateOfGame.ToString("hh:mm");
            gameModel.GameStatus = GameStatus.NOT_STARTED;
            gameModel.WeekNumber = game.Week;
            gameModel.PointSpread = Math.Round(Convert.ToDouble(game.PointSpread), 1);
            gameModel.Stadium = game.StadiumDetails.Name;
            gameModel.Season = currentSeason;
            gameModel.GameStatus = GameStatus.NOT_STARTED;
            gameModel.ScoreId = game.ScoreID;

            thisWeekGames.Add(gameModel);
            await _gameData.InsertGame(gameModel);
        }

        return thisWeekGames;
    }

    public async Task FetchAllScoresForFinishedGames()
    {
        if (teams is null || !teams.Any())
            teams = await _teamData.GetTeams();
        if (games is null || !games.Any())
            games = await _gameData.GetGames();
        if (bets is null || !bets.Any())
            bets = await _betData.GetBets();
        if (parleyBets is null || !parleyBets.Any())
            parleyBets = await _parleyData.GetParleyBets();

        SeasonType season = DateTime.Now.CalculateSeason();
        int week = season.CalculateWeek(DateTime.Now);

        HashSet<GameModel> unfinishedGamesOfCurrentWeek = games.Where(g =>
            g.WeekNumber == week && g.GameStatus != GameStatus.FINISHED)
            .ToHashSet<GameModel>();

        foreach (GameModel game in unfinishedGamesOfCurrentWeek)
        {
            GameByScoreIdDto gameLookup = await GetGameByScoreId(
                    game.ScoreId);

            if (!gameLookup.Score.IsOver)
                continue;

            if (!double.TryParse(gameLookup.Score.HomeScore.ToString(), out var homeScore))
                continue;

            if (!double.TryParse(gameLookup.Score.AwayScore.ToString(), out var awayScore))
                continue;

            game.HomeTeamFinalScore = homeScore;
            game.AwayTeamFinalScore = awayScore;

            await game.UpdateScores(
                game.HomeTeamFinalScore, game.AwayTeamFinalScore,
                    teams!, _gameData);

            await game.UpdateBettors(
                game.HomeTeamFinalScore, game.AwayTeamFinalScore,
                    _betData, games!, teams!, bets!);

            await game.UpdateTeamRecords(
                game.HomeTeamFinalScore, game.AwayTeamFinalScore,
                    teams!, _teamData);
        }

        await _parleyData.UpdateParleyBetWinners(
                    parleyBets!, games!, teams!, bets!);
    }

    public async Task GetPointSpreadUpdateForAvailableGames()
    {
        if (games is null || !games.Any())
            games = await _gameData.GetGames();

        foreach (GameModel game in games.Where(g
                    => g.GameStatus == GameStatus.NOT_STARTED))
        {
            GameByScoreIdDto gameLookup = new();

            gameLookup = await GetGameByScoreId(game.ScoreId);

            if (gameLookup.Score.HasStarted)
            {
                game.GameStatus = GameStatus.IN_PROGRESS;
                await _gameData.UpdateGame(game);
                continue;
            }

            if (game.PointSpread == Math.Round(gameLookup.Score.PointSpread, 1))
                continue;

            game.PointSpread = Math.Round(gameLookup.Score.PointSpread, 1);

            await _gameData.UpdateGame(game);
        }
    }
}

#nullable restore
