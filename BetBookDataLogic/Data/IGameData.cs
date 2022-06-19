using BetBookDataLogic.Models;

namespace BetBookDataLogic.Data;
public interface IGameData
{
    Task AddGameWinner(GameModel game, TeamModel team);
    Task DeleteGame(int id);
    Task<IEnumerable<GameModel>> GetAllFinishedGames();
    Task<IEnumerable<GameModel>> GetAllInProgressGames();
    Task<IEnumerable<GameModel>> GetAllNotStartedGames();
    Task<GameModel?> GetGame(int id);
    Task<IEnumerable<GameModel>> GetGames();
    Task<IEnumerable<GameModel>> GetGamesByWeek(int weekNumber);
    Task InsertGame(GameModel game);
    Task UpdateGame(GameModel game);
}
