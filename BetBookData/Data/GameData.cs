using System.Data;
using BetBookData.Interfaces;
using BetBookData.Models;
using BetBookDbAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Dapper;

namespace BetBookData.Data;

#nullable enable

public class GameData : IGameData
{
    private readonly ISqlConnection _db;
    private readonly ILogger<GameData> _logger;
    private readonly IConfiguration _configuration;

    public GameData(ISqlConnection db, ILogger<GameData> logger, IConfiguration configuration)
    {
        _db = db;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<GameModel> GetCurrentGameByGameId(int _gameId)
    {
        using IDbConnection connection = new System.Data.SqlClient.SqlConnection(
            _configuration.GetConnectionString("BetBookDB"));

        string sqlQuery = $@"select * from dbo.Games where Id = {_gameId};";

        GameModel currentGame =
            await connection.QueryFirstOrDefaultAsync<GameModel>(sqlQuery);

        try
        {
            currentGame.AwayTeam =
                await connection.QueryFirstOrDefaultAsync<TeamModel>(
                    $@"select * from dbo.Teams where Id = {currentGame.AwayTeamId};");
            currentGame.HomeTeam =
                await connection.QueryFirstOrDefaultAsync<TeamModel>(
                    $@"select * from dbo.Teams where Id = {currentGame.HomeTeamId};");
        }
        catch(Exception ex)
        {
            _logger.LogInformation(ex, "Failed To Populate Current Game / GameData");
        }
        
        return currentGame;
    }

    public async Task<IEnumerable<GameModel>> GetGamesByWeekAndSeason(int _week, Season _season)
    {
        using IDbConnection connection = new System.Data.SqlClient.SqlConnection(
            _configuration.GetConnectionString("BetBookDB"));

        string sqlQueryForGamesByWeek =
            $@"select * from dbo.Games where WeekNumber = {_week} and Season = '{_season}';";
        string sqlQueryForTeams =
            $@"select * from dbo.Teams;";

        IEnumerable<GameModel> gamesByWeek =
            await connection.QueryAsync<GameModel>(sqlQueryForGamesByWeek);

        try
        {
            IEnumerable<TeamModel> teams =
            await connection.QueryAsync<TeamModel>(sqlQueryForTeams);

            foreach (GameModel game in gamesByWeek)
            {
                game.AwayTeam =
                    teams.Where(t => t.Id == game.AwayTeamId).FirstOrDefault();
                game.HomeTeam =
                    teams.Where(t => t.Id == game.HomeTeamId).FirstOrDefault();
            }
        }
        catch(Exception ex)
        {
            _logger.LogInformation(ex, "Failed To Populate Games By Week / GameData");
        }

        return gamesByWeek;
    }

    public async Task<int> InsertGame(GameModel _game)
    {
        _logger.LogInformation("Calling Insert Game...");

        using IDbConnection connection = new System.Data.SqlClient.SqlConnection(
            _configuration.GetConnectionString("BetBookDB"));

        var parameters = new DynamicParameters();

        parameters.Add("@HomeTeamId", _game.HomeTeamId);
        parameters.Add("@AwayTeamId", _game.AwayTeamId);
        parameters.Add("@Stadium", _game.Stadium);
        parameters.Add("@PointSpread", _game.PointSpread);
        parameters.Add("@WeekNumber", _game.WeekNumber);
        parameters.Add("@Season", _game.Season.ToStringFast());
        parameters.Add("@DateOfGame", _game.DateOfGame);
        parameters.Add("@GameStatus", _game.GameStatus.ToStringFast());
        parameters.Add("@ScoreId", _game.ScoreId);
        parameters.Add("@DateOfGameOnly", _game.DateOfGameOnly);
        parameters.Add("@TimeOfGameOnly", _game.TimeOfGameOnly);
        parameters.Add("@Id", 0, dbType: DbType.Int32,
            direction: ParameterDirection.Output);

        string insertBetSqlQuery =
                $@"insert into dbo.Games 
                    (HomeTeamId, AwayTeamId, Stadium, PointSpread, WeekNumber, Season, DateOfGame, GameStatus, ScoreId, DateOfGameOnly, TimeOfGameOnly)
                    output Inserted.Id
                    values (@HomeTeamId, @AwayTeamId, @Stadium, @PointSpread, @WeekNumber, @Season, @DateOfGame, @GameStatus, @ScoreId, @DateOfGameOnly, @TimeOfGameOnly);";

        _game.Id = await connection.QuerySingleAsync<int>(insertBetSqlQuery, parameters);


        return _game.Id;
    }

    public async Task UpdateGame(GameModel game)
    {
        string season = game.Season.ToStringFast();
        string gameStatus = game.GameStatus.ToStringFast();

        _logger.LogInformation("Update Game Call");

        try
        {
            await _db.SaveData("dbo.spGames_Update", new
            {
                game.Id,
                game.HomeTeamId,
                game.AwayTeamId,
                game.Stadium,
                game.PointSpread,
                game.WeekNumber,
                game.HomeTeamFinalScore,
                game.AwayTeamFinalScore,
                game.GameWinnerId,
                season,
                game.DateOfGame,
                gameStatus,
                game.ScoreId,
                game.DateOfGameOnly,
                game.TimeOfGameOnly

            });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Failed To Update Game / GameData");
        }
    }
}


#nullable restore

