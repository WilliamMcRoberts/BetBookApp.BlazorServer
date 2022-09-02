using BetBookData.Models;

namespace BetBookData.Interfaces;

public interface IGameData
{
    Task<GameModel> GetCurrentGameByGameId(int _gameId);
    Task<IEnumerable<GameModel>> GetGamesByWeekAndSeason(int _week, Season _season);
    Task<int> InsertGame(GameModel _game);
    Task UpdateGame(GameModel game);
}