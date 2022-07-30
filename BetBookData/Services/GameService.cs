
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using BetBookData.Interfaces;
using BetBookData.Lookups;
using BetBookData.Models;
using BetBookData.Helpers;

namespace BetBookData.Services;
public class GameService : IGameService
{
    private readonly HttpClient _httpClient;
    private readonly IGameData _gameData;
    private readonly ITeamData _teamData;
    private readonly IBetData _betData;
    private readonly IParleyBetData _parleyData;
    IEnumerable<TeamModel>? teams;
    IEnumerable<GameModel>? games;
    IEnumerable<BetModel>? bets;
    IEnumerable<ParleyBetModel>? parleyBets;

    public GameService(IGameData gameData, ITeamData teamData,
            IBetData betData, IParleyBetData parleyData)
    {
        _httpClient = new();
        _gameData = gameData;
        _teamData = teamData;
        _betData = betData;
        _parleyData = parleyData;
    }

    public async Task<TeamLookup> GetGameByTeamLookup(TeamModel team)
    {
        TeamLookup? teamLookup = new();

        try
        {
            var response = await _httpClient.GetAsync(
                    $"https://api.sportsdata.io/v3/nfl/odds/json/TeamTrends/{team.Symbol}?key=631ffaf2cf5d4555a9a1f7ba2d74a0f3");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                teamLookup = JsonSerializer.Deserialize<TeamLookup>(json);
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);
        }

        return teamLookup!;
    }

    public async Task<GameLookup> GetGameByScoreIdLookup(int scoreId)
    {
        GameLookup? gameLookup = new();

        try
        {
            var response = await _httpClient.GetAsync(
                    $"https://api.sportsdata.io/v3/nfl/stats/json/BoxScoreByScoreIDV3/{scoreId}?key=631ffaf2cf5d4555a9a1f7ba2d74a0f3");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                gameLookup = JsonSerializer.Deserialize<GameLookup>(json);
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
                    $"https://api.sportsdata.io/v3/nfl/scores/json/ScoresByWeek/2022{currentSeason}/{week}?key=631ffaf2cf5d4555a9a1f7ba2d74a0f3");

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

        int nextWeek;

        if (currentSeason == SeasonType.PRE && currentWeek == 4)
        {
            currentSeason = SeasonType.REG;
            nextWeek = 1;
        }

        else if (currentSeason == SeasonType.REG && currentWeek == 17)
        {
            currentSeason = SeasonType.POST;
            nextWeek = 1;
        }

        else
            nextWeek = currentWeek + 1;

        Game[] gameArray = new Game[16];

        try
        {
            gameArray = await GetGamesByWeekLookup(currentSeason, nextWeek);
        }

        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);
        }

        if (gameArray.Length > 0)
        {
            foreach (Game g in gameArray)
            {
                GameModel gameModel = new();

                gameModel.HomeTeam = teams.Where(t =>
                    t.Symbol == g.HomeTeam)?.FirstOrDefault()!;
                gameModel.AwayTeam = teams.Where(t =>
                    t.Symbol == g.AwayTeam)?.FirstOrDefault()!;
                gameModel.HomeTeamId = gameModel.HomeTeam.Id;
                gameModel.AwayTeamId = gameModel.AwayTeam.Id;
                gameModel.DateOfGame = g.DateTime;
                gameModel.DateOfGameOnly = gameModel.DateOfGame.ToString("MM-dd");
                gameModel.TimeOfGameOnly = gameModel.DateOfGame.ToString("hh:mm");
                gameModel.GameStatus = GameStatus.NOT_STARTED;
                gameModel.WeekNumber = g.Week;
                gameModel.PointSpread = Math.Round(g.PointSpread, 1);
                gameModel.Stadium = g.StadiumDetails.Name;
                gameModel.Season = currentSeason;
                gameModel.GameStatus = GameStatus.NOT_STARTED;
                gameModel.ScoreId = g.ScoreID;

                if (gameModel.PointSpread <= 0)
                {
                    gameModel.Favorite = gameModel.HomeTeam;
                    gameModel.FavoriteId = gameModel.Favorite.Id;
                    gameModel.Underdog = gameModel.AwayTeam;
                    gameModel.UnderdogId = gameModel.Underdog.Id;
                }

                else
                {
                    gameModel.Favorite = gameModel.AwayTeam;
                    gameModel.FavoriteId = gameModel.AwayTeam.Id;
                    gameModel.Underdog = gameModel.HomeTeam;
                    gameModel.UnderdogId = gameModel.Underdog.Id;
                }

                nextWeekGames.Add(gameModel);

                await _gameData.InsertGame(gameModel);
            }
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
            GameLookup gameLookup = new();

            gameLookup = await GetGameByScoreIdLookup(game.ScoreId);

            if (gameLookup.Score.IsOver)
            {
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
            GameLookup gameLookup = new();

            gameLookup = await GetGameByScoreIdLookup(game.ScoreId);

            if (gameLookup.Score.HasStarted == false)
            {
                if (Math.Round(gameLookup.Score.PointSpread, 1) != game.PointSpread)
                    game.PointSpread = Math.Round(gameLookup.Score.PointSpread, 1);

                await _gameData.UpdateGame(game);
            }
        }
    }
}
