using BetBookData.DataLogic.Interfaces;
using BetBookData.DbAccess;
using BetBookData.Models;

namespace BetBookData.DataLogic;

public class GameData : IGameData
{

    private readonly ISqlConnection _db;

    /// <summary>
    /// GameData Constructor
    /// </summary>
    /// <param name="db">ISqlConnection represents SqlConnection class interface</param>
    public GameData(ISqlConnection db)
    {
        _db = db;
    }

    /// <summary>
    /// Async method calls the spGames_GetAll stored procedure to retrieve 
    /// all games in the database
    /// </summary>
    /// <returns>
    /// IEnumerable of GameModel representing all games in the database
    /// </returns>
    public async Task<IEnumerable<GameModel>> GetGames()
    {
        return await _db.LoadData<GameModel, dynamic>(
        "dbo.spGames_GetAll", new { });
    }

    /// <summary>
    /// Async method calls the spGames_GetAllByWeek stored procedure to retrieve 
    /// all games in the database by a certain week
    /// </summary>
    /// <param name="weekNumber">int represents a week in the NFL season</param>
    /// <returns>
    /// IEnumerable of GameModel represents all games in a specified week
    /// </returns>
    public async Task<IEnumerable<GameModel>> GetGamesByWeek(int weekNumber)
    {
        return await _db.LoadData<GameModel, dynamic>(
        "dbo.spGames_GetAllByWeek", new
        {
            WeekNumber = weekNumber
        });
    }

    /// <summary>
    /// Async method calls the spGames_GetAllFinished stored procedure to retrieve 
    /// all games that have a "FINISHED" status
    /// </summary>
    /// <returns>
    /// IEnumerable of GameModel represents all games with game status "FINISHED"
    /// </returns>
    public async Task<IEnumerable<GameModel>> GetAllFinishedGames()
    {
        return await _db.LoadData<GameModel, dynamic>(
        "dbo.spGames_GetAllFinished", new { });
    }

    /// <summary>
    /// Async method calls the spGames_GetAllInProgress stored procedure to retrieve 
    /// all games that have an "IN_PROGRESS" status
    /// </summary>
    /// <returns>
    /// IEnumerable of GameModel represents all games with game status "IN_PROGRESS"
    /// </returns>
    public async Task<IEnumerable<GameModel>> GetAllInProgressGames()
    {
        return await _db.LoadData<GameModel, dynamic>(
        "dbo.spGames_GetAllInProgress", new { });
    }

    /// <summary>
    /// Async method calls the spGames_GetAllNotStarted stored procedure to retrieve 
    /// all games that have a "NOT_STARTED" status
    /// </summary>
    /// <returns>
    /// IEnumerable of GameModel represents all games with game status "NOT_STARTED"
    /// </returns>
    public async Task<IEnumerable<GameModel>> GetAllNotStartedGames()
    {
        return await _db.LoadData<GameModel, dynamic>(
        "dbo.spGames_GetAllNotStarted", new { });
    }

    /// <summary>
    /// Async method calls spGames_Get stored procedure which retrieves one 
    /// game by game id
    /// </summary>
    /// <param name="id">int represents the Id of a game</param>
    /// <returns>
    /// GameModel represents a game to retrieve from database
    /// </returns>
    public async Task<GameModel?> GetGame(int id)
    {
        var results = await _db.LoadData<GameModel, dynamic>(
            "dbo.spGames_Get", new
            {
                Id = id
            });

        return results.FirstOrDefault();
    }

    /// <summary>
    /// Async method calls the spGames_Insert stored procedure to insert one game 
    /// entry into the database
    /// </summary>
    /// <param name="game">GameModel represents a game to insert into the database</param>
    /// <returns></returns>
    public async Task InsertGame(GameModel game)
    {
        int weekNumber = game.Week;
        string seasonType = game.Season.ToString();
        string gameStatus = game.GameStatus.ToString();

        await _db.SaveData("dbo.spGames_Insert", new
        {
            game.HomeTeamId,
            game.AwayTeamId,
            game.FavoriteId,
            game.UnderdogId,
            game.Stadium,
            game.PointSpread,
            weekNumber,
            seasonType,
            game.DateOfGame,
            gameStatus
        });
    }

    /// <summary>
    /// Async method calls the spGames_Update stored procedure to update a game
    /// </summary>
    /// <param name="game">GameModel represents a game to update in the database</param>
    /// <returns></returns>
    public async Task UpdateGame(GameModel game)
    {
        string seasonType = game.Season.ToString();
        string gameStatus = game.GameStatus.ToString();

        await _db.SaveData("dbo.spGames_Update", new
        {
            game.Id,
            game.HomeTeamId,
            game.AwayTeamId,
            game.FavoriteId,
            game.UnderdogId,
            game.Stadium,
            game.PointSpread,
            game.FavoriteFinalScore,
            game.UnderdogFinalScore,
            seasonType,
            game.DateOfGame,
            gameStatus
        });
    }

    /// <summary>
    /// Async method calls the spGames_AddWinner stored procedure to declare the 
    /// winner of a game
    /// </summary>
    /// <param name="game">GameModel represents a game to update with the winner</param>
    /// <returns></returns>
    public async Task AddGameWinner(GameModel game, TeamModel team)
    {
        int gameWinnerId = team.Id;

        await _db.SaveData("dbo.spGames_AddWinner", new
        {
            game.Id,
            gameWinnerId
        });
    }

    /// <summary>
    /// Async method calls the spGames_Delete stored procedure which deletes one game
    /// entry in the database
    /// </summary>
    /// <param name="id">int represents the Id of a game to delete from database</param>
    /// <returns></returns>
    public async Task DeleteGame(int id)
    {
        await _db.SaveData(
        "dbo.spGames_Delete", new
        {
            Id = id
        });
    }
}




