using BetBookDataLogic.Models;

namespace BetBookDataLogic.Data.Interfaces;
public interface IBetData
{
    Task DeleteBet(int id);
    Task<IEnumerable<BetModel>> GetAllBetterBets(int id);
    Task<IEnumerable<BetModel>> GetAllBetterLosingBets(int id);
    Task<IEnumerable<BetModel>> GetAllBetterWinningBets(int id);
    Task<BetModel?> GetBet(int id);
    Task<IEnumerable<BetModel>> GetBets();
    Task<IEnumerable<BetModel>> GetAllBetterInProgressBets(int id);
    Task InsertBet(BetModel bet);
    Task UpdateBet(BetModel bet);
}
