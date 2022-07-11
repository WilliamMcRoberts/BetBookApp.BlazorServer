using BetBookData.Models;

namespace BetBookData.Interfaces;
public interface IBetData
{
    Task DeleteBet(int id);
    Task<BetModel?> GetBet(int betId);
    Task<IEnumerable<BetModel>> GetBets();
    Task<int> InsertBet(BetModel bet);
    Task UpdateBet(BetModel bet);
}