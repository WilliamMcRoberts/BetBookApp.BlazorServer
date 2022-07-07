using BetBookData.Models;

namespace BetBookData.Interfaces;
public interface IBetData
{
    Task DeleteBet(int bettorId);
    Task<IEnumerable<BetModel>> GetAllBetsOnGame(int gameId);
    Task<IEnumerable<BetModel>> GetAllBettorBets(int bettorId);
    Task<BetModel?> GetBet(int betId);
    Task<IEnumerable<BetModel>> GetBets();
    Task InsertBet(BetModel bet);
    Task UpdateBet(BetModel bet);
}
