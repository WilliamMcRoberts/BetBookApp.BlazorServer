using BetBookData.Models;

namespace BetBookData.Interfaces;
public interface IGameData
{
    Task DeleteGame(int id);
    Task<GameModel?> GetGame(int id);
    Task<IEnumerable<GameModel>> GetGames();
    Task InsertGame(GameModel game);
    Task UpdateGame(GameModel game);
}