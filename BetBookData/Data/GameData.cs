using BetBookData.Interfaces;
using BetBookData.Models;
using BetBookDbAccess;
using Microsoft.Extensions.Logging;

namespace BetBookData.Data;

#nullable enable

public class GameData : IGameData
{
    private readonly ISqlConnection _db;
    private readonly ILogger<GameData> _logger;

    public GameData(ISqlConnection db, ILogger<GameData> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<GameModel>> GetGames() 
    {
        _logger.LogInformation( "Get Games Call");

        return await _db.LoadData<GameModel, dynamic>(
            "dbo.spGames_GetAll", new { });
    }
        
    public async Task<GameModel?> GetGame(int id)
    {
        _logger.LogInformation( "Get Game Call");

        var results = await _db.LoadData<GameModel, dynamic>(
            "dbo.spGames_Get", new
            {
                Id = id
            });

        return results.FirstOrDefault();
    }

    public async Task InsertGame(GameModel game)
    {
        string seasonType = game.Season.ToStringFast();
        string gameStatus = game.GameStatus.ToStringFast();

        _logger.LogInformation( "Insert Game Call");

        try
        {
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
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Failed To Insert Game");
        }
    }

    public async Task UpdateGame(GameModel game)
    {
        string seasonType = game.Season.ToStringFast();
        string gameStatus = game.GameStatus.ToStringFast();

        _logger.LogInformation( "Update Game Call");

        try
        {
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
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Failed To Update Game");
        }
    }

    public async Task DeleteGame(int id)
    {
        _logger.LogInformation( "Delete Game Call");

        try
        {
            await _db.SaveData( "dbo.spGames_Delete", new
            {
                Id = id
            });
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Failed To Delete Game");

        }
    }
}


#nullable restore

