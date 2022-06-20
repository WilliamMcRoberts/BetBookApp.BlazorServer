using BetBookDataLogic.Models;

namespace BetBookDataLogic.Data.Interfaces;
public interface IBetData
{
    Task DeleteBet(int id);
    Task<IEnumerable<BetModel>> GetAllBettorBets(int id);
    Task<IEnumerable<BetModel>> GetAllBettorLosingBets(int id);
    Task<IEnumerable<BetModel>> GetAllBettorWinningBets(int id);
    Task<BetModel?> GetBet(int id);
    Task<IEnumerable<BetModel>> GetBets();
    Task<IEnumerable<BetModel>> GetAllBettorInProgressBets(int id);
    Task InsertBet(BetModel bet);
    Task UpdateBet(BetModel bet);
}
