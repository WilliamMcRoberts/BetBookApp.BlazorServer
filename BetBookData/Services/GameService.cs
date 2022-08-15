
using System.Text.Json;
using BetBookData.Interfaces;
using BetBookData.Lookups;
using BetBookData.Models;
using BetBookData.Helpers;
using Microsoft.Extensions.Configuration;


namespace BetBookData.Services;

#nullable enable

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

    public async Task<GameByTeamDto> GetGameByTeamLookup(TeamModel team)
    {
        GameByTeamDto? teamLookup = new();

        try
        {
            var response = await _httpClient.GetAsync(
                    $"https://api.sportsdata.io/v3/nfl/odds/json/TeamTrends/" +
                    $"{team.Symbol}?key={_config.GetSection("SportsDataIO").GetSection("Key").Value}");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                teamLookup = JsonSerializer.Deserialize<GameByTeamDto>(json);
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);
        }

        return teamLookup!;
    }

    public async Task<GameByScoreIdDto> GetGameByScoreIdLookup(int scoreId)
    {
        GameByScoreIdDto? gameLookup = new();

        try
        {
            var response = await _httpClient.GetAsync(
                    $"https://api.sportsdata.io/v3/nfl/stats/json/BoxScoreByScoreIDV3/" +
                    $"{scoreId}?key={_config.GetSection("SportsDataIO").GetSection("Key").Value}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                gameLookup = JsonSerializer.Deserialize<GameByScoreIdDto>(json);
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

        Game[]? games = new Game[16];

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
            if (game.PointSpread is null)
            {
                continue;
            }

            GameModel gameModel = new();

            gameModel.HomeTeam = teams.Where(t =>
                t.Symbol == game.HomeTeam)?.FirstOrDefault();
            gameModel.AwayTeam = teams.Where(t =>
                t.Symbol == game.AwayTeam)?.FirstOrDefault();

            if (gameModel.HomeTeam is null || gameModel.AwayTeam is null)
            {
                continue;
            }

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
            GameByScoreIdDto gameLookup = await GetGameByScoreIdLookup(
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
            GameByScoreIdDto gameLookup = new();

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

#nullable restore
