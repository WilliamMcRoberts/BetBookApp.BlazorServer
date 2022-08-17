using BetBookData.DbAccess;
using BetBookData.Interfaces;
using BetBookData.Models;

namespace BetBookData.Data;

#nullable enable

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
    public async Task<IEnumerable<GameModel>> GetGames() => 
        await _db.LoadData<GameModel, dynamic>("dbo.spGames_GetAll", new{});

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
        string seasonType = game.Season.ToString();
        string gameStatus = game.GameStatus.ToString();

        await _db.SaveData("dbo.spGames_Insert", new
        {
            game.HomeTeamId,
            game.AwayTeamId,
            game.Stadium,
            game.PointSpread,
            game.WeekNumber,
            seasonType,
            game.DateOfGame,
            gameStatus,
            game.ScoreId,
            game.DateOfGameOnly,
            game.TimeOfGameOnly
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
            game.Stadium,
            game.PointSpread,
            game.HomeTeamFinalScore,
            game.AwayTeamFinalScore,
            game.GameWinnerId,
            seasonType,
            game.DateOfGame,
            gameStatus,
            game.ScoreId,
            game.DateOfGameOnly,
            game.TimeOfGameOnly
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


#nullable restore

