
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using BetBookData.Interfaces;
using BetBookData.Lookups;
using BetBookData.Models;
using BetBookData.Helpers;
using Microsoft.Extensions.Configuration;

namespace BetBookData.Services;
public class GameService : IGameService
{
    private readonly HttpClient _httpClient;
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
                       IConfiguration config)
    {
        _httpClient = new();
        _gameData = gameData;
        _teamData = teamData;
        _betData = betData;
        _parleyData = parleyData;
        _config = config;
    }

    public async Task<GameByTeamLookup> GetGameByTeamLookup(TeamModel team)
    {
        GameByTeamLookup? teamLookup = new();

        try
        {
            var response = await _httpClient.GetAsync(
                    $"https://api.sportsdata.io/v3/nfl/odds/json/TeamTrends/" +
                    $"{team.Symbol}?key={_config.GetSection("SportsDataIO").GetSection("Key").Value}");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                teamLookup = JsonSerializer.Deserialize<GameByTeamLookup>(json);
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);
        }

        return teamLookup!;
    }

    public async Task<GameByScoreIdLookup> GetGameByScoreIdLookup(int scoreId)
    {
        GameByScoreIdLookup? gameLookup = new();

        try
        {
            var response = await _httpClient.GetAsync(
                    $"https://api.sportsdata.io/v3/nfl/stats/json/BoxScoreByScoreIDV3/" +
                    $"{scoreId}?key={_config.GetSection("SportsDataIO").GetSection("Key").Value}");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                gameLookup = JsonSerializer.Deserialize<GameByScoreIdLookup>(json);
            }
        }

        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);
        }

        return gameLookup!;
    }

    public async Task<Game[]> GetGamesByWeekLookup(SeasonType currentSeason, int week)
    {
        Game[] games = new Game[16];

        try
        {
            var response = await _httpClient.GetAsync(
                    $"https://api.sportsdata.io/v3/nfl/scores/json/ScoresByWeek/2022{currentSeason}" +
                    $"/{week}?key={_config.GetSection("SportsDataIO").GetSection("Key").Value}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsByteArrayAsync();

                games = JsonSerializer.Deserialize<Game[]>(data)!;
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return games;
    }

    public async Task<List<GameModel>> GetGamesForNextWeek(
        SeasonType currentSeason, int currentWeek)
    {
        if (teams is null || !teams.Any())
            teams = await _teamData.GetTeams();

        List<GameModel> nextWeekGames = new();

        (int, SeasonType) nextWeekSeason =
            (currentWeek, currentSeason).CalculateNextWeekAndSeasonFromCurrentWeekAndSeason();

        Game[] gameArray = new Game[16];

        try
        {
            gameArray = await GetGamesByWeekLookup(
                nextWeekSeason.Item2, nextWeekSeason.Item1);
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        foreach (Game game in gameArray)
        {
            GameModel gameModel = new();

            gameModel.HomeTeam = teams.Where(t =>
                t.Symbol == game.HomeTeam)?.FirstOrDefault()!;
            gameModel.AwayTeam = teams.Where(t =>
                t.Symbol == game.AwayTeam)?.FirstOrDefault()!;
            gameModel.HomeTeamId = gameModel.HomeTeam.Id;
            gameModel.AwayTeamId = gameModel.AwayTeam.Id;
            gameModel.DateOfGame = game.DateTime;
            gameModel.DateOfGameOnly = gameModel.DateOfGame.ToString("MM-dd");
            gameModel.TimeOfGameOnly = gameModel.DateOfGame.ToString("hh:mm");
            gameModel.GameStatus = GameStatus.NOT_STARTED;
            gameModel.WeekNumber = game.Week;
            gameModel.PointSpread = Math.Round(game.PointSpread, 1);
            gameModel.Stadium = game.StadiumDetails.Name;
            gameModel.Season = currentSeason;
            gameModel.GameStatus = GameStatus.NOT_STARTED;
            gameModel.ScoreId = game.ScoreID;

            if (gameModel.PointSpread > 0)
            {
                gameModel.Favorite = gameModel.AwayTeam;
                gameModel.FavoriteId = gameModel.AwayTeam.Id;
                gameModel.Underdog = gameModel.HomeTeam;
                gameModel.UnderdogId = gameModel.Underdog.Id;
                nextWeekGames.Add(gameModel);
                await _gameData.InsertGame(gameModel);
                continue;
            }

            gameModel.Favorite = gameModel.HomeTeam;
            gameModel.FavoriteId = gameModel.Favorite.Id;
            gameModel.Underdog = gameModel.AwayTeam;
            gameModel.UnderdogId = gameModel.Underdog.Id;
            nextWeekGames.Add(gameModel);
            await _gameData.InsertGame(gameModel);
        }

        return nextWeekGames;
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

        foreach (GameModel game in games!.Where(g =>
            g.GameStatus != GameStatus.FINISHED))
        {
            GameByScoreIdLookup gameLookup = await GetGameByScoreIdLookup(
                    game.ScoreId);

            if (gameLookup.Score.IsOver == false)
                continue;

            game.HomeTeamFinalScore = (double)gameLookup.Score.HomeScore;
            game.AwayTeamFinalScore = (double)gameLookup.Score.AwayScore;

            await game.UpdateScores(
                game.HomeTeamFinalScore, game.AwayTeamFinalScore,
                    teams!, _gameData);

            await game.UpdateBetWinners(
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
            GameByScoreIdLookup gameLookup = new();

            gameLookup = await GetGameByScoreIdLookup(game.ScoreId);

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
