﻿
using System.Text.Json;
using BetBookData.Interfaces;
using BetBookData.Lookups;
using BetBookData.Models;
using BetBookData.Helpers;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;

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
    IEnumerable<TeamModel>? teams;
    IEnumerable<GameModel>? games;
    IEnumerable<BetModel>? bets;
    IEnumerable<ParleyBetModel>? parleyBets;

    public GameService(IGameData gameData,
                       ITeamData teamData,
                       IBetData betData,
                       IParleyBetData parleyData,
                       IConfiguration config,
                       IHttpClientFactory httpClientFactory)
    {
        _gameData = gameData;
        _teamData = teamData;
        _betData = betData;
        _parleyData = parleyData;
        _config = config;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<GameByTeamDto> GetGameByTeam(TeamModel team)
    {
        GameByTeamDto? game = new();

        try
        {
            var client = _httpClientFactory.CreateClient("sportsdata");

            game = await client.GetFromJsonAsync<GameByTeamDto>(
                    $"odds/json/TeamTrends/{team.Symbol}?key={_config.GetSection("SportsDataIO").GetSection("Key").Value}");

        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);
        }

        return game!;
    }

    public async Task<GameByScoreIdDto> GetGameByScoreId(int scoreId)
    {
        GameByScoreIdDto? game = new();

        try
        {
            var client = _httpClientFactory.CreateClient("sportsdata");

            game = await client.GetFromJsonAsync<GameByScoreIdDto>(
                    $"stats/json/BoxScoreByScoreIDV3/{scoreId}?key={_config.GetSection("SportsDataIO").GetSection("Key").Value}");
        }

        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);
        }

        return game!;
    }

    public async Task<Game[]> GetGamesByWeek(SeasonType currentSeason, int week)
    {

        Game[]? games = new Game[16];

        try
        {
            var client = _httpClientFactory.CreateClient("sportsdata");

            games = await client.GetFromJsonAsync<Game[]>(
                    $"scores/json/ScoresByWeek/2022{currentSeason}/{week}?key={_config.GetSection("SportsDataIO").GetSection("Key").Value}");
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
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

        HashSet<GameModel> thisWeekGames =
            games.Where(g => g.GameStatus != GameStatus.FINISHED).ToHashSet<GameModel>();

        Game[] gameArray = new Game[16];

        try
        {
            gameArray = await GetGamesByWeek(
                currentSeason, currentWeek);
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        foreach (Game game in gameArray)
        {
            if (game.PointSpread is null)
                continue;

            if (thisWeekGames.Contains(games.Where(g => g.ScoreId == game.ScoreID).FirstOrDefault()!))
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

        foreach (GameModel game in games!.Where(g => g.WeekNumber == week &&
            g.GameStatus != GameStatus.FINISHED))
        {
            GameByScoreIdDto gameLookup = await GetGameByScoreId(
                    game.ScoreId);

            if (gameLookup.Score.IsOver == false)
                continue;
            if (double.TryParse(gameLookup.Score.HomeScore.ToString(), out var homeScore) == false)
                continue;
            if (double.TryParse(gameLookup.Score.AwayScore.ToString(), out var awayScore) == false)
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

            if (Math.Round(gameLookup.Score.PointSpread, 1) != game.PointSpread)
                    game.PointSpread = Math.Round(gameLookup.Score.PointSpread, 1);

            await _gameData.UpdateGame(game);
        }
    }
}

#nullable restore
